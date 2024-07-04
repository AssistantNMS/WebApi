using System;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using static FirebaseAdmin.FirebaseApp;

namespace NMS.Assistant.Integration.Repository
{
    public class FirebaseRepository : IFirebaseRepository
    {
        //private readonly IFirebase _config;
        private readonly FirebaseApp _fireApp;

        public FirebaseRepository(IFirebase config)
        {
            //_config = config;
            GoogleCredential googleCredential = GoogleCredential.FromFile(config.PathRoRefreshToken);
            _fireApp = DefaultInstance ?? Create(new AppOptions()
            {
                ProjectId = config.ProjectId,
                Credential = googleCredential,
            });
        }


        public async Task<ResultWithValue<string>> SendMessage(string topic, string msgTitle, string msgBody)
        {
            try
            {
                FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(_fireApp);
                string result = await messaging.SendAsync(new Message
                {
                    Topic = topic,
                    Notification = new Notification
                    {
                        Title = msgTitle,
                        Body = msgBody
                    }
                });
                return new ResultWithValue<string>(true, result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<string>(false, string.Empty, $"FireBase exception: {ex.Message}");
            }

        }
    }
}
