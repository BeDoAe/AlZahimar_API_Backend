using System.Linq.Expressions;
using ZahimarProject.DTOS.AppointmentDTOs;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.AppointmentRepo
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        public bool IsRequestAppointmentAvailable(RequestAppointmentDTO requestAppointmentDTO, int DoctorId);
        public Appointment GetAppointmentForAccept(int id);



        public List<AppointmentToGet> GetAllPending(int doctorId);

        public List<AppointmentToGet> GetAllAccepted(int doctorId);
        public List<AppointmentToGet> GetAllRejected(int doctorId);
        public List<AppointmentToGet> GetAllCompleted(int doctorId);
        public List<AppointmentToGet> GetAllDeleted(int doctorId);

        public int GetAllAppointmentsCountOfDoctor(int doctorId);
        public int GetPendingAppointmentsCountOfDoctor(int doctorId);
        public int GetAcceptedAppointmentsCountOfDoctor(int doctorId);
        public int GetRejectedAppointmentsCountOfDoctor(int doctorId);
        public int GetCompletedAppointmentsCountOfDoctor(int doctorId);
        public int GetDeletedAppointmentsCountOfDoctor(int doctorId);


        public List<Appointment> GetAcceptedAppointmentsOfToday();
        public List<Appointment> GetAcceptedAfterAppointmentsOfToday();
        public List<Appointment> GetAcceptedPreviousAppointmentsOfToday();

        public List<Appointment> GetAcceptedAfterAppointmentsOfTodayOfPatient(int PatientId);



        public bool DeleteAppointment(int DoctorId, AppointmentDateAndTimeDTO dateAndTime);

        public List<Appointment> GetUnavailableAppointmentsOfDoctor(int DoctorId);

        public bool DeleteAppointmentById(int appointmentId);

    }
}