using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Guide;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class GuideDetailsMapper
    {
        public static GuideDetailViewModel ToViewModel(this GuideDetailWithMeta guideDetail)
        {
            GuideDetailViewModel vm = new GuideDetailViewModel
            {
                Guid = guideDetail.Guid,
                GuideMetaGuid = guideDetail.GuidMetaGuid,
                Language = guideDetail.Language,
                Title = guideDetail.Title,
                ShortTitle = guideDetail.ShortTitle,
                Author = guideDetail.Author,
                Minutes = guideDetail.Minutes,
                DateCreated = DateHelper.GetFrontendSafeDateTime(guideDetail.DateCreated),
                Tags = guideDetail.Tags.Split(',').ToList()
            };

            return vm;
        }
        public static List<GuideDetailViewModel> ToViewModel(this List<GuideDetailWithMeta> orig) => orig.Select(o => o.ToViewModel()).ToList();

        //public static GuideDetail ToDatabase(this AddGuideDetailViewModel vm)
        //{
        //    GuideDetail database = new GuideDetail
        //    {
        //        Guid = Guid.NewGuid(),
        //        Title = vm.Title,
        //        ShortTitle = vm.ShortTitle,
        //        Author = vm.Author,
        //        Minutes = vm.Minutes,
        //        DateCreated = vm.DateCreated,
        //        Tags = string.Join(',', vm.Tags)
        //    };

        //    return database;
        //}

        public static GuideDetail ToDatabase(this GuideDetailViewModel vm)
        {
            GuideDetail database = new GuideDetail
            {
                Guid = Guid.NewGuid(),
                Title = vm.Title,
                ShortTitle = vm.ShortTitle,
                Author = vm.Author,
                Minutes = vm.Minutes,
                DateCreated = vm.DateCreated,
                Tags = string.Join(',', vm.Tags)
            };

            return database;
        }
    }
}
