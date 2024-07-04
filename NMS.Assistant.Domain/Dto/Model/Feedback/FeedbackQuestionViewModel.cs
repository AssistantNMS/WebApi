using System;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Feedback
{
    public class FeedbackQuestionViewModel
    {
        public Guid Guid { get; set; }

        public string Question { get; set; }

        public FeedbackQuestionType Type { get; set; }
    }
}
