using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.PatientDTOs
{
    public class PatientGetDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }

        //[Column(TypeName = "nvarchar(7)")]
        [JsonIgnore]
        public Gender Gender { get; set; }

        [NotMapped]
        public string GenderString { get; set; }
        public string? PicURL { get; set; }

    }
}
