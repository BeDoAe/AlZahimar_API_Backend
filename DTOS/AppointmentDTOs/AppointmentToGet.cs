using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Helpers.Enums;

namespace ZahimarProject.DTOS.AppointmentDTOs
{
    public class AppointmentToGet
    {

        public int Id { get; set; }
        public string Date { get; set; }

        public string? FullName { get; set; }

        public string? PicUrl { get; set; }

        public int? PatientId { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [JsonIgnore]
        public AppointmentStatus Status { get; set; }

        [NotMapped]
        public string StatusName =>Status.ToString();


    }
}
