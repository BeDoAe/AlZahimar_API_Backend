using ZahimarProject.DTOS.ToDoListDtos;
using ZahimarProject.Models;
using ZahimarProject.Repos.ToDoListRepo;

namespace ZahimarProject.Services.ToDoListServices
{
    public interface IToDoListService : IService<ToDoList>
    {
        public IToDoListRepository ToDoListRepository { get; }

        public ToDoList AddToDoList(ToDoListDto toDoListDto, int PatientId);

        public void Edit(ToDoList toDoList, ToDoListDto toDoListDto);

        public ReturnToDoListDto mapToDto(ToDoList toDoList);

        public List<ReturnToDoListDto> mapListToDto(List<ToDoList> toDoList, int patientId);



    }
}
