using System.ComponentModel.DataAnnotations.Schema;

namespace ZahimarProject.Models
{
    public class Memmories
    {
        public int Id { get; set; }
        public string Person_Name { get; set; }
        public string Description { get; set; }
        public string PicURL { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public bool IsDeleted { get; set; }

    }
}
