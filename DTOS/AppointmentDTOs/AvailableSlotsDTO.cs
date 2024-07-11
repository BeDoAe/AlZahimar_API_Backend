
namespace ZahimarProject.DTOS.AppointmentDTOs
{
    public class AvailableSlotsDTO
    {
        public AvailableSlotsDTO()
        {
            Id = _nextId++;
        }
        private static int _nextId = 1;

        public int Id { get; private set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string FormattedSlot { get; set; }
    }
}
