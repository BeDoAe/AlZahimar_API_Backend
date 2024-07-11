namespace ZahimarProject.DTOS.AppointmentDTOs
{
    public class AppointmentToKnowAvailabilityDTO
    {
        public string Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
