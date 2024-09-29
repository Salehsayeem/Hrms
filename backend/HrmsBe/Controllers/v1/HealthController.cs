using HrmsBe.Context;
using HrmsBe.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HrmsBe.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IMongoClient _mongoClient;
        private readonly ApplicationDbContext _context;
        private readonly MongoDbSettings _mongoDbSettings;
        public HealthController(IMongoClient mongoClient, ApplicationDbContext context, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoClient = mongoClient;
            _context = context;
            _mongoDbSettings = mongoDbSettings.Value;
        }

        [HttpGet("CheckConnection")]
        public bool CheckConnection()
        {
            bool isMongoConnected = _mongoClient.ListDatabaseNames().Any();
            bool isPostgreSqlConnected = _context.Database.CanConnect();

            return isPostgreSqlConnected && isMongoConnected;
        }
    }
}