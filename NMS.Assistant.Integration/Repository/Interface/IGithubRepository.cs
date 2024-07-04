using System.Threading.Tasks;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface IGithubRepository
    {
        Task<string> GetFileContents(string filename);
    }
}