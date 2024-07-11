using System.Linq.Expressions;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.MemmoriesRepo
{
    public interface IMemmoriesRepository : IRepository<Memmories>
    {

        public List<Memmories> GetAllMemoriesForPatient(int PatientId);

        public bool CustomMemmoryDelete(Memmories memmory);


    }
}
