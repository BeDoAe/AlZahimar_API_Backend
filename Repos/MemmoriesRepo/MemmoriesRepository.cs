using System.Linq;
using System.Linq.Expressions;
using ZahimarProject.Models;
using System.IO;

namespace ZahimarProject.Repos.MemmoriesRepo
{
    public class MemmoriesRepository : Repository<Memmories>, IMemmoriesRepository
    {
        private readonly Context context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MemmoriesRepository(Context _context,IWebHostEnvironment webHostEnvironment) : base(_context)
        {
            context = _context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public  List<Memmories> GetAllMemoriesForPatient(int PatientId)
        {
            List<Memmories> memories = context.Memmories.Where(m=>m.PatientId==PatientId && m.IsDeleted == false).ToList();
            return memories;
        }

        public new bool CustomMemmoryDelete(Memmories memmory)
        {
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Combine the web root path with the relative path from the database
            string fullPath = Path.Combine(webRootPath, memmory.PicURL.TrimStart('/'));
            // Check if the file exists before attempting to delete it
            if (memmory.PicURL != null)
            {
                File.Delete(fullPath);
                context.Memmories.Remove(memmory);
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
