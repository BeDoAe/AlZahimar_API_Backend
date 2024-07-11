using Microsoft.EntityFrameworkCore;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.ToDoListRepo
{
    public class ToDoListRepository : Repository<ToDoList>, IToDoListRepository
    {
        private Context context;
        internal DbSet<ToDoList> ToDoList;
        public ToDoListRepository(Context _context) : base(_context)
        {
            context = _context;
            ToDoList = context.ToDoLists;
        }

        public override List<ToDoList> GetAll()
        {
            IQueryable<ToDoList> toDoLists = context.ToDoLists.OrderByDescending(toDoLists => toDoLists.Time);
            
            return toDoLists.ToList();
        }

        public List<ToDoList> GetAll2(int patientid)
        {
            IQueryable<ToDoList> toDoLists = context.ToDoLists.Where(t => t.PatientId == patientid).OrderByDescending(toDoLists => toDoLists.Time);

            return toDoLists.ToList();
        }

        public override void Delete(ToDoList toDoList)
        {
            toDoList.IsDeleted = true;
            base.Delete(toDoList);
        }
    }
}