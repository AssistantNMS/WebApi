using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Integration.Repository
{
    public class DownloadFileRepository: IDownloadFileRepository
    {
        private readonly HttpClient _httpClient;

        public DownloadFileRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result> DownloadFile(string path, string url, Action<HttpRequestHeaders> manipulateHeaders = null, bool overwrite = true)
        {
            if (File.Exists(path))
            {
                if (overwrite == false)
                {
                    return new Result(false, "file already exists");
                } 
                File.Delete(path);
            }

            try
            {
                manipulateHeaders?.Invoke(_httpClient.DefaultRequestHeaders);
                HttpResponseMessage response = await _httpClient.GetAsync(new Uri(url));

                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;
                    Stream contentStream = await content.ReadAsStreamAsync(); // get the actual content stream
                    Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                    await contentStream.CopyToAsync(stream);
                    await contentStream.DisposeAsync();
                    await stream.DisposeAsync();
                    return new Result(true, string.Empty);
                }
                return new Result(false, "Status code was not 200");
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
