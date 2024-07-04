using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.FriendCode
{
    public class AddFriendCodeViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public PlatformType PlatformType { get; set; }
        public string Code { get; set; }
        public string LanguageCode { get; set; }
    }
}
