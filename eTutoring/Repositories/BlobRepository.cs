using eTutoring.DbContext;
using eTutoring.Models;
using eTutoring.Models.DTO.FormModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eTutoring.Repositories
{
    public class BlobRepository : IDisposable
    {
        private readonly AuthContext _context = new AuthContext();

        public Task<int> AddNewFileEntry(string userId, string fileName, string fileId)
        {
            var model = new FileUploadModel
            {
                FileName = fileName,
                FileId = fileId,
                AuthorId = userId
            };
            _context.FileUploads.Add(model);
            return _context.SaveChangesAsync();
        }

        public IQueryable<FileUploadModel> AllFilesOfUserId(string userId)
        {
            var files = from fileUpload in _context.FileUploads
                        where fileUpload.AuthorId.Equals(userId)
                        select fileUpload;
            return files.Include("FileComments.Author");
        }

        public Task<int> AddComment(string userId, FileCommentFormModel model)
        {
            var commentDbModel = new FileCommentModel
            {
                FileUploadId = model.FileId,
                AuthorId = userId,
                Comment = model.Comment
            };
            _context.FileComments.Add(commentDbModel);
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}