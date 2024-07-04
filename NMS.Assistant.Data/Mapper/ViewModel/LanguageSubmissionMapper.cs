using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Language;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class LanguageSubmissionMapper
    {
        public static LanguageFileViewModel ToViewModel(this LanguageSubmission langFile)
        {
            LanguageFileViewModel vm = new LanguageFileViewModel
            {
                Guid = langFile.Guid,
                Name = langFile.UserContactDetails,
                Filename = langFile.Filename,
                Content = string.Empty
            };

            return vm;
        }

        public static List<LanguageFileViewModel> ToViewModel(this List<LanguageSubmission> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static LanguageSubmission ToDatabase(this LanguageFileViewModel langFile)
        {
            LanguageSubmission vm = new LanguageSubmission
            {
                Guid = Guid.NewGuid(),
                Filename = langFile.Filename,
                UserContactDetails = langFile.Name,
                DateSubmitted = DateTime.Now,
            };

            return vm;
        }
    }
}
