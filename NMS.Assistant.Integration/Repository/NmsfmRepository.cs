using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using ExcelDataReader;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Contract;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Integration.Repository
{
    public class NmsfmRepository: INmsfmRepository
    {
        private readonly IDirectory _directoryConfig;

        private readonly IDownloadFileRepository _downloadRepo;

        private const string BoxDownloadFileUrl = "https://api.box.com/2.0/files/{0}/content/";
        private const string DocumentId = "780006060721";
        private const string AuthHeader = "Bearer Nyh2uiybN0afCx6Dd6xrVYLzgIAXJBkQ";

        public NmsfmRepository(IDirectory directoryConfig, IDownloadFileRepository downloadRepo)
        {
            _directoryConfig = directoryConfig;
            _downloadRepo = downloadRepo;
        }

        public async Task<ResultWithValue<List<NmsfmSheet>>> ReadTrackDataFromExcel()
        {
            string tempFilePath = Path.Combine(_directoryConfig.TempDownload.DiskPath, "nmsfmDoc.xls");
            string url = string.Format(BoxDownloadFileUrl, DocumentId);
            Result docDownloadResult = await _downloadRepo.DownloadFile(tempFilePath, url, headers =>
            {
                headers.Add("Authorization", AuthHeader);
            });

            if (docDownloadResult.HasFailed) return new ResultWithValue<List<NmsfmSheet>>(false, null, docDownloadResult.ExceptionMessage);

            List<NmsfmSheet> sheets = new List<NmsfmSheet>();
            await using (FileStream stream = File.Open(tempFilePath, FileMode.Open, FileAccess.Read))
            {
                using IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                DataSet result = reader.AsDataSet();
                foreach (DataTable table in result.Tables)
                {
                    bool hasSkippedFirstRow = false;
                    List<NmsfmTrackData> tracks = new List<NmsfmTrackData>();
                    foreach (DataRow tableRow in table.Rows)
                    {
                        if (!hasSkippedFirstRow)
                        {
                            hasSkippedFirstRow = true;
                            continue;
                        }

                        string title = tableRow[0].ToString();
                        string artist = tableRow[1].ToString();
                        string runtimeString = tableRow[2].ToString();
                        string hash = HashSaltHelper.GetHashString(title + artist, runtimeString);
                        tracks.Add(new NmsfmTrackData
                        {
                            Hash = hash,
                            Title = title,
                            Artist = artist,
                            RuntimeString = runtimeString,
                        });
                    }

                    sheets.Add(new NmsfmSheet
                    {
                        Name = table.TableName,
                        Tracks = tracks,
                    });
                }
            }

            return new ResultWithValue<List<NmsfmSheet>>(true, sheets, string.Empty);
        }
    }
}
