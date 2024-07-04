using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Community;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class CommunityLinkMapper
    {
        public static CommunityLinkViewModel ToViewModel(this CommunityLink communityLink)
        {
            CommunityLinkViewModel vm = new CommunityLinkViewModel
            {
                Name = communityLink.Name,
                Subtitle = communityLink.Subtitle,
                ExternalUrl = communityLink.ExternalUrl,
                IconUrl = communityLink.IconUrl,
            };

            return vm;
        }

        public static List<CommunityLinkViewModel> ToViewModel(this List<CommunityLink> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static AdminCommunityLinkViewModel ToAdminViewModel(this CommunityLink communityLink)
        {
            AdminCommunityLinkViewModel vm = new AdminCommunityLinkViewModel
            {
                Guid = communityLink.Guid,
                Name = communityLink.Name,
                Subtitle = communityLink.Subtitle,
                ExternalUrl = communityLink.ExternalUrl,
                IconUrl = communityLink.IconUrl,
                SortRank = communityLink.SortRank,
            };

            return vm;
        }
        public static List<AdminCommunityLinkViewModel> ToAdminViewModel(this List<CommunityLink> orig) => orig.Select(o => o.ToAdminViewModel()).ToList();

        public static CommunityLink ToDatabaseModel(this AdminCommunityLinkViewModel vm)
        {
            CommunityLink persistence = new CommunityLink
            {
                Guid = vm.Guid,
                Name = vm.Name,
                Subtitle = vm.Subtitle,
                ExternalUrl = vm.ExternalUrl,
                IconUrl = vm.IconUrl,
                SortRank = vm.SortRank
            };

            return persistence;
        }

        public static CommunityLink ToDatabaseModel(this AddCommunityLinkViewModel vm) => vm.ToDatabaseModel(Guid.NewGuid());

        public static CommunityLink ToDatabaseModel(this AddCommunityLinkViewModel vm, Guid guid)
        {
            CommunityLink persistence = new CommunityLink
            {
                Guid = guid,
                Name = vm.Name,
                Subtitle = vm.Subtitle,
                ExternalUrl = vm.ExternalUrl,
                IconUrl = vm.IconUrl,
                SortRank = vm.SortRank
            };

            return persistence;
        }
    }
}
