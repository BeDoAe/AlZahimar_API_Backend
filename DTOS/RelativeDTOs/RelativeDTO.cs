using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.RelativeDTOs
{
    public class RelativeDTO
    {
        public string RelativeFirstName { get; set; }
        public string RelativeLastName { get; set; }
        public string RelativeUserName { get; set; }
        public string RelativePhoneNumber { get; set; }
        public string RelativeAddress { get; set; }
        [JsonIgnore]
        public Gender RelativeGender { get; set; }

        //patient

        public string PatientName { get; set; }
        public string PatientLastName { get; set; }
        public int PatientAge { get; set; }
        public string PatientAddress { get; set; }
        [JsonIgnore]
        public Gender PatientGender { get; set; }
        //public IFormFile Image { get; set; }


        //[NotMapped] 
        //public string GenderString { get; set; }

    }
}