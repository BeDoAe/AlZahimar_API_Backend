using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.PatientDTOs
{
    public class PatientProfileDTO
    {
        //Patient
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public int PatientAge { get; set; }
        public string PatientAddress { get; set; }

        [JsonIgnore]
        public Gender PatientGender { get; set; }

        [NotMapped]
        public string PatientGenderString { get; set; }
        public string? PatientPicUrl { get; set; }


        //Relative
        public string RelativeFirstName { get; set; }
        public string RelativeLastName { get; set; }
        public int RelativeAge { get; set; }
        public string RelativeAddress { get; set; }

        [JsonIgnore]
        public Gender RelativeGender { get; set; }

        [NotMapped]
        public string RelativeGenderString { get; set; }
        public string RelativePhoneNumber { get; set; }
        public string? RelativePicURL { get; set; }
      
    }
}
