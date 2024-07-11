using System.ComponentModel.DataAnnotations.Schema;

namespace ZahimarProject.Models
{
    public class Patient
    {
        //additons
        //
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        List<PatientTest> PatientTests { get; set; }
        //List<PatientDoctor> PatientsDoctors { get; set; }
        List<Memmories> Memmories { get; set; } 
        List<ToDoList> ToDoList { get; set; }
        List<Report> Reports { get; set; }
        List<PatientStoryTest> PatientStoryTests { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();


        [Column (TypeName = "nvarchar(7)")]
        public Gender Gender { get; set; }
        public string? PicURL { get; set; }


    }
}
