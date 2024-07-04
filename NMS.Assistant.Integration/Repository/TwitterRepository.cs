using System;
using System.Net.Http;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace NMS.Assistant.Integration.Repository
{
    public class TwitterRepository: ITwitterRepository
    {
        private readonly ITwitter _config;
        private readonly HttpClient _httpClient;

        public TwitterRepository(HttpClient httpClient, ITwitter config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<Result> TweetMessage(string msgContent)
        {
            Auth.SetUserCredentials(_config.ConsumerApiKey, _config.ConsumerApiSecretKey, _config.AccessToken, _config.AccessTokenSecret);
            try
            {
                await TweetAsync.PublishTweet(msgContent);
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> TweetMessageWithImageFromUrl(string msgContent, string imageUrl)
        {
            byte[] imageByteArray = await _httpClient.GetByteArrayAsync(new Uri(imageUrl));
            return await TweetMessageWithImageByteArray(msgContent, imageByteArray);
        }

        public async Task<Result> TweetMessageWithImageFromPath(string msgContent, string imagePath)
        {
            if (!System.IO.File.Exists(imagePath)) return new Result(false, "File does not Exist");
            byte[] imageByteArray = await System.IO.File.ReadAllBytesAsync(imagePath);
            return await TweetMessageWithImageByteArray(msgContent, imageByteArray);
        }

        private async Task<Result> TweetMessageWithImageByteArray(string msgContent, byte[] imageByteArray)
        {
            Auth.SetUserCredentials(_config.ConsumerApiKey, _config.ConsumerApiSecretKey, _config.AccessToken, _config.AccessTokenSecret);
            try
            {
                IMedia media = Upload.UploadBinary(new UploadParameters(imageByteArray));
                if (media?.MediaId == null || !media.HasBeenUploaded) return new Result(false, "media was not uploaded");

                await TweetAsync.PublishTweet(msgContent, new PublishTweetOptionalParameters
                {
                    Medias = { media }
                });
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
