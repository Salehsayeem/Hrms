using HrmsBe.Models.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HrmsBe.Models
{
    public class Room : BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long HouseId { get; set; }
        public long RoomCategoryId { get; set; }
        public int BasePrice { get; set; }
        public short BillGenerationDate { get; set; } = 10;
        public bool IsRented { get; set; }
    }
}
