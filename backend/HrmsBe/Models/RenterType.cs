using HrmsBe.Models.BaseModel;

namespace HrmsBe.Models
{
    public class RenterType : BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
    }
}
