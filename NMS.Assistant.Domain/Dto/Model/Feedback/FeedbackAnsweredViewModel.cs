using System;
using System.Collections.Generic;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Feedback
{
    public class FeedbackAnsweredViewModel
    {
        public Guid FeedbackGuid { get; set; }
        public AppType AppType { get; set; }

        public List<FeedbackQuestionAnsweredViewModel> Answers { get; set; }
    }
}
