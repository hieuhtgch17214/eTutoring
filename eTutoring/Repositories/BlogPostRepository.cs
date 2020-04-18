using eTutoring.DbContext;
using eTutoring.Models;
using eTutoring.Models.DTO;
using eTutoring.Models.DTO.FormModels;
using eTutoring.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eTutoring.Repositories
{
    public class BlogPostRepository : IDisposable
    {
        private readonly AuthContext _authContext = new AuthContext();

        public Task<int> AddBlogPost(string userId, BlogPostFormModel model)
        {
            var blogPostDBModel = new BlogPostModel
            {
                Title = model.Title,
                Content = model.Content,
                AuthorId = userId
            };
            _authContext.BlogPosts.Add(blogPostDBModel);
            return _authContext.SaveChangesAsync();
        }

        public IEnumerable<BlogPostResponseModel> GetAllPostsOfUserId(string userId)
        {
            var posts = from blogPost in _authContext.BlogPosts
                        where blogPost.AuthorId.Equals(userId)
                        select blogPost;
            var result = from blogPost in posts.AsEnumerable()
                         select blogPost.ToBlogPostResponse();
            return result;
        }

        public void Dispose()
        {
            _authContext.Dispose();
        }
    }
}