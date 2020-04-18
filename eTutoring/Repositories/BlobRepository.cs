using eTutoring.DbContext;
using eTutoring.Models;
using eTutoring.Models.DTO;
using eTutoring.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eTutoring.Repositories
{
    public class BlobRepository : IDisposable
    {
        private readonly AuthContext _context = new AuthContext();

        public Task<int> AddNewFileEntry(string userId, string fileName, string fileId, string comment)
        {
            var model = new FileUploadModel
            {
                FileName = fileName,
                FileId = fileId,
                AuthorId = userId,
                Comment = comment
            };
            _context.FileUploads.Add(model);
            return _context.SaveChangesAsync();
        }

        public IEnumerable<FileUploadResponseModel> AllFilesOfUserId(string userId)
        {
            var files = from fileUpload in _context.FileUploads
                        where fileUpload.AuthorId.Equals(userId)
                        select fileUpload;
            var result = from fileUpload in files.AsEnumerable()
                         select fileUpload.ToFileUploadResponse();
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}