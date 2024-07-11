using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.PatientDoctorRequestRepo
{
    public class PatientDoctorRequestRepository : Repository<PatientDoctorRequest>, IPatientDoctorRequestRepository
    {
        private readonly Context context;
        public PatientDoctorRequestRepository(Context _context) : base(_context)
        {
            this.context = _context;
        }

        public override PatientDoctorRequest Get(Expression<Func<PatientDoctorRequest, bool>> filter)
        {
            return context.PatientDoctorRequests.Include(r=>r.Patient).Include(r=>r.Doctor).Include(r=>r.Doctor.Patients).FirstOrDefault(filter);
        }

        public override List<PatientDoctorRequest> GetAll()
        {
            return context.PatientDoctorRequests.Include(r=>r.Patient).ToList();
        }

        public void DeleteDoctorOfPatient(PatientDoctorRequest patientDoctorRequest)
        {
             context.Remove(patientDoctorRequest);
            context.SaveChanges();
        }

         public bool IsDoctorAssignedToDoctor(int patientId)
        {
            bool isAssigned= context.
                PatientDoctorRequests.Any(r => r.PatientId == patientId && r.Status==Helpers.Enums.RequestStatus.Accepted);
            return isAssigned;
        }

    }
}
