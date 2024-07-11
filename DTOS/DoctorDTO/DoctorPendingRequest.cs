using ZahimarProject.Models;

namespace ZahimarProject.DTOS.DoctorDTO
{
    public class DoctorPendingRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; }

        public int Age { get; set; }
        public string Gender { get; set; }
        public DateTime Date { get; set; }
    }
}
