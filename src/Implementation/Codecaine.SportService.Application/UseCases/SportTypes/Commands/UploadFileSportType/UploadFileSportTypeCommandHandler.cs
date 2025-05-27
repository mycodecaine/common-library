using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Primitives.Result;
using Codecaine.Common.Storage;
using Codecaine.Common.Storage.Providers.Minio;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.UploadFileSportType
{
    internal sealed class UploadFileSportTypeCommandHandler : CommandHandler<UploadFileSportTypeCommand, Result<UploadFileSportTypeCommandResponse>>
    {
        private readonly ILogger<UploadFileSportTypeCommandHandler> _logger;
        private readonly IStorageProvider _storageProvider;
        public UploadFileSportTypeCommandHandler(IStorageProvider storageProvider,  ILogger<UploadFileSportTypeCommandHandler> logger) : base(logger)
        {
            _storageProvider = storageProvider ;
            _logger = logger;
        }

        public override async Task<Result<UploadFileSportTypeCommandResponse>> Handle(UploadFileSportTypeCommand request, CancellationToken cancellationToken)
            => await HandleSafelyAsync(async () =>
        {

            Dictionary<string, string> metadata = new()
            {
                { "author", "test-user" },
                { "content-type", request.ContentType }
            };

           

            var result = await _storageProvider.Upload(request.FileName, request.FileStream, metadata);

            if (string.IsNullOrEmpty(result))
            {
                _logger.LogError("Failed to upload file: {FileName}", request.FileName);
                return Result.Failure<UploadFileSportTypeCommandResponse>(new Common.Primitives.Errors.Error("FailedToUpload", "Failed to upload file"));
            }
            var url =   _storageProvider.GenerateSecureUrlAsync(request.FileName, 1);

            return Result.Success(new UploadFileSportTypeCommandResponse(
                request.FileName,
                url
            ));
        });
    }
}
