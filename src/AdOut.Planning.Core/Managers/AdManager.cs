using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Managers
{
    public class AdManager : BaseManager<Ad>, IAdManager
    {
        private readonly IAdRepository _adRepository;
        private readonly IContentStorage _contentStorage;
        private readonly IContentValidatorProvider _contentValidatorProvider;
        private readonly IContentHelperProvider _contentHelperProvider;
        private readonly ICommitProvider _commitProvider;
            
        public AdManager(
            IAdRepository adRepository,
            IContentStorage contentStorage,
            IContentValidatorProvider contentValidatorProvider,
            IContentHelperProvider contentHelperProvider,
            ICommitProvider commitProvider) 
            : base(adRepository)
        {
            _adRepository = adRepository;
            _contentStorage = contentStorage;
            _contentValidatorProvider = contentValidatorProvider;
            _contentHelperProvider = contentHelperProvider;
            _commitProvider = commitProvider;
        }

        public Task<ValidationResult<ContentError>> ValidateAsync(IFormFile content)
        {
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

        public async Task CreateAdAsync(CreateAdModel createAdModel)
        {
            var extension = Path.GetExtension(createAdModel.Content.FileName);
            var contentStream = createAdModel.Content.OpenReadStream();

            var contentHelper = _contentHelperProvider.CreateContentHelper(extension);

            var thumbnail = contentHelper.GetThumbnail(contentStream, DefaultValues.DefaultThumbnailWidth, DefaultValues.DefaultThumbnailHeight);

            var pathForContent = _contentStorage.GenerateFilePath(extension);
            var pathForThumbnail = _contentStorage.GenerateFilePath(DefaultValues.DefaultThumbnailExtension);

            var saveContentTask = _contentStorage.CreateObjectAsync(contentStream, pathForContent);
            var saveThumbnailTask = _contentStorage.CreateObjectAsync(thumbnail, pathForThumbnail);
            await Task.WhenAll(saveContentTask, saveThumbnailTask);

            var ad = new Ad()
            {
                Title = createAdModel.Title,
                Path = pathForContent,
                PreviewPath = pathForThumbnail,
                AddedDate = DateTime.UtcNow,
                Status = Model.Enum.AdStatus.OnModeration,
                ContentType = ContentTypes[extension]
            };

            Create(ad);
            await _commitProvider.SaveChangesAsync();
        } 
    }
}
