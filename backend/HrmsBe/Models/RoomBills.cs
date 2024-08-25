using HrmsBe.Models.BaseModel;

namespace HrmsBe.Models
{
    public class RoomDetails 
    {
        public long Id { get; set; }
        public long RoomId { get; set; }
        public string BillType { get; set; } = string.Empty;
        public string BillOptions { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int NoOfUnits { get; set; }
        public bool IsRecurring { get; set; }

    }
}
