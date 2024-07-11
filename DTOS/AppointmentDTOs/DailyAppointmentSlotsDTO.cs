namespace ZahimarProject.DTOS.AppointmentDTOs
{
    public class DailyAppointmentSlotsDTO
    {
        public DateTime Date { get; set; }
        public List<AppointmentSlotDTO> AvailableSlots { get; set; }
    }
}
