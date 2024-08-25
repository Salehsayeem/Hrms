using HrmsBe.Dto.V1.RoomCategory;
using HrmsBe.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HrmsBe.Helper
{
    public class MongoDbSettings
    {
        public string ConnectionUri { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }
    public class MongoDbService
    {
        private readonly IMongoCollection<Audit> _auditCollection;
        public MongoDbService(IOptions<MongoDbSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionUri);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            if (!CollectionExists(database, mongoDBSettings.Value.CollectionName))
            {
                database.CreateCollection(mongoDBSettings.Value.CollectionName);
            }

            _auditCollection = database.GetCollection<Audit>(mongoDBSettings.Value.CollectionName);
        }
        private bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter });
            return collections.Any();
        }
        public async Task CreateOrUpdateAudit(AuditDto audit)
        {
            try
            {
                var data = DeviceHelper.GetAdditionalDetails();
                var device = new Device()
                {
                    DeviceName = data.Item1,
                    DeviceModel = data.Item2,
                    MacAddress = data.Item3,
                    IpV6Address = data.Item4,
                    IpAddress = data.Item5
                };
                var newAuditDetails = new AuditDetails()
                {
                    Description = audit.Description,
                    RequestParameters = audit.RequestParameters,
                    ControllerName = audit.ControllerName,
                    StatusCode = audit.StatusCode,
                    Device = device
                };
                var filter = Builders<Audit>.Filter.Eq(
                    "UserId", audit.UserId);
                var availableAuditByUserId = await _auditCollection.Find(filter).FirstOrDefaultAsync();

                if (availableAuditByUserId != null)
                {
                    var addNewDetails = Builders<Audit>.Update.Push(a => a.Details, newAuditDetails);
                    await _auditCollection.UpdateOneAsync(filter, addNewDetails);
                }
                else
                {
                    Audit obj = new Audit();
                    var details = new List<AuditDetails>();
                    details.Add(newAuditDetails);
                    obj.UserId = audit.UserId;
                    obj.Details = details;
                    await _auditCollection.InsertOneAsync(obj);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public AuditDto CreateAuditInfo(string description,string methodName, object obj, int statusCode, string userId)
        {
            return new AuditDto
            {
                ControllerName = methodName,
                Description = $"{description}",
                RequestParameters = JsonSerializer.Serialize(obj),
                StatusCode = statusCode,
                UserId = userId
            };
        }
    }
}
