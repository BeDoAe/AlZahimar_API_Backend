using System.Linq.Expressions;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.PatientDoctorRequestRepo
{
    public interface IPatientDoctorRequestRepository:IRepository<PatientDoctorRequest>
    {
        public PatientDoctorRequest Get(Expression<Func<PatientDoctorRequest, bool>> filter);
        public void DeleteDoctorOfPatient(PatientDoctorRequest patientDoctorRequest);

        public bool IsDoctorAssignedToDoctor(int patientId);

    }
}