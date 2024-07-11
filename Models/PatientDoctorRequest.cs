using System.ComponentModel.DataAnnotations.Schema;
using ZahimarProject.Helpers.Enums;

namespace ZahimarProject.Models
{
    public class PatientDoctorRequest
    {
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        
        [Column(TypeName = "nvarchar(10)")]
        public RequestStatus Status { get; set; }
        public bool IsDeleted { get; set; }

    }


}
