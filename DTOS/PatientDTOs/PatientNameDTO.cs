using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.PatientDTOs
{
    public class PatientNameDTO
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public string FullName { get; set; }

        public string? PicUrl { get; set; }

        public DateTime Time { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }

        [JsonIgnore]
        public RequestStatus RequestStatus { get; set; }

        [NotMapped]
        public string Status { get; set; }
    }
}
