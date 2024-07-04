using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Feedback
{
    public class AddFeedbackQuestionViewModel
    {
        public string Question { get; set; }

        public FeedbackQuestionType Type { get; set; }

        public bool ContainsPotentiallySensitiveInfo { get; set; }
    }
}
