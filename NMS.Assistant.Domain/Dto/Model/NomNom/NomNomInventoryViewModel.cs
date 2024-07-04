using NMS.Assistant.Domain.Dto.Enum;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.NomNom
{
    public class NomNomInventoryViewModel
    {
        public string Name { get; set; }

        public NomNomInventoryType Type { get; set; }

        public NomNomSubInventoryType SubType { get; set; }

        public List<NomNomInventorySlotViewModel> Slots { get; set; }
    }

    public class NomNomInventorySlotViewModel
    { 
        public string AppId { get; set; }

        public int Quantity { get; set; }
    }
}
