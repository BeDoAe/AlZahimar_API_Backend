using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZahimarProject.Helpers.Enums;

namespace ZahimarProject.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int ?PatientId { get; set; }
        public Patient ?Patient { get; set; }


        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }

        public string? Reason { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }


    }
}
