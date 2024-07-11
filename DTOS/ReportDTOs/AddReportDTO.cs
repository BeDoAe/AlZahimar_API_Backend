﻿using ZahimarProject.Helpers.Enums;

namespace ZahimarProject.DTOS.ReportDTOs
{
    public class AddReportDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ReportRanking Ranking { get; set; }


    }
}
