using eTutoring.Models.DTO.FormModels;
using eTutoring.Providers;
using eTutoring.Repositories;
using eTutoring.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using MultipartDataMediaFormatter.Infrastructure;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace eTutoring.Controllers
{
    [Authorize]
    [RoutePrefix("api/files")]
    public class FileUploadController : ApiController
    {
        private readonly BlobRepository _blobRepo = new BlobRepository();
        private readonly AuthRepository _authRepo = new AuthRepository();

        [HttpGet]
        [Route("all-files")]
        public async Task<IHttpActionResult> GetFileHistories(string userId)
        {
            var container = await CreateBlobContainer();
            var key = GetNewAccessKey();

            var userTask = _authRepo.FindUserById(userId);
            var fileHistoryQuery = _blobRepo.AllFilesOfUserId(userId);
            var fileHistoryResponse = from history in fileHistoryQuery.AsEnumerable()
                                      select history.ToFileUploadResponse(container, key);

            var response = new
            {
                Author = (await userTask).ToUserResponseModel(),
                Files = fileHistoryResponse
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("add-file")]
        public async Task<IHttpActionResult> UploadFile(FileUploadFormModel model)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.GetUserId();
            var filePath = await UploadFileToStorage(userId, model.File);

            await _blobRepo.AddNewFileEntry(userId, model.File.FileName, filePath);

            var message = new
            {
                message = "File uploaded"
            };
            var response = Request.CreateResponse(HttpStatusCode.Created, message);

            return ResponseMessage(response);
        }

        [HttpPost]
        [Route("add-comment")]
        public async Task<IHttpActionResult> AddCommentToFile(FileCommentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.GetUserId();
            await _blobRepo.AddComment(userId, model);

            var message = new
            {
                message = "Comment added to file"
            };
            return Ok(message);
        }

        private async Task<string> UploadFileToStorage(string userId, HttpFile file)
        {
            var container = await CreateBlobContainer();
            var filePath = $"{userId}/{Guid.NewGuid()}/{file.FileName}";
            var blobBlock = container.GetBlockBlobReference(filePath);
            var stream = new MemoryStream(file.Buffer);

            await blobBlock.UploadFromStreamAsync(stream);

            return filePath;
        }

        private async Task<CloudBlobContainer> CreateBlobContainer()
        {
            var accountName = ConfigurationManager.AppSettings["storage:account:name"];
            var accountKey = ConfigurationManager.AppSettings["storage:account:key"];
            CloudStorageAccount storageAccount;
            if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(accountKey))
            {
                storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            } else
            {
                var credentials = new StorageCredentials(accountName, accountKey);
                storageAccount = new CloudStorageAccount(credentials, true);
            }
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("files");
            await container.CreateIfNotExistsAsync();
            return container;
        }

        private SharedAccessBlobPolicy GetNewAccessKey()
        {
            var key = new SharedAccessBlobPolicy();
            key.Permissions = SharedAccessBlobPermissions.Read;
            key.SharedAccessStartTime = DateTime.UtcNow.AddSeconds(-5);
            key.SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1);
            return key;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _blobRepo.Dispose();
                _authRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
