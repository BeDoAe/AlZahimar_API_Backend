using ZahimarProject.DTOS.AppointmentDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.AppointmentRepo;
using ZahimarProject.Repos.DoctorRepo;

namespace ZahimarProject.Services.AppointmentServices
{
    public interface IAppointmentService : IService<Appointment>
    {
        public GeneralResponse AddAppointmentRequest(RequestAppointmentDTO requestAppointmentDTO, int PatientId, int? DoctorId);
        public bool AcceptAppointment(int appointmentId);
        public bool RejectAppointment(int appointmentId);
        public bool CompleteAppointment(int appointmentId);
        public GeneralResponse GetAllPending(int DoctorId);
        public GeneralResponse GetAllAccepted(int DoctorId);
        public GeneralResponse GetAllRejected(int DoctorId);
        public GeneralResponse GetAllCompleted(int DoctorId);
        public GeneralResponse GetAllDeleted(int DoctorId);

        public GeneralResponse GetDeletedAppointmentsCountOfDoctor(int DoctorId);
        public GeneralResponse GetCompletedAppointmentsCountOfDoctor(int DoctorId);
        public GeneralResponse GetRejectedAppointmentsCountOfDoctor(int DoctorId);
        public GeneralResponse GetAcceptedAppointmentsCountOfDoctor(int DoctorId);
        public GeneralResponse GetPendingAppointmentsCountOfDoctor(int DoctorId);
        public GeneralResponse GetAllAppointmentsCountOfDoctor(int DoctorId);

        public GeneralResponse GetPreviousAppointmentsOfToday();
        public GeneralResponse GetAppointmentsOfToday();
        public GeneralResponse GetAfterAppointmentsOfToday();
        public GeneralResponse GetAfterAppointmentsOfTodayOfPatient(int PatientId);
        public GeneralResponse DeleteAppointment(int DoctorId, AppointmentDateAndTimeDTO dateAndTimeDTO);
        public GeneralResponse GetUnavailableAppointmentOfDoctor(int DoctorId);
        public GeneralResponse GetAvailableAppointmentsOfDoctor(Doctor doctor);

        public GeneralResponse DeleteAppointmentById(int appointmentId);

    }
}