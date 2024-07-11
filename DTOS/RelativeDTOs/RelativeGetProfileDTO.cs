using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.RelativeDTOs
{
    public class RelativeGetProfileDTO
    {
        public string RelativeFirstName { get; set; }
        public string RelativeLastName { get; set; }
        public string RelativeUserName { get; set; }
        public string RelativePhoneNumber { get; set; }
        public string RelativeAddress { get; set; }
        [JsonIgnore]
        public Gender RelativeGender { get; set; }

        [NotMapped]
        public string RelativeGenderString { get; set; }

        //patient
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientLastName { get; set; }

        [NotMapped]
        public string PatientGenderString { get; set; }
        public int PatientAge { get; set; }
        public string PatientAddress { get; set; }

        public string? PatientPicURL { get; set; }
        [JsonIgnore]
        public Gender PatientGender { get; set; }
      
    }
}
