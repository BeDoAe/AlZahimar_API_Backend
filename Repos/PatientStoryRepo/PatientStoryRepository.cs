using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.PatientStoryRepo
{
    public class PatientStoryRepository : Repository<PatientStoryTest>, IPatientStoryRepository
    {
        public readonly Context context;

        public PatientStoryRepository(Context _context) : base(_context)
        {
            this.context = _context;

        }
        public List<PatientStoryTest> GetAllPatientStoryTests(int patientId)
        {
            List < PatientStoryTest > patientStoryTest =
             context.PatientStoryTests
                          .Include(pt => pt.Patient)
                          .Include(pt => pt.StoryTest)
                          .Where(pt => pt.PatientId == patientId)
                          .ToList();

            return patientStoryTest;
        }
    }
}
