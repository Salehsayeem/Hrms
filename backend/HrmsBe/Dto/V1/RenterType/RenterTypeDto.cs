namespace HrmsBe.Dto.V1.RenterType
{
    public class CreateOrUpdateRenterTypesDto
    {
        public string UserId { get; set; }
        public List<CreateOrUpdateRenterTypeDto> Data { get; set; }
    }
    public class CreateOrUpdateRenterTypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class GetRenterTypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class RenterTypePaginationDto
    {
        public List<RenterTypeDataDto> Response { get; set; } = default!;
        public int CurrentPage { get; set; } = default!;
        public int TotalCount { get; set; } = default!;
        public int PageSize { get; set; } = default!;
    }

    public class RenterTypeDataDto
    {
        public int Sl { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
