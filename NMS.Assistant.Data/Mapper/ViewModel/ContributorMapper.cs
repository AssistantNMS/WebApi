using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Contributor;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class ContributorMapper
    {
        public static ContributorViewModel ToViewModel(this Contributor contributor)
        {
            ContributorViewModel vm = new ContributorViewModel
            {
                Name = contributor.Name,
                Link = contributor.Link,
                SortRank = contributor.SortRank,
                ImageUrl = contributor.ImageUrl,
                Description = contributor.Contribution
            };

            return vm;
        }

        public static List<ContributorViewModel> ToViewModel(this List<Contributor> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static AdminContributorViewModel ToAdminViewModel(this Contributor contributor)
        {
            AdminContributorViewModel vm = new AdminContributorViewModel
            {
                Guid = contributor.Guid,
                Name = contributor.Name,
                Link = contributor.Link,
                SortRank = contributor.SortRank,
                ImageUrl = contributor.ImageUrl,
                Description = contributor.Contribution
            };

            return vm;
        }
        public static List<AdminContributorViewModel> ToAdminViewModel(this List<Contributor> orig) => orig.Select(o => o.ToAdminViewModel()).ToList();

        //public static Contributor ToDatabaseModel(this ContributorViewModel vm) => vm.ToDatabaseModel(Guid.NewGuid());
        public static Contributor ToDatabaseModel(this AdminContributorViewModel vm)
        {
            Contributor persistence = new Contributor
            {
                Guid = vm.Guid,
                Name = vm.Name,
                Link = vm.Link,
                SortRank = vm.SortRank,
                ImageUrl = vm.ImageUrl,
                Contribution = vm.Description
            };

            return persistence;
        }

        public static Contributor ToDatabaseModel(this AddContributorViewModel vm) => vm.ToDatabaseModel(Guid.NewGuid());

        public static Contributor ToDatabaseModel(this ContributorViewModel vm, Guid guid)
        {
            Contributor persistence = new Contributor
            {
                Guid = guid,
                Name = vm.Name,
                Link = vm.Link,
                SortRank = vm.SortRank,
                ImageUrl = vm.ImageUrl,
                Contribution = vm.Description
            };

            return persistence;
        }
        public static Contributor ToDatabaseModel(this AddContributorViewModel vm, Guid guid)
        {
            Contributor persistence = new Contributor
            {
                Guid = guid,
                Name = vm.Name,
                Link = vm.Link,
                SortRank = vm.SortRank,
                ImageUrl = vm.ImageUrl,
                Contribution = vm.Description
            };

            return persistence;
        }
    }
}
