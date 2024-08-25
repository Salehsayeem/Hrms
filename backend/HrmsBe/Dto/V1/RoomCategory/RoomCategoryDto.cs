namespace HrmsBe.Dto.V1.RoomCategory
{
    public class CreateOrUpdateRoomCategoriesDto
    {
        public string UserId { get; set; }
        public long HouseId { get; set; }
        public List<CreateOrUpdateRoomCategoryDto> Data { get; set; }
    }
    public class CreateOrUpdateRoomCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } 
    }
    public class GetRoomCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class RoomCategoryPaginationDto
    {
        public List<RoomCategoryDataDto> Response { get; set; } = default!;
        public int CurrentPage { get; set; } = default!;
        public int TotalCount { get; set; } = default!;
        public int PageSize { get; set; } = default!;
    }

    public class RoomCategoryDataDto
    {
        public int Sl { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
