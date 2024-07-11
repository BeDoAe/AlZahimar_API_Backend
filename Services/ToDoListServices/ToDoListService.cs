using ZahimarProject.DTOS.ToDoListDtos;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Models;
using ZahimarProject.Repos.ToDoListRepo;

namespace ZahimarProject.Services.ToDoListServices
{
    public class ToDoListService : Service<ToDoList> , IToDoListService
    {
        public IToDoListRepository  ToDoListRepository { get;}

        public ToDoListService(IToDoListRepository _ToDoListRepository)
        {
            ToDoListRepository = _ToDoListRepository;
        }

        public ToDoList AddToDoList(ToDoListDto toDoListDto, int PatientId)
        {
            ToDoList toDoList = new ToDoList()
            {
                PatientId = PatientId,
                Description = toDoListDto.Description,
                Name = toDoListDto.Name,
                IsDeleted = false,
                Time = DateTime.Now,
                Status=ToDoListStatus.InProgress,
            };
            return toDoList;
        }

        public void Edit(ToDoList toDoList, ToDoListDto toDoListDto)
        {
            toDoList.Description = toDoListDto.Description;
            toDoList.Name = toDoListDto.Name;
            toDoList.Time = DateTime.Now;
        }

        public ReturnToDoListDto mapToDto(ToDoList toDoList)
        {
            ReturnToDoListDto returnToDoList = new ReturnToDoListDto()
            {
                Id = toDoList.Id,
                Description = toDoList.Description,
                Name = toDoList.Name,
                PatientId = toDoList.PatientId,
                Time = toDoList.Time.ToString("dddd hh:mm tt"),
                Status = toDoList.Status,

            };
            return returnToDoList;
        }

        public List<ReturnToDoListDto> mapListToDto(List<ToDoList> toDoList, int patientId)
        {
            List<ReturnToDoListDto> returnToDoListDtos = new List<ReturnToDoListDto>();
            foreach(var toDo in toDoList)
            {

                ReturnToDoListDto returnToDoListDto = new ReturnToDoListDto()
                {
                    Id=toDo.Id,
                    Description = toDo.Description,
                    Name = toDo.Name,
                    PatientId=toDo.PatientId,
                    Time = toDo.Time.ToString("dddd hh:mm tt"),
                    Status=toDo.Status,
                };
                returnToDoListDtos.Add(returnToDoListDto);
            }
            return returnToDoListDtos;
        }
    }
}
