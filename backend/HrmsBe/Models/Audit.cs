using HrmsBe.Helper;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HrmsBe.Models
{
    public class Audit
    {
        [BsonId] 
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public List<AuditDetails> Details { get; set; } = new List<AuditDetails>();
    }

    public class AuditDetails
    {
        public string Description { get; set; } = "";
        public string RequestParameters { get; set; } = "";
        public string ControllerName { get; set; } = "";
        public int StatusCode { get; set; }
        public DateTime CreatedDate { get; set; } = CommonHelper.CurrentDateTime();
        public Device Device { get; set; } = new Device();

    }
    public class Device
    {
        public string DeviceName { get; set; } = "";
        public string DeviceModel { get; set; } = "";
        public string MacAddress { get; set; } = "";
        public string IpV6Address { get; set; } = "";
        public string IpAddress { get; set; } = "";
    }

    public class AuditDto
    {
        public string UserId { get; set; }
        public string Description { get; set; } = "";
        public string RequestParameters { get; set; } = "";
        public string ControllerName { get; set; } = "";
        public int StatusCode { get; set; }
    }

}
