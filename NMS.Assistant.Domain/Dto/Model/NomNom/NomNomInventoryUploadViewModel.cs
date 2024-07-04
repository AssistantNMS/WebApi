using Newtonsoft.Json;
using NMS.Assistant.Domain.Dto.Enum;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.NomNom
{

    public class NomNomInventoryUploadViewModel
    {
        public string Name { get; set; }

        public NomNomInventoryType Type { get; set; }

        public NomNomSubInventoryType SubType { get; set; }

        public List<NomNomInventorySlotUploadViewModel> Slots { get; set; }
    }

    public class NomNomInventorySlotUploadViewModel
    {
        public string GameId { get; set; }

        public int Quantity { get; set; }
    }
}
