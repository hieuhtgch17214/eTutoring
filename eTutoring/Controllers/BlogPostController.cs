using eTutoring.Models.DTO.FormModels;
using eTutoring.Repositories;
using eTutoring.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace eTutoring.Controllers
{
    [Authorize]
    [RoutePrefix("api/blog")]
    public class BlogPostController : ApiController
    {
        private readonly BlogPostRepository _blogRepo = new BlogPostRepository();
        private readonly AuthRepository _authRepo = new AuthRepository();

        [HttpGet]
        [Route("all-posts")]
        public async Task<IHttpActionResult> GetAllBlogPosts(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is needed");
            }
            var userTask = _authRepo.FindUserById(userId);
            var blogPosts = _blogRepo.GetAllPostsOfUserId(userId);
            var user = await userTask;
            if (user == null)
            {
                var message = new
                {
                    message = "User does not exist"
                };
                var response = Request.CreateResponse(System.Net.HttpStatusCode.NotFound, message);
                return ResponseMessage(response);
            }
            var result = new
            {
                Author = user.ToUserResponseModel(),
                Posts = blogPosts
            };
            return Ok(result);
        }

        [HttpPost]
        [Route("new-post")]
        public async Task<IHttpActionResult> AddNewPost(BlogPostFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = User.Identity.GetUserId();
            try
            {
                await _blogRepo.AddBlogPost(id, model);
                var message = Request.CreateResponse(System.Net.HttpStatusCode.Created, new
                {
                    message = "Blog post created"
                });
                return ResponseMessage(message);
            } 
            catch (Exception e)
            {
                var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, e);
                return ResponseMessage(response);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _blogRepo.Dispose();
                _authRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
