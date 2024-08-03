using HrmsBe.Models.BaseModel;

namespace HrmsBe.Models
{
    public class RoomCategory:BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public long HouseId { get; set; } 
    }
}
