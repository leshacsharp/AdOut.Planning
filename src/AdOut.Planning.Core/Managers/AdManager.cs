using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Managers
{
    public class AdManager : BaseManager<Ad>, IAdManager
    {
        private readonly IAdRepository _adRepository;
        private readonly IContentStorage _contentStorage;
        private readonly IContentComponentsProvider _contentComponentsProvider;
        private readonly ICommitProvider _commitProvider;
            
        public AdManager(
            IAdRepository adRepository,
            IContentStorage contentStorage,
            IContentComponentsProvider contentComponentsProvider,
            ICommitProvider commitProvider) 
            : base(adRepository)
        {
            _adRepository = adRepository;
            _contentStorage = contentStorage;
            _contentComponentsProvider = contentComponentsProvider;
            _commitProvider = commitProvider;
        }

        public async Task CreateAdAsync(CreateAdModel createAdModel)
        {
            var content = createAdModel.Content.OpenReadStream();
            var extension = Path.GetExtension(createAdModel.Content.FileName);
            var isAllowedExtension = AllowedExtensions.Contains(extension);

            if(!isAllowedExtension)
            {
                throw new BadRequestException($"{extension} extension is not allowed");
            }

            var contentComponentsFactory = _contentComponentsProvider.CreateContentFactory(extension);
            var contentValidator = contentComponentsFactory.CreateContentValidator();
            var contentHelper = contentComponentsFactory.CreateContentHelper();

            var validationResult = await contentValidator.ValidAsync(content);
            if(!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(e => e.Description);
                var validationMessage = string.Join("\n", validationErrors);
                throw new BadRequestException(validationMessage);
            }

            var thumbnail = contentHelper.GetThumbnail(content, DefaultValues.DefaultThumbnailWidth, DefaultValues.DefaultThumbnailHeight);

            var pathForContent = _contentStorage.GenerateFilePath(extension);
            var pathForThumbnail = _contentStorage.GenerateFilePath(DefaultValues.DefaultThumbnailExtension);

            var saveContentTask = _contentStorage.CreateObjectAsync(content, pathForContent);
            var saveThumbnailTask = _contentStorage.CreateObjectAsync(thumbnail, pathForThumbnail);
            await Task.WhenAll(saveContentTask, saveThumbnailTask);

            var ad = new Ad()
            {
                Title = createAdModel.Title,
                Path = pathForContent,
                PreviewPath = pathForThumbnail,
                AddedDate = DateTime.UtcNow,
                Status = Model.Enum.AdStatus.OnModeration,
                Type = ContentTypes[extension]
            };

            Create(ad);
            await _commitProvider.SaveChangesAsync();
        }
    }
}
