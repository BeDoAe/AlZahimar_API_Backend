using System.ComponentModel.DataAnnotations.Schema;
using ZahimarProject.Authentication;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.Enums;

namespace ZahimarProject.Models
{
    public class Doctor
    {

        public Doctor()
        {

            Status= DoctorAccountStatus.Pending;
            StartTimeOfDay=GeneralClass.StartTimeOfDay;
            EndTimeOfDay=GeneralClass.EndTimeOfDay;

            AppointmentDuration = TimeSpan.FromMinutes(GeneralClass.AppointmentDuration);

        }
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CardNumber { get; set; }
        public int Age { get; set; }
        [Column (TypeName = "nvarchar(7)")]
        public Gender Gender { get; set; }
        public List<Patient> Patients { get; set; }= new List<Patient> ();
        public List<Report> Reports { get; set; }
        public List<PatientDoctorRequest> PatientDoctorRequests { get; set; } = new List<PatientDoctorRequest>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public bool IsDeleted { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string? PicURL { get; set; }

        //new

        public string?  WorksIn { get; set; }
        public string? History { get; set; }
        public double? Price { get; set; }

        public double? AverageRating { get; set; }

        [Column(TypeName = "nvarchar(10)")]

        public DoctorAccountStatus Status { get; set; }

        public TimeSpan StartTimeOfDay { get; set; }
        public TimeSpan EndTimeOfDay { get; set; }


        public TimeSpan AppointmentDuration { get; set; }

        public List<DoctorPayment> DoctorPayments { get; set; }
    }
}
