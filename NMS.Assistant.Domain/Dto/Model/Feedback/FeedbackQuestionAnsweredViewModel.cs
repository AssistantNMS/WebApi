using System;

namespace NMS.Assistant.Domain.Dto.Model.Feedback
{
    public class FeedbackQuestionAnsweredViewModel
    {
        public Guid FeedbackQuestionGuid { get; set; }

        public string Answer { get; set; }
    }
}
