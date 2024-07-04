using System;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Contract
{
    public class GuideDetailWithMeta
    {
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Author { get; set; }
        public int Minutes { get; set; }
        public DateTime DateCreated { get; set; }
        public string Tags { get; set; }

        public LanguageType Language { get; set; }


        public Guid GuidMetaGuid { get; set; }
        public int Likes { get; set; }
        public int Views { get; set; }
        //public string FileRelativePath { get; set; }

        public static GuideDetailWithMeta Combine(GuideMetaGuideDetail all) 
        {
            GuideDetailWithMeta detailWithMeta = new GuideDetailWithMeta
            {
                Guid = all.GuideDetailGuid,
                Title = all.GuideDetail.Title,
                ShortTitle = all.GuideDetail.ShortTitle,
                Author = all.GuideDetail.Author,
                Minutes = all.GuideDetail.Minutes,
                DateCreated = DateHelper.GetFrontendSafeDateTime(all.GuideDetail.DateCreated),
                Tags = all.GuideDetail.Tags,
                Language = all.LanguageType,
                GuidMetaGuid = all.GuideMetaGuid,
                Likes = all.GuideMeta.Likes,
                Views = all.GuideMeta.Views,
            };
            return detailWithMeta;
        }
    }
}
