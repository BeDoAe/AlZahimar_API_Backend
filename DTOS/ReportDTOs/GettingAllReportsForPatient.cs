using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZahimarProject.Helpers.Enums;

namespace ZahimarProject.DTOS.ReportDTOs
{
    public class GettingAllReportsForPatient
    {
        public string Title { get; set; }
        public int ReportID { get; set; }
        public DateTime DateTime { get; set; }

        [JsonIgnore]
        public ReportRanking Ranking { get; set; }
        [NotMapped]
        public string ReportStatus { get; set; }  

    }
}
