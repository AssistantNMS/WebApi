using System;

namespace NMS.Assistant.Domain.Dto.Model.WhatIsNew
{
    public class AddWhatIsNewViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsAndroid { get; set; }

        public bool IsIos { get; set; }

        public bool IsWebApp { get; set; }

        public bool IsWeb { get; set; }

        public bool IsDiscord { get; set; }

        public DateTime ActiveDate { get; set; }
    }
}
