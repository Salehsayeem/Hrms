using HrmsBe.Helper;

namespace HrmsBe.Models.BaseModel
{
    public class BaseEntity
    {
        public Ulid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Ulid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime ServerActionDateTime { get; set; } = CommonHelper.CurrentDateTime();
    }
}
