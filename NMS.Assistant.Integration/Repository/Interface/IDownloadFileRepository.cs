using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface IDownloadFileRepository
    {
        Task<Result> DownloadFile(string path, string url, Action<HttpRequestHeaders> manipulateHeaders = null, bool overwrite = true);
    }
}
