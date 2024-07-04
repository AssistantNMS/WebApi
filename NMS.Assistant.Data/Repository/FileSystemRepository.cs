using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Data.Repository
{
    public class FileSystemRepository
    {
        private readonly string _jsonDirectory;
        private readonly JsonSerializerSettings _jsonSettings;

        public FileSystemRepository(string jsonDirectory)
        {
            _jsonDirectory = jsonDirectory;
            _jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public ResultWithValue<Dictionary<string, string>> LoadJsonDict(string fileName)
        {
            string jsonFilePath = Path.Combine(_jsonDirectory, fileName);
            if (!File.Exists(jsonFilePath)) return new ResultWithValue<Dictionary<string, string>>(false, new Dictionary<string, string>(), $"File does not exist: {jsonFilePath}");
            try
            {
                string json = File.ReadAllText(jsonFilePath);
                Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return new ResultWithValue<Dictionary<string, string>>(true, result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<Dictionary<string, string>>(false, new Dictionary<string, string>(), $"LoadJsonDict: {ex.Message}");
            }
        }

        public ResultWithValue<List<string>> GetListOfGuideFiles(string folder)
        {
            string folderPath = Path.Combine(_jsonDirectory, folder);
            if (!Directory.Exists(folderPath)) return new ResultWithValue<List<string>>(false, new List<string>(), $"File does not exist: {folderPath}");
            try
            {
                List<string> files = Directory.GetFiles(folderPath)
                    .Where(f => f.Contains(".json", StringComparison.InvariantCultureIgnoreCase))
                    .Select(f => f.Replace(folderPath, string.Empty)
                        .Replace(Path.DirectorySeparatorChar.ToString(), string.Empty)
                        .Replace(".json", string.Empty)
                    ).ToList();
                return new ResultWithValue<List<string>>(true, files, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<List<string>>(false, new List<string>(), $"GetListOfGuideFiles: {ex.Message}");
            }
        }

        public ResultWithValue<string> LoadJsonContent(string fileName)
        {
            string jsonFilePath = Path.Combine(_jsonDirectory, fileName);
            if (!File.Exists(jsonFilePath)) return new ResultWithValue<string>(false, string.Empty, $"File does not exist: {jsonFilePath}");
            try
            {
                string json = File.ReadAllText(jsonFilePath);
                return new ResultWithValue<string>(true, json, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<string>(false, string.Empty, $"LoadJsonContent: {ex.Message}");
            }
        }

        public void WriteJsonFile(object jsonObj, string fileName)
        {
            string jsonFilePath = Path.Combine(_jsonDirectory, fileName);
            if (File.Exists(jsonFilePath))
            {
                File.Delete(jsonFilePath);
            }

            int directorySeparatorIndex = jsonFilePath.LastIndexOf(Path.DirectorySeparatorChar);
            if (directorySeparatorIndex > 0)
            {
                string dir = jsonFilePath.Remove(directorySeparatorIndex, jsonFilePath.Length - directorySeparatorIndex);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            }

            string json = JsonConvert.SerializeObject(jsonObj, Formatting.Indented, _jsonSettings);
            File.WriteAllText(jsonFilePath, json);
        }

        public void DeleteJsonFile(string fileName)
        {
            string jsonFilePath = Path.Combine(_jsonDirectory, fileName);
            if (File.Exists(jsonFilePath))
            {
                File.Delete(jsonFilePath);
            }
        }
    }
}
