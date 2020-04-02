using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace eTutoring.Tests.AuthTests
{
    /// <summary>
    /// Signup integration test
    /// </summary>
    [TestClass]
    public class SignupTest
    {
        [TestMethod]
        public async Task TestSuccessfulSignup()
        {
            var postBody = new
            {
                username = "abcdefghijk",
                fullname = "John John",
                email = "abcdjek@def.com",
                password = "abcdefghijk",
                confirmpassword = "abcdefghijk",
                role = "student",
                gender = "male",
                birthday = "12/11/2001"
            };
            
            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.PostAsJsonAsync("api/auth/register", postBody);
                var responseContent = await response.Content.ReadAsStringAsync();
                var entity = JObject.Parse(responseContent);

                Assert.AreEqual(entity["message"], "Registration Completed");
                response.EnsureSuccessStatusCode();
            }
        }

        [TestMethod]
        public async Task TestMissingFieldSignUp()
        {
            var postBody = new
            {
                username = "abcdefghijk",
                fullname = "John John",
                password = "abcdefghijk",
                confirmpassword = "abcdefghijk",
                role = "student",
                gender = "male",
                birthday = "12/11/2001"
            };

            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.PostAsJsonAsync("api/auth/register", postBody);
                var responseContent = await response.Content.ReadAsStringAsync();
                var entity = JObject.Parse(responseContent);

                Assert.AreEqual(entity["message"], "The request is invalid.");
                var firstErrorMessage = entity["modelState"]["userModel.Email"][0];
                Assert.AreEqual(firstErrorMessage, "The Email field is required.");
            }
        }
    }
}
