namespace ZahimarProject.DTOS.AppointmentDTOs
{
    public class AppointmentStartEndDurationDTO
    {
        public int DurationByMinutes { get; set; }
        public TimeSpan StartOfDay { get; set; }
        public TimeSpan EndOfDay { get; set; }
    }
}
