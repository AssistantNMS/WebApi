using NMS.Assistant.Integration.Mapper;

namespace NMS.Assistant.Integration.Contract
{
    public class NmsfmTrackData
    {
        public string Hash { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string RuntimeString { get; set; }

        public int RuntimeInSeconds => RuntimeMapper.ToSeconds(RuntimeString);
    }
}
