using HrmsBe.Helper;

namespace HrmsBe.Models.BaseModel
{
    public class BaseEntity
    {
        public Ulid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = CommonHelper.CurrentDateTime();
        public Ulid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime ServerActionDateTime { get; set; } = CommonHelper.CurrentDateTime();
    }
}
