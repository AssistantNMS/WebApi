using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.OnlineMeetup;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class OnlineMeetup2020SubmissionMapper
    {
        public static OnlineMeetup2020SubmissionViewModel ToViewModel(this OnlineMeetup2020Submission persistence)
        {
            OnlineMeetup2020SubmissionViewModel vm = new OnlineMeetup2020SubmissionViewModel
            {
                Guid = persistence.Guid,
                UserImage = persistence.UserImage,
                UserName = persistence.UserName,
                ImageUrl = persistence.ImageUrl,
                Text = persistence.Text,
                ExternalUrl = persistence.ExternalUrl,
                SortRank = persistence.SortRank,
            };

            return vm;
        }

        public static List<OnlineMeetup2020SubmissionViewModel> ToViewModel(this List<OnlineMeetup2020Submission> orig) => orig.Select(o => o.ToViewModel()).ToList();
        
        public static OnlineMeetup2020Submission ToDatabaseModel(this OnlineMeetup2020SubmissionViewModel vm, Guid guid)
        {
            OnlineMeetup2020Submission persistence = new OnlineMeetup2020Submission
            {
                Guid = guid,
                UserImage = vm.UserImage,
                UserName = vm.UserName,
                ImageUrl = vm.ImageUrl,
                Text = vm.Text,
                ExternalUrl = vm.ExternalUrl,
                SortRank = vm.SortRank,
            };

            return persistence;
        }
    }
}
