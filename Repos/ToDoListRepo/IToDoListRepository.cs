using ZahimarProject.Models;

namespace ZahimarProject.Repos.ToDoListRepo
{
    public interface IToDoListRepository : IRepository<ToDoList>
    {
        public List<ToDoList> GetAll2(int patientid);

    }
}
