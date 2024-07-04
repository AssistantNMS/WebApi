using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.WhatIsNew;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class WhatIsNewMapper
    {
        public static WhatIsNewViewModel ToViewModel(this WhatIsNew whatsNew)
        {
            WhatIsNewViewModel vm = new WhatIsNewViewModel
            {
                Guid = whatsNew.Guid,
                Title = whatsNew.Title,
                IsAndroid = whatsNew.IsAndroid,
                IsIos = whatsNew.IsIos,
                IsWebApp = whatsNew.IsWebApp,
                IsWeb = whatsNew.IsWeb,
                IsDiscord = whatsNew.IsDiscord,
                Description = whatsNew.Description,
                ActiveDate = whatsNew.ActiveDate
            };

            return vm;
        }

        public static List<WhatIsNewViewModel> ToViewModel(this List<WhatIsNew> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static WhatIsNew ToDatabaseModel(this WhatIsNewViewModel vm)
        {
            WhatIsNew persistence = new WhatIsNew
            {
                Guid = vm.Guid,
                Title = vm.Title,
                Description = vm.Description,
                IsAndroid = vm.IsAndroid,
                IsIos = vm.IsIos,
                IsWebApp = vm.IsWebApp,
                IsWeb = vm.IsWeb,
                IsDiscord = vm.IsDiscord,
                ActiveDate = vm.ActiveDate
            };

            return persistence;
        }

        public static WhatIsNew ToDatabaseModel(this AddWhatIsNewViewModel vm)
        {
            WhatIsNew persistence = new WhatIsNew
            {
                Guid = Guid.NewGuid(),
                Title = vm.Title,
                Description = vm.Description,
                IsAndroid = vm.IsAndroid,
                IsIos = vm.IsIos,
                IsWebApp = vm.IsWebApp,
                IsWeb = vm.IsWeb,
                IsDiscord = vm.IsDiscord,
                ActiveDate = vm.ActiveDate
            };

            return persistence;
        }
    }
}
