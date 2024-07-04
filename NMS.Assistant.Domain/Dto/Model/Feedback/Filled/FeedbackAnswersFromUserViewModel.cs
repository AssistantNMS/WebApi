using System;
using System.Collections.Generic;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Feedback.Filled
{
    public class FeedbackAnswersFromUserViewModel
    {
        public Guid UserGuid { get; set; }
        public DateTime AnswerDate { get; set; }
        public AppType AppType { get; set; }
        public List<FeedbackQuestionAndAnswerViewModel> Responses { get; set; }
    }
}
