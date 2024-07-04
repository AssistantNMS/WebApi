using System;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.Feedback
{
    public class FeedbackViewModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<FeedbackQuestionViewModel> Questions { get; set; }
    }
}
