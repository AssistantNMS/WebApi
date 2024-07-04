using System;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.Feedback.Filled
{
    public class FeedbackWithQuestionsAndAnswersPerUserViewModel
    {
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<FeedbackAnswersFromUserViewModel> QuestionsAndAnswersPerUser { get; set; }
    }
}
