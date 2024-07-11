using ZahimarProject.Helpers.DateConverter;
using Newtonsoft.Json;

namespace ZahimarProject.DTOS.AppointmentDTOs
{
    public class AppointmentDateAndTimeDTO
    {
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime date { get; set; }
        public TimeSpan time { get; set; }
    }
}
