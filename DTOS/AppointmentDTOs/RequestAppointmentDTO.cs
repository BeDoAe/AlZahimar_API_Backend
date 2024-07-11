using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.AppointmentDTOs
{
    public class RequestAppointmentDTO
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }

        public string Reason { get; set; }

    }
}
