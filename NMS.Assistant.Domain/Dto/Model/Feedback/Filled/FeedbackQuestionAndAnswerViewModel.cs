using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Feedback.Filled
{
    public class FeedbackQuestionAndAnswerViewModel
    {
        public string Question { get; set; }

        public FeedbackQuestionType QuestionType { get; set; }

        public string Answer { get; set; }
    }
}
