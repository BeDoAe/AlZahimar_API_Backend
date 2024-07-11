using System.ComponentModel.DataAnnotations.Schema;
using ZahimarProject.Authentication;

namespace ZahimarProject.Models
{
    public class Relative
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [Column(TypeName = "nvarchar(7)")]
        public Gender Gender { get; set; }
        public bool IsDeleted { get; set; }


        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        public AppUser AppUser{ get; set; }
        public string? PicURL { get; set; }

        public List<RelativePayment> RelativePayments { get; set; }

    }
}
