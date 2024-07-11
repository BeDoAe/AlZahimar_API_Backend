using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.RelativeDTOs
{
    public class RelativeGetDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        //[Column(TypeName = "nvarchar(7)")]
        [JsonIgnore]
        public Gender Gender { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string? PicURL { get; set; }
        [NotMapped]
        public string GenderString { get; set; }
    }
}
