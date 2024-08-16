//using HrmsBe.Helper;
//using HrmsBe.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace HrmsBe.Controllers.v1
//{
//    [Route("api/v{version:apiVersion}/[controller]")]
//    [ApiVersion("1.0")]
//    [ApiController]
//    public class AuditController : ControllerBase
//    {
//        private readonly MongoDbService _mongoDbService;

//        public AuditController(MongoDbService mongoDbService)
//        {
//            _mongoDbService = mongoDbService;
//        }

//        [HttpPost("Log")]
//        public async Task<IActionResult> CreateOrUpdateAudit([FromBody] AuditDto auditDto)
//        {
//            try
//            {
//                await _mongoDbService.CreateOrUpdateAudit(auditDto);
//                return Ok("Audit entry created or updated successfully.");
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }
//    }
//}
