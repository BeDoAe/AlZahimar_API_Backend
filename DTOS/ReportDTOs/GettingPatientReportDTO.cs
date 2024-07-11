using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Helpers.Enums;

namespace ZahimarProject.DTOS.ReportDTOs
{
    public class GettingPatientReportDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set;}

        public DateTime DateTime { get; set;}
        public int DoctorID { get; set;}
        public int PatientID { get; set; }
        [JsonIgnore]
        public ReportRanking Ranking { get; set; }
        [NotMapped]
        public string ReportStatus { get; set; }



    }
}
