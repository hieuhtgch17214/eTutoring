using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Collections.Specialized;

namespace eTutoring.Providers
{
    public class AzureStorageMultipartFormProvider : MultipartFormDataStreamProvider
    {
        private readonly CloudBlobContainer _container;
        private readonly string _userId;

        public string FileName { get; set; }

        public AzureStorageMultipartFormProvider(CloudBlobContainer container, string userId) : base("azure")
        {
            _container = container;
            _userId = userId + "/";
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            var fileHeader = _userId + Guid.NewGuid().ToString();
            var blobBlock = _container.GetBlockBlobReference(fileHeader);

            if (headers.ContentType != null)
            {
                // Set appropriate content type for your uploaded file
                blobBlock.Properties.ContentType = headers.ContentType.MediaType;
            }

            var currentFileData = new MultipartFileData(headers, blobBlock.Name);

            FileData.Add(currentFileData);

            FormData.Set("blobUri", blobBlock.Uri.ToString());

            return blobBlock.OpenWrite();
        }
    }
}