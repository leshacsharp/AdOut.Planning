﻿using AdOut.Planning.Common.Helpers;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
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
    public class AdManager : BaseManager<Ad>, IAdManager
    {
        private readonly IAdRepository _adRepository;
        private readonly IPlanAdRepository _planAdRepository;
        private readonly IContentStorage _contentStorage;
        private readonly IContentValidatorProvider _contentValidatorProvider;
        private readonly IContentHelperProvider _contentHelperProvider;
            
        public AdManager(
            IAdRepository adRepository,
            IPlanAdRepository planAdRepository,
            IContentStorage contentStorage,
            IContentValidatorProvider contentValidatorProvider,
            IContentHelperProvider contentHelperProvider) 
            : base(adRepository)
        {
            _adRepository = adRepository;
            _planAdRepository = planAdRepository;
            _contentStorage = contentStorage;
            _contentValidatorProvider = contentValidatorProvider;
            _contentHelperProvider = contentHelperProvider;
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

        public async Task CreateAsync(CreateAdModel createModel, string userId)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }

            var extension = Path.GetExtension(createModel.Content.FileName);
            var contentStream = createModel.Content.OpenReadStream();

            var contentHelper = _contentHelperProvider.CreateContentHelper(extension);
            var thumbnail = contentHelper.GetThumbnail(contentStream, DefaultValues.DefaultThumbnailWidth, DefaultValues.DefaultThumbnailHeight);

            var pathForContent = PathHelper.GeneratePath(extension, Model.Enum.DirectoryPath.None);
            var pathForThumbnail = PathHelper.GeneratePath(DefaultValues.DefaultThumbnailExtension, Model.Enum.DirectoryPath.None);

            var saveContentTask = _contentStorage.CreateObjectAsync(contentStream, pathForContent);
            var saveThumbnailTask = _contentStorage.CreateObjectAsync(thumbnail, pathForThumbnail);

            await Task.WhenAll(saveContentTask, saveThumbnailTask);

            var ad = new Ad()
            {
                UserId = userId,
                Title = createModel.Title,
                Path = pathForContent,
                PreviewPath = pathForThumbnail,
                Status = Model.Enum.AdStatus.OnModeration,
                ContentType = ContentTypes[extension]
            };

            Create(ad);
        }

        public Task<List<AdListDto>> GetAdsAsync(AdsFilterModel filterModel, string userId)
        {
            if (filterModel == null)
            {
                throw new ArgumentNullException(nameof(filterModel));
            }

            var filter = PredicateBuilder.New<Ad>(ad => ad.UserId == userId);

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

            Update(ad);
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

            Delete(ad);
        }

        public Task<AdDto> GetDtoByIdAsync(string adId)
        {
            return _adRepository.GetDtoByIdAsync(adId);
        }

        public async Task<Ad> GetByIdAsync(string adId)
        {
            return await _adRepository.GetByIdAsync(adId);
        }
    }
}
