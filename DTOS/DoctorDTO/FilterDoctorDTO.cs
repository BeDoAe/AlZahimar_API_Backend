using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.DoctorDTO
{
    public class FilterDoctorDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? PicURL { get; set; }
        public string? WorksIn { get; set; }
        public double? Price { get; set; }
        public double? AverageRating { get; set; }
        public int Age { get; set; }

        public string Address { get; set; }
        [JsonIgnore]
        public Gender Gender { get; set; }
        [NotMapped]
        public string GenderString { get; set; }

    }
}
