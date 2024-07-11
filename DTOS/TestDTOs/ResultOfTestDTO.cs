namespace ZahimarProject.DTOS.TestDTOs
{
    public class ResultOfTestDTO
    {
        public string PatientName { get; set; }
        public string TestTitle { get; set; }
        public int PatientScore { get; set; }
        public int TestScore { get; set; }
        public DateTime DateTaken { get; set; }
        public Dictionary<string, string> ?patientAnswers { get; set; }
        public Dictionary<string, string> ?TestAnswers { get; set; }

    }
}
