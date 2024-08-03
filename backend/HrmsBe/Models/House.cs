using HrmsBe.Models.BaseModel;

namespace HrmsBe.Models
{
    public class House : BaseEntity
    {
        public  long Id { get; set; }
        public string Name { get; set; } = ""; 
        public  string Address { get; set; } = "";
        public  string Contact { get; set; } = "";
    }
}
