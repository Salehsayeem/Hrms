namespace HrmsBe.Dto.V1.House
{
    public class CreateOrUpdateHouseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Contact { get; set; } = "";
        public string UserId { get; set; }
    }
    public class GetHouseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Contact { get; set; } = "";
    }
    public class HousePaginationDto
    {
        public List<HouseDataDto> Response { get; set; } = default!;
        public int CurrentPage { get; set; } = default!;
        public int TotalCount { get; set; } = default!;
        public int PageSize { get; set; } = default!;
    }

    public class HouseDataDto
    {
        public int Sl { get; set; }
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = "";
        public string Contact { get; set; } = "";
    }
}
