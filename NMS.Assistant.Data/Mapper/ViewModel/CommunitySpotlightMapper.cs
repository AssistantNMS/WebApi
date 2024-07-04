using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Community;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class CommunitySpotlightMapper
    {
        public static CommunitySpotlightViewModel ToViewModel(this CommunitySpotlight persistence)
        {
            CommunitySpotlightViewModel vm = new CommunitySpotlightViewModel
            {
                UserImage = persistence.UserImage,
                UserName = persistence.UserName,
                Title = persistence.Title,
                Subtitle = persistence.Subtitle,
                ExternalUrl = persistence.ExternalUrl,
                PreviewImageUrl = persistence.PreviewImageUrl,
            };

            return vm;
        }

        public static List<CommunitySpotlightViewModel> ToViewModel(this List<CommunitySpotlight> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static AdminCommunitySpotlightViewModel ToAdminViewModel(this CommunitySpotlight persistence)
        {
            AdminCommunitySpotlightViewModel vm = new AdminCommunitySpotlightViewModel
            {
                Guid = persistence.Guid,
                UserImage = persistence.UserImage,
                UserName = persistence.UserName,
                Title = persistence.Title,
                Subtitle = persistence.Subtitle,
                ExternalUrl = persistence.ExternalUrl,
                PreviewImageUrl = persistence.PreviewImageUrl,
                ActiveDate = persistence.ActiveDate,
                ExpiryDate = persistence.ExpiryDate,
                SortRank = persistence.SortRank,
            };

            return vm;
        }
        public static List<AdminCommunitySpotlightViewModel> ToAdminViewModel(this List<CommunitySpotlight> orig) => orig.Select(o => o.ToAdminViewModel()).ToList();

        public static CommunitySpotlight ToDatabaseModel(this AdminCommunitySpotlightViewModel vm)
        {
            CommunitySpotlight persistence = new CommunitySpotlight
            {
                Guid = vm.Guid,
                UserImage = vm.UserImage,
                UserName = vm.UserName,
                Title = vm.Title,
                Subtitle = vm.Subtitle,
                ExternalUrl = vm.ExternalUrl,
                PreviewImageUrl = vm.PreviewImageUrl,
                ActiveDate = vm.ActiveDate,
                ExpiryDate = vm.ExpiryDate,
                SortRank = vm.SortRank,
            };

            return persistence;
        }
    }
}
