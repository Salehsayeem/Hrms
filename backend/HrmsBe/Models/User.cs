namespace HrmsBe.Models
{
    public class User : BaseModel.BaseEntity
    {
        public Ulid Id { get; set; }
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
