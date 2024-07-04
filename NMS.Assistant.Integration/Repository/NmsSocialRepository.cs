using AngleSharp.Dom;
using Mastonet;
using Mastonet.Entities;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Visibility = Mastonet.Visibility;

namespace NMS.Assistant.Integration.Repository
{
    public class NmsSocialRepository
    {
        private static string _instance = "nomanssky.social";
        private readonly MastodonClient _client;
        private readonly HttpClient _httpClient;

        public NmsSocialRepository(HttpClient httpClient, IApiConfiguration config)
        {
            _client = new MastodonClient(_instance, config.Authentication.MastodonAuth.AccessToken);
            _httpClient = httpClient;
        }

        public async Task<Result> TootMessage(string msgContent)
        {
            try
            {
                await _client.PublishStatus(status: msgContent, visibility: Visibility.Public);
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> TootMessageWithImageFromPath(string msgContent, string imagePath)
        {
            try
            {
                FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                Attachment attachment = await _client.UploadMedia(stream);
                await _client.PublishStatus(status: msgContent, visibility: Visibility.Public, mediaIds: new string[] { attachment.Id });
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> TootMessageWithImageFromUrl(string msgContent, string imageUrl)
        {
            try
            {
                byte[] imageData = await _httpClient.GetByteArrayAsync(imageUrl);
                MemoryStream stream = new MemoryStream(imageData);
                Attachment attachment = await _client.UploadMedia(stream);
                await _client.PublishStatus(status: msgContent, visibility: Visibility.Public, mediaIds: new string[] { attachment.Id });
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
