namespace ZahimarProject.DTOS.MemmoriesDto
{
    public class AddMemmoryDto
    {
        public string Person_Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
