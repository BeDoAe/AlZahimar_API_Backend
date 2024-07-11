using System.ComponentModel.DataAnnotations.Schema;
using ZahimarProject.Helpers.Enums;

namespace ZahimarProject.Models
{
    public class ToDoList
    {
        public ToDoList()
        {
            Status=ToDoListStatus.InProgress;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public DateTime Time { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public ToDoListStatus Status { get; set; }
    }
}
