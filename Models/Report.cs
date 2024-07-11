using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Models;

namespace ZahimarProject.Models
{
    public class Report
    {
        public Report()
        {
            Ranking= ReportRanking.Normal;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }


        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public bool IsDeleted { get; set; }

        
        [Column(TypeName = "nvarchar(10)")]
        public ReportRanking Ranking { get; set; }
    }



    
}
