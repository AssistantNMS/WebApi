using NMS.Assistant.Domain.Configuration;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Integration.Repository;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace NMS.Assistant.Test
{
    public class IntegrationTests
    {
        HttpClient _client;
        IAuthentication _auth;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
            _auth = new Authentication { 
                NoMansSkySocialAuth = new NoMansSkySocialAuth
                {
                    ApiKey = "tester"
                }
            };
        }

        [TestCase]
        public async Task TestQsRequestAsync()
        {
            NmsSocialAdminRepository adminRepo = new NmsSocialAdminRepository(_client, _auth, "http://localhost:55555");
            var result = await adminRepo.TriggerQuicksilverMerchantWithManualData(new CommunityMission { 
                CurrentTier = 1,
                MissionId = 72,
                Percentage = 100,
                TotalTiers = 3
            });

            Assert.IsTrue(result.IsSuccess);
        }
    }
}

