using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Feedback;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class FeedbackMapper
    {
        public static FeedbackViewModel ToViewModel(this Feedback latestFeedback)
        {
            FeedbackViewModel vm = new FeedbackViewModel
            {
                Guid = latestFeedback.Guid,
                Name = latestFeedback.Name,
                CreatedOn = latestFeedback.Created,
                Questions = latestFeedback.Questions.OrderBy(q => q.SortOrder).Select(ToViewModel).ToList()
            };

            return vm;
        }
        public static List<FeedbackViewModel> ToViewModel(this List<Feedback> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static FeedbackQuestionViewModel ToViewModel(this FeedbackQuestion latestFeedback)
        {
            FeedbackQuestionViewModel vm = new FeedbackQuestionViewModel
            {
                Guid = latestFeedback.Guid,
                Question = latestFeedback.Question,
                Type = latestFeedback.Type
            };

            return vm;
        }
        public static FeedbackWithNumResponsesViewModel ToFeedbackWithNumResponsesViewModel(this Feedback latestFeedback)
        {
            FeedbackWithNumResponsesViewModel vm = new FeedbackWithNumResponsesViewModel
            {
                Guid = latestFeedback.Guid,
                Name = latestFeedback.Name,
                CreatedOn = latestFeedback.Created,
                NumberOfResponses = latestFeedback.Answers.GroupBy(fa => fa.Guid).Select(g => g.First()).Count(),
                Questions = latestFeedback.Questions.OrderBy(q => q.SortOrder).Select(ToViewModel).ToList(),
            };

            return vm;
        }
        public static List<FeedbackWithNumResponsesViewModel> ToFeedbackWithNumResponsesViewModel(this List<Feedback> orig) => orig.Select(o => o.ToFeedbackWithNumResponsesViewModel()).ToList(); 

        public static Feedback ToDatabase(this AddFeedbackViewModel newFeedback)
        {
            Feedback domain = new Feedback
            {
                Guid = Guid.NewGuid(),
                Name = newFeedback.Name,
                Created = DateTime.Now,
                Answers = new List<FeedbackAnswer>(),
                Questions = new List<FeedbackQuestion>()//newFeedback.Questions.Select(ToDatabase).ToList()
            };

            for (int qIndex = 0; qIndex < newFeedback.Questions.Count; qIndex++)
            {
                domain.Questions.Add(newFeedback.Questions[qIndex].ToDatabase(qIndex));
            }

            return domain;
        }

        public static FeedbackQuestion ToDatabase(this AddFeedbackQuestionViewModel newFeedback, int index)
        {
            FeedbackQuestion domain = new FeedbackQuestion
            {
                Guid = Guid.NewGuid(),
                Question = newFeedback.Question,
                Type = newFeedback.Type,
                SortOrder = index,
                ContainsPotentiallySensitiveInfo = newFeedback.ContainsPotentiallySensitiveInfo
            };

            return domain;
        }

        //public static Feedback ToDatabase(this FeedbackViewModel newFeedback)
        //{
        //    Feedback domain = new Feedback
        //    {
        //        Guid = Guid.NewGuid(),
        //        Name = newFeedback.Name,
        //        Created = DateTime.Now,
        //        Answers = new List<FeedbackAnswer>(),
        //        Questions = newFeedback.Questions.Select(ToDatabase).ToList()
        //    };

        //    return domain;
        //}

        //public static FeedbackQuestion ToDatabase(this FeedbackQuestionViewModel newFeedback)
        //{
        //    FeedbackQuestion domain = new FeedbackQuestion
        //    {
        //        Guid = newFeedback.Guid,
        //        Question = newFeedback.Question,
        //        Type = newFeedback.Type
        //    };

        //    return domain;
        //}

        public static List<FeedbackAnswer> ToDatabaseModel(this FeedbackAnsweredViewModel feedbackResponse)
        {
            Guid anonymousUserGuid = Guid.NewGuid();
            DateTime dateAnswered = DateTime.Now;

            List<FeedbackAnswer> entries = new List<FeedbackAnswer>();
            foreach (FeedbackQuestionAnsweredViewModel answerObject in feedbackResponse.Answers)
            {
                entries.Add(new FeedbackAnswer
                {
                    FeedbackGuid = feedbackResponse.FeedbackGuid,
                    FeedbackQuestionGuid = answerObject.FeedbackQuestionGuid,
                    Answer = answerObject.Answer,
                    AppType = feedbackResponse.AppType,
                    AnonymousUserGuid = anonymousUserGuid,
                    DateAnswered = dateAnswered
                });
            }

            return entries;
        }
    }
}
