using AdOut.Extensions.Exceptions;
using AdOut.Planning.Core.Helpers;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Services;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Managers
{
    public class AdManager : IAdManager
    {
        private readonly IAdRepository _adRepository;
        private readonly IPlanAdRepository _planAdRepository;
        private readonly IUserService _userManager;
        private readonly IContentStorage _contentStorage;
        private readonly IContentValidatorProvider _contentValidatorProvider;
        private readonly IContentServiceProvider _contentServiceProvider;
            
        public AdManager(
            IAdRepository adRepository,
            IPlanAdRepository planAdRepository,
            IUserService userManager,
            IContentStorage contentStorage,
            IContentValidatorProvider contentValidatorProvider,
            IContentServiceProvider contentServiceProvider) 
        {
            _adRepository = adRepository;
            _planAdRepository = planAdRepository;
            _userManager = userManager;
            _contentStorage = contentStorage;
            _contentValidatorProvider = contentValidatorProvider;
            _contentServiceProvider = contentServiceProvider;
        }

        public Task<ValidationResult<string>> ValidateAsync(IFormFile content)
        {
            if(content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var extension = Path.GetExtension(content.FileName);
            var isAllowedExtension = AllowedExtensions.Contains(extension);

            if (!isAllowedExtension)
            {
                throw new BadRequestException($"{extension} extension is not allowed");
            }

            var contentValidator = _contentValidatorProvider.CreateContentValidator(extension);
            var contentStream = content.OpenReadStream();

            return contentValidator.ValidateAsync(contentStream);
        }

        public async Task CreateAsync(CreateAdModel createModel)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }

            var extension = Path.GetExtension(createModel.Content.FileName);
            var contentStream = createModel.Content.OpenReadStream();

            var contentService = _contentServiceProvider.CreateContentService(extension);
            var thumbnail = contentService.GetThumbnail(contentStream, DefaultValues.DefaultThumbnailWidth, DefaultValues.DefaultThumbnailHeight);

            var pathForContent = PathHelper.GeneratePath(extension, Model.Enum.DirectoryPath.None);
            var pathForThumbnail = PathHelper.GeneratePath(DefaultValues.DefaultThumbnailExtension, Model.Enum.DirectoryPath.None);

            var saveContentTask = _contentStorage.CreateObjectAsync(contentStream, pathForContent);
            var saveThumbnailTask = _contentStorage.CreateObjectAsync(thumbnail, pathForThumbnail);

            await Task.WhenAll(saveContentTask, saveThumbnailTask);

            var ad = new Ad()
            {
                Title = createModel.Title,
                Path = pathForContent,
                PreviewPath = pathForThumbnail,
                Status = Model.Enum.AdStatus.OnModeration,
                ContentType = ContentTypes[extension]
            };

            _adRepository.Create(ad);
        }

        public Task<List<AdListDto>> GetAdsAsync(AdsFilterModel filterModel)
        {
            if (filterModel == null)
            {
                throw new ArgumentNullException(nameof(filterModel));
            }

            var userId = _userManager.GetUserId();
            var filter = PredicateBuilder.New<Ad>(ad => ad.Creator == userId);

            if (filterModel.Title != null)
            {
                filter = filter.And(ad => EF.Functions.Like(ad.Title, $"%{filterModel.Title}%"));
            }

            if (filterModel.ContentType != null)
            {
                filter = filter.And(ad => ad.ContentType == filterModel.ContentType);
            }

            if (filterModel.Status != null)
            {
                filter = filter.And(ad => ad.Status == filterModel.Status);
            }

            if (filterModel.FromDate != null)
            {
                filter = filter.And(ad => ad.AddedDate >= filterModel.FromDate);
            }

            if (filterModel.ToDate != null)
            {
                filter = filter.And(ad => ad.AddedDate <= filterModel.ToDate);
            }

            return _adRepository.GetAds(filter);
        }
        
        public async Task UpdateAsync(UpdateAdModel updateModel)
        {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel));
            }

            var ad = await _adRepository.GetByIdAsync(updateModel.AdId);
            if (ad == null)
            {
                throw new ObjectNotFoundException($"Ad with id={updateModel.AdId} was not found");
            }

            ad.Title = updateModel.Title;

            _adRepository.Update(ad);
        }

        public async Task DeleteAsync(string adId)
        {
            var ad = await _adRepository.GetByIdAsync(adId);
            if (ad == null)
            {
                throw new ObjectNotFoundException($"Ad with id={adId} was not found");
            }

            var havePlans = await _planAdRepository.Read(pa => pa.AdId == adId).AnyAsync();
            if (havePlans)
            {
                throw new BadRequestException($"Ad with id={adId} is used in plans");
            }

            var deleteContentTask = _contentStorage.DeleteObjectAsync(ad.Path);
            var deletePreviewContentTask = _contentStorage.DeleteObjectAsync(ad.PreviewPath);
            await Task.WhenAll(deleteContentTask, deletePreviewContentTask);

            _adRepository.Delete(ad);
        }

        public Task<AdDto> GetDtoByIdAsync(string adId)
        {
            return _adRepository.GetDtoByIdAsync(adId);
        }
    }
}
