using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.Feedback
{
    public class AddFeedbackViewModel
    {
        public string Name { get; set; }

        public List<AddFeedbackQuestionViewModel> Questions { get; set; }
    }
}
