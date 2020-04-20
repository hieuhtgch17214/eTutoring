using eTutoring.DbContext;
using eTutoring.Models;
using eTutoring.Models.DTO;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eTutoring.Utils
{
    public static class Utils
    {
        public static ApplicationUser ToApplicationUser(this UserFormModel userModel) => new ApplicationUser
        {
            UserName = userModel.UserName,
            Email = userModel.Email,
            FullName = userModel.FullName,
            Gender = userModel.Gender,
            Birthday = userModel.Birthday
        };

        public static UserResponseModelDto ToUserResponseModel(this ApplicationUser user) => new UserResponseModelDto
        {
            ID = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            Gender = user.Gender,
            Birthday = user.Birthday.ToString("dd/MM/yyyy")
        };

        public static UserResponseModelDto ToUserResponseModel(this ApplicationUser user, string role) => new UserResponseModelDto
        {
            ID = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            Gender = user.Gender,
            Birthday = user.Birthday.ToString("dd/MM/yyyy"),
            Role = role
        };

        public static StudentResponseModel ToStudentResponseModel(this ApplicationUser user) => new StudentResponseModel
        {
            ID = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            Gender = user.Gender,
            Birthday = user.Birthday.ToString("dd/MM/yyyy")
        };

        public static BlogPostResponseModel ToBlogPostResponse(this BlogPostModel model) => new BlogPostResponseModel
        {
            ID = model.ID,
            Title = model.Title,
            Content = model.Content
        };

        public static FileUploadResponseModel ToFileUploadResponse(this FileUploadModel model, CloudBlobContainer container, SharedAccessBlobPolicy key)
        {
            var blobBlock = container.GetBlockBlobReference(model.FileId);
            var blobToken = blobBlock.GetSharedAccessSignature(key);
            var comments = model.FileComments.Select(comment => comment.ToFileCommentResponse());
            return new FileUploadResponseModel
            {
                ID = model.ID,
                FileUri = blobBlock.Uri + blobToken,
                FileName = model.FileName,
                Comments = comments
            };
        }

        public static FileCommentResponseModel ToFileCommentResponse(this FileCommentModel model) => new FileCommentResponseModel
        {
            ID = model.ID,
            Author = model.Author.ToUserResponseModel(),
            Comment = model.Comment
        };
    }
}