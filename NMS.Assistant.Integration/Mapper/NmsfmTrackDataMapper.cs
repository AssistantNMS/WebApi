using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Integration.Contract;

namespace NMS.Assistant.Integration.Mapper
{
    public static class NmsfmTrackDataMapper
    {
        public static List<NmsfmTrackDataViewModel> ToViewModel(this List<NmsfmSheet> sheets)
        {
            //Dictionary<string, string> lookupData = new Dictionary<string, string>();
            Dictionary<string, NmsfmTrackDataViewModel> uniqueTracks = new Dictionary<string, NmsfmTrackDataViewModel>();

            List<NmsfmSheet> mainSheets = new List<NmsfmSheet>();
            List<NmsfmSheet> notMainSheets = new List<NmsfmSheet>();

            foreach (NmsfmSheet nmsfmSheet in sheets)
            {
                if (nmsfmSheet.Name.Equals("Station Schedule", StringComparison.InvariantCultureIgnoreCase)) mainSheets.Add(nmsfmSheet);
                else notMainSheets.Add(nmsfmSheet);
            }

            foreach (NmsfmSheet notMainSheet in notMainSheets)
            {
                foreach (NmsfmTrackData track in notMainSheet.Tracks)
                {
                    //if (lookupData.ContainsKey(track.Hash)) continue;
                    //lookupData.Add(track.Hash, notMainSheet.Name);

                    if (uniqueTracks.ContainsKey(track.Hash)) continue;

                    NmsfmTrackType type = GetTypeFromName(notMainSheet.Name);
                    uniqueTracks.Add(track.Hash, new NmsfmTrackDataViewModel
                    {
                        Type = type,
                        Title = track.Title,
                        Artist = track.Artist,
                        RuntimeInSeconds = track.RuntimeInSeconds
                    });
                }
            }

            //List<NmsfmTrackDataViewModel> liveTracks = new List<NmsfmTrackDataViewModel>();
            //foreach (NmsfmSheet notMainSheet in mainSheets)
            //{
            //    foreach (NmsfmTrackData track in notMainSheet.Tracks)
            //    {
            //        if (!lookupData.ContainsKey(track.Hash)) continue;
            //        string existingTrackSheet = lookupData[track.Hash];

            //        NmsfmTrackType type = GetTypeFromName(existingTrackSheet);

            //        liveTracks.Add(new NmsfmTrackDataViewModel
            //        {
            //            Type = type,
            //            Title = track.Title,
            //            Artist = track.Artist,
            //            RuntimeInSeconds = track.RuntimeInSeconds
            //        });
            //    }
            //    break;
            //}

            //return liveTracks;
            return uniqueTracks.Values.OrderBy(tr => tr.Title).ToList();
        }

        private static NmsfmTrackType GetTypeFromName(string existingTrackSheet)
        {

            NmsfmTrackType type = NmsfmTrackType.Unknown;
            if (existingTrackSheet.Equals("jingles", StringComparison.CurrentCultureIgnoreCase))
                type = NmsfmTrackType.Jingle;
            if (existingTrackSheet.Equals("jinlges", StringComparison.CurrentCultureIgnoreCase))
                type = NmsfmTrackType.Jingle;
            if (existingTrackSheet.Equals("Adverts", StringComparison.CurrentCultureIgnoreCase))
                type = NmsfmTrackType.Advert;
            if (existingTrackSheet.Equals("Radio Show Segments", StringComparison.CurrentCultureIgnoreCase))
                type = NmsfmTrackType.RadioShow;
            if (existingTrackSheet.Equals("Track List", StringComparison.CurrentCultureIgnoreCase))
                type = NmsfmTrackType.Track;

            return type;
        }
    }
}
