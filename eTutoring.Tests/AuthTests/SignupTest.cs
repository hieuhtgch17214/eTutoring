using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using System.Net.Http;
using Newtonsoft.Json;

namespace eTutoring.Tests.AuthTests
{
    /// <summary>
    /// Summary description for SignupTest
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
                role = "student"
            };
            
            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.PostAsJsonAsync("api/auth/register", postBody);
                var responseContent = await response.Content.ReadAsStringAsync();
                var entity = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.IsTrue(entity.ContainsKey("message"));
                Assert.AreEqual(entity["message"], "Registration Completed");
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
