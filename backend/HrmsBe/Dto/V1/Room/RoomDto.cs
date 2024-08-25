using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HrmsBe.Dto.V1.Room
{

    public class CreateRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public int BasePrice { get; set; }
        public short BillGenerationDate { get; set; } = 10;
        public string UserId { get; set; } = string.Empty;
        public long HouseId { get; set; }
        public long RoomCategoryId { get; set; }
        public bool IsRented { get; set; }
        public List<CreateRoomDetailsDto> Details { get; set; } = new List<CreateRoomDetailsDto>();
    }
    public class CreateRoomDetailsDto
    {
        public string BillType { get; set; } = string.Empty;
        public string BillOptions { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int NoOfUnits { get; set; }
        public bool IsRecurring { get; set; }
    }
    public class UpdateRoomDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BasePrice { get; set; }
        public short BillGenerationDate { get; set; } = 10;
        public string UserId { get; set; } = string.Empty;
        public long HouseId { get; set; }
        public long RoomCategoryId { get; set; }
        public bool IsRented { get; set; }
        public List<UpdateRoomDetailsDto> Details { get; set; } = new List<UpdateRoomDetailsDto>();
    }
    public class UpdateRoomDetailsDto
    {
        public long Id { get; set; }
        public string BillType { get; set; } = string.Empty;
        public string BillOptions { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int NoOfUnits { get; set; }
        public bool IsRecurring { get; set; }
    }
    public class GetRoomDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BasePrice { get; set; }
        public short BillGenerationDate { get; set; } = 10;
        public long HouseId { get; set; }
        public long RoomCategoryId { get; set; }
        public bool IsRented { get; set; }
        public List<GetRoomDetailsDto> Details { get; set; } = new List<GetRoomDetailsDto>();
    }
    public class GetRoomDetailsDto
    {
        public long Id { get; set; }
        public string BillType { get; set; } = string.Empty;
        public string BillOptions { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int NoOfUnits { get; set; }
        public bool IsRecurring { get; set; }
    }

    public class RoomPaginationDto
    {
        public List<RoomDataDto> Response { get; set; } = default!;
        public int CurrentPage { get; set; } = default!;
        public int TotalCount { get; set; } = default!;
        public int PageSize { get; set; } = default!;
    }

    public class RoomDataDto
    {
        public int Sl { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public int BasePrice { get; set; }
        public short BillGenerationDate { get; set; }
        public bool IsRented { get; set; }
    }

}
