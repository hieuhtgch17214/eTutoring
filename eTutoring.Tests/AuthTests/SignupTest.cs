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
            var postBodyObject = new
            {
                username = "abcdefgh",
                fullname = "John John",
                email = "abcd@def.com",
                password = "abcdefghi",
                confirmpassword = "abcdefghi"
            };
            var postBodyJson = JsonConvert.SerializeObject(postBodyObject);
            var postBodyStringContent = new StringContent(postBodyJson, Encoding.UTF8, "application/json");

            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.PostAsync("api/auth/register", postBodyStringContent);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                var entity = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                Assert.IsTrue(entity.ContainsKey("message"));
            }
        }
    }
}
