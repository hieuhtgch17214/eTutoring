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
                username = "abcdef",
                fullname = "John John",
                email = "abc@def.com",
                password = "abcdefgh",
                confirmpassword = "abcdefgh"
            };
            var postBodyStringContent = new StringContent(postBody.ToString(), Encoding.UTF8, "application/json");

            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.PostAsync("api/auth/register", postBodyStringContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var entity = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.IsTrue(entity.ContainsKey("message"));
            }
        }
    }
}
