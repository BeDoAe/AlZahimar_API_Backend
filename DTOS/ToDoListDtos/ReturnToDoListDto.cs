using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.ToDoListDtos
{
    public class ReturnToDoListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PatientId { get; set; }
        public string Time { get; set; }

        [JsonIgnore]
        public ToDoListStatus Status { get; set; }

        [NotMapped]
        public string StatusName => Status.ToString();



    }
}
