using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Guide;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class GuideMapper
    {
        public static GuideMetaViewModel ToViewModel(this GuideMeta guideMeta)
        {
            GuideMetaViewModel vm = new GuideMetaViewModel
            {
                Guid = guideMeta.Guid,
                Name = guideMeta.Name,
                Likes = guideMeta.Likes,
                Views = guideMeta.Views,
                FileRelativePath = guideMeta.FileRelativePath,
            };

            return vm;
        }
        public static List<GuideMetaViewModel> ToViewModel(this List<GuideMeta> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static GuideMeta ToDatabaseModel(this GuideMetaViewModel vm)
        {
            GuideMeta databaseModel = new GuideMeta
            {
                Guid = vm.Guid,
                Name = vm.Name,
                Likes = vm.Likes,
                Views = vm.Views,
                FileRelativePath = vm.FileRelativePath,
            };

            return databaseModel;
        }

        public static PendingGuide ToDatabaseModel(this PendingGuideViewModel vm)
        {
            PendingGuide databaseModel = new PendingGuide
            {
                Guid = Guid.NewGuid(),
                GuideMetaGuid = vm.GuideMetaGuid,
                UserContactDetails = vm.UserContactDetails,
                DateSubmitted = DateTime.Now,
            };

            return databaseModel;
        }

        public static AdminPendingGuideViewModel ToViewModel(this PendingGuide db)
        {
            AdminPendingGuideViewModel vm = new AdminPendingGuideViewModel
            {
                Guid = db.Guid,
                GuideMetaGuid = db.GuideMetaGuid,
                UserContactDetails = db.UserContactDetails,
                DateSubmitted = db.DateSubmitted,
            };

            return vm;
        }
        public static List<AdminPendingGuideViewModel> ToViewModel(this List<PendingGuide> orig) => orig.Select(o => o.ToViewModel()).ToList();
    }
}
