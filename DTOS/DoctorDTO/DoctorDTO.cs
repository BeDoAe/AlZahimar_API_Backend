using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Authentication;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.DoctorDTO
{
    public class DoctorDTO 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int CardNumber { get; set; }
        public int Age { get; set; }
        //[Column(TypeName = "nvarchar(7)")]
        [JsonIgnore]
        public Gender Gender { get; set; }
        //[NotMapped]
        //public string GenderString { get; set; }
        //public IFormFile? Image { get; set; }

        public string? WorksIn { get; set; }
        public string? History { get; set; }
        public double? Price { get; set; }

        //public double? AverageRating { get; set; }
        //
    }
}
