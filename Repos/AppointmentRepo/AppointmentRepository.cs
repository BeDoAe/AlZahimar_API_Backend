using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZahimarProject.DTOS.AppointmentDTOs;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Models;
using ZahimarProject.Repos.RelativeRepo;
using ZahimarProject.Services.EmailServices;

namespace ZahimarProject.Repos.AppointmentRepo
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private readonly Context context;
        internal DbSet<Appointment> Appointments;
        private readonly IMailService mailService;
        private readonly IRelativeRepository relativeRepository;

        private readonly TimeSpan appointmentDuration = TimeSpan.FromMinutes(60);

        public AppointmentRepository(Context _context, IMailService mailService, IRelativeRepository relativeRepository) : base(_context)
        {
            this.context = _context;
            this.Appointments = context.Appointments;
            this.mailService = mailService;
            this.relativeRepository = relativeRepository;
        }

        public  Appointment GetAppointmentForAccept(int id)
        {
            Appointment appointment=Appointments
                .Include(a=>a.Doctor)
                .FirstOrDefault(a=>a.Id==id);
            return appointment;
            
        }

        
        public bool IsRequestAppointmentAvailable(RequestAppointmentDTO requestAppointmentDTO, int DoctorId)
        {
            // Check for conflicts
            bool conflict = Appointments.
                Any(a => a.DoctorId == DoctorId
                && a.AppointmentDate == requestAppointmentDTO.AppointmentDate
                && a.AppointmentTime == requestAppointmentDTO.AppointmentTime);
            if (conflict)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public List<AppointmentToGet> GetAllPending(int doctorId)
        {
            List<Appointment> PendingAppointments=
                Appointments.Include(a=>a.Doctor).Include(a => a.Patient).Where(a=>a.DoctorId == doctorId && a.Status==AppointmentStatus.Pending)
                    .ToList();
            List<AppointmentToGet> appointmentToGets = new List<AppointmentToGet>();
            foreach (Appointment appointment in PendingAppointments)
            {
                AppointmentToGet appointmentToGet = new AppointmentToGet()
                {
                    Id = appointment.Id,
                    Date = appointment.AppointmentDate.ToString("ddd MMM dd"),
                    StartTime = appointment.AppointmentTime,
                    EndTime = appointment.AppointmentTime
                             .Add(appointment.Doctor.AppointmentDuration) ,
                    Status = appointment.Status,
                    FullName = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}",
                    PatientId = appointment.PatientId,
                    PicUrl = appointment.Patient.PicURL,
                    
                    
                };
                appointmentToGets.Add(appointmentToGet);
            }
            return appointmentToGets;
        }

        public List<AppointmentToGet> GetAllAccepted(int doctorId)
        {
            List<Appointment> PendingAppointments =
                Appointments.Include(a => a.Doctor).Include(a => a.Patient).Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Accepted && a.IsDeleted == false)
                .OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime).ToList();
            List<AppointmentToGet> appointmentToGets = new List<AppointmentToGet>();
            foreach (Appointment appointment in PendingAppointments)
            {
                AppointmentToGet appointmentToGet = new AppointmentToGet()
                {
                    Date = appointment.AppointmentDate.ToString("ddd MMM dd"),
                    StartTime = appointment.AppointmentTime,
                    EndTime = appointment.AppointmentTime
                    .Add(appointment.Doctor.AppointmentDuration),
                    Status = appointment.Status, 
                    FullName = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}",
                    PatientId = appointment.PatientId,
                    Id = appointment.Id,
                    PicUrl = appointment.Patient.PicURL
                };
                appointmentToGets.Add(appointmentToGet);
            }
            return appointmentToGets;
        }

        public List<AppointmentToGet> GetAllRejected(int doctorId)
        {
            List<Appointment> PendingAppointments =
                Appointments.Include(a => a.Doctor).Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Rejected).ToList();
            List<AppointmentToGet> appointmentToGets = new List<AppointmentToGet>();
            foreach (Appointment appointment in PendingAppointments)
            {
                AppointmentToGet appointmentToGet = new AppointmentToGet()
                {
                    Date = appointment.AppointmentDate.ToString("ddd MMM dd"),
                    StartTime = appointment.AppointmentTime,
                    EndTime = appointment.AppointmentTime
                    .Add(appointment.Doctor.AppointmentDuration),
                    Status = appointment.Status
                };
                appointmentToGets.Add(appointmentToGet);
            }
            return appointmentToGets;
        }

        public List<AppointmentToGet> GetAllCompleted(int doctorId)
        {
            List<Appointment> PendingAppointments =
                Appointments.Include(a => a.Doctor).Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Completed)
                .OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime).ToList();
            List<AppointmentToGet> appointmentToGets = new List<AppointmentToGet>();
            foreach (Appointment appointment in PendingAppointments)
            {
                AppointmentToGet appointmentToGet = new AppointmentToGet()
                {
                    Date = appointment.AppointmentDate.ToString("ddd MMM dd"),
                    StartTime = appointment.AppointmentTime,
                    EndTime = appointment.AppointmentTime
                    .Add(appointment.Doctor.AppointmentDuration),
                    Status = appointment.Status
                };
                appointmentToGets.Add(appointmentToGet);
            }
            return appointmentToGets;
        }

        public List<AppointmentToGet> GetAllDeleted(int doctorId)
        {
            List<Appointment> PendingAppointments =
                Appointments.Include(a => a.Doctor).Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Deleted).ToList();
            List<AppointmentToGet> appointmentToGets = new List<AppointmentToGet>();
            foreach (Appointment appointment in PendingAppointments)
            {
                AppointmentToGet appointmentToGet = new AppointmentToGet()
                {
                    Date = appointment.AppointmentDate.ToString("ddd MMM dd"),
                    StartTime = appointment.AppointmentTime,
                    EndTime = appointment.AppointmentTime
                      .Add(appointment.Doctor.AppointmentDuration),
                    Status = appointment.Status
                };
                appointmentToGets.Add(appointmentToGet);
            }
            return appointmentToGets;
        }

        public int GetAllAppointmentsCountOfDoctor(int doctorId)
        {
            List<Appointment> AllAppointments =
                Appointments.Where(a => a.DoctorId == doctorId).ToList();
            return AllAppointments.Count;

        }

        public int GetPendingAppointmentsCountOfDoctor(int doctorId)
        {
            List<Appointment> AllAppointments =
                Appointments.Where(a => a.DoctorId == doctorId && a.Status==AppointmentStatus.Pending).ToList();
            return AllAppointments.Count;

        }
        public int GetAcceptedAppointmentsCountOfDoctor(int doctorId)
        {
            List<Appointment> AllAppointments =
                Appointments.Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Accepted).ToList();
            return AllAppointments.Count;

        }
        public int GetRejectedAppointmentsCountOfDoctor(int doctorId)
        {
            List<Appointment> AllAppointments =
                Appointments.Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Rejected).ToList();
            return AllAppointments.Count;

        }
        public int GetCompletedAppointmentsCountOfDoctor(int doctorId)
        {
            List<Appointment> AllAppointments =
                Appointments.Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Completed).ToList();
            return AllAppointments.Count;

        }
        public int GetDeletedAppointmentsCountOfDoctor(int doctorId)
        {
            List<Appointment> AllAppointments =
                Appointments.Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Deleted).ToList();
            return AllAppointments.Count;

        }


        public List<Appointment> GetAcceptedPreviousAppointmentsOfToday()
        {
            DateTime Today = DateTime.Now;
            List<Appointment> appointments =
                Appointments.Include(a => a.Doctor).Where(a => a.AppointmentDate < Today && a.Status == AppointmentStatus.Accepted).ToList();

            return appointments;
        }
        public List<Appointment> GetAcceptedAfterAppointmentsOfToday( )
        {
            DateTime Today = DateTime.Now;
            List<Appointment> appointments =
                Appointments.Include(a => a.Doctor).Where(a => a.AppointmentDate > Today && a.Status == AppointmentStatus.Accepted).ToList();

            return appointments;
        }
        public List<Appointment> GetAcceptedAppointmentsOfToday()
        {
            DateTime Today = DateTime.Now;
            List<Appointment> appointments =
                Appointments.Include(a => a.Doctor).Where(a => a.AppointmentDate == Today && a.Status == AppointmentStatus.Accepted).ToList();

            return appointments;
        }


        public List<Appointment> GetAcceptedAfterAppointmentsOfTodayOfPatient(int PatientId)
        {
            DateTime Today = DateTime.Now;
            List<Appointment> appointments =
                Appointments.Include(a => a.Doctor).Where(a => a.AppointmentDate >= Today
                && a.Status == AppointmentStatus.Accepted
                && a.PatientId == PatientId
                ).ToList();

            return appointments;
        }

        //Doctor With Appointments

        public bool DeleteAppointment(int DoctorId,AppointmentDateAndTimeDTO dateAndTime)
        {
            if (dateAndTime != null)
            {

                //Appointment appointmentInDB=Get(a=>a.AppointmentDate==dateAndTime.date && a.AppointmentTime == dateAndTime.time);
                
                //if (appointmentInDB != null)
                //{
                //    string RelativeMail = relativeRepository.GetMailOfRelativeToSendEmail((int)appointmentInDB.PatientId);
                //    Relative relative = relativeRepository.GetRelative((int)appointmentInDB.PatientId);
                //    if (appointmentInDB.Status == AppointmentStatus.Pending)
                //    {

                //        MailData mailData = new MailData()
                //        {
                //            EmailToId = RelativeMail,
                //            EmailToName = relative.FirstName + " " + relative.LastName,
                //            EmailBody = $"" +
                //             $"Dear {relative.AppUser.Email} Sorry your Appointment has been Rejected \n" +
                //             $"Try again Later.\n" +
                //             $"Thank you for choosing our services.\n" +
                //             $"Best regards,\n\n" +
                //             $"RemindMe Team\n"
                //             ,
                //            EmailSubject = "Appointment Rejection"
                //        };

                //        mailService.SendMail(mailData);
                //    }
                //    if (appointmentInDB.Status == AppointmentStatus.Accepted)
                //    {
                //        //send mail to tell Patient that the doctor has some problems and can not come in this appointment
                //        MailData mailData = new MailData()
                //        {
                //            EmailToId = RelativeMail,
                //            EmailToName = relative.FirstName + " " + relative.LastName,
                //            EmailBody = $"" +
                //            $"Dear {relative.AppUser.Email} your Appointment has been Canceled \n" +
                //            $"I apologize, but this is due to circumstances beyond my control.\n" +
                //            $"Thank you for choosing our services.\n" +
                //            $"Best regards,\n\n" +
                //            $"RemindMe Team\n"
                //            ,
                //            EmailSubject = "Appointment Cancelation"
                //        };

                //        mailService.SendMail(mailData);
                //    }
                //    appointmentInDB.Status = AppointmentStatus.Deleted;
                //    Update(appointmentInDB);
                //    return true;
                //}
                Appointment NewAppointment = new Appointment()

                    {
                        AppointmentDate = dateAndTime.date,
                        AppointmentTime = dateAndTime.time,
                        DoctorId = DoctorId,
                        Status = AppointmentStatus.Deleted,
                        Reason = "Doctor is Not Available at this Time Of That Day",
                    };

                    Insert(NewAppointment);
                    return true;
                
               
            }
            else
            {
                return false;
            }
        }


        public bool DeleteAppointmentById(int appointmentId)
        {
            // Retrieve the appointment using the appointmentId
            Appointment appointmentInDB = Get(a => a.Id == appointmentId);

            if (appointmentInDB != null)
            {
                // Retrieve the relative's email and relative details
                string relativeMail = relativeRepository.GetMailOfRelativeToSendEmail((int)appointmentInDB.PatientId);
                Relative relative = relativeRepository.GetRelative((int)appointmentInDB.PatientId);

                // Check the appointment status and send the appropriate email
                if (appointmentInDB.Status == AppointmentStatus.Pending)
                {
                    MailData mailData = new MailData()
                    {
                        EmailToId = relativeMail,
                        EmailToName = relative.FirstName + " " + relative.LastName,
                        EmailBody = $"Dear {relative.AppUser.Email}, Sorry your Appointment has been Rejected \n" +
                                    $"Try again Later.\n" +
                                    $"Thank you for choosing our services.\n" +
                                    $"Best regards,\n\n" +
                                    $"RemindMe Team\n",
                        EmailSubject = "Appointment Rejection"
                    };

                    mailService.SendMail(mailData);
                }
                else if (appointmentInDB.Status == AppointmentStatus.Accepted)
                {
                    MailData mailData = new MailData()
                    {
                        EmailToId = relativeMail,
                        EmailToName = relative.FirstName + " " + relative.LastName,
                        EmailBody = $"Dear {relative.AppUser.Email}, your Appointment has been Canceled \n" +
                                    $"I apologize, but this is due to circumstances beyond my control.\n" +
                                    $"Thank you for choosing our services.\n" +
                                    $"Best regards,\n\n" +
                                    $"RemindMe Team\n",
                        EmailSubject = "Appointment Cancellation"
                    };

                    mailService.SendMail(mailData);
                }

                // Mark the appointment as deleted
                appointmentInDB.IsDeleted = true;
                appointmentInDB.Status = AppointmentStatus.Deleted;
                Update(appointmentInDB);
                return true;
            }
            else
            {
                return false; // Appointment not found
            }
        }




        public List<Appointment> GetUnavailableAppointmentsOfDoctor(int DoctorId)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(7);
            List<Appointment>UnAvailableAppointment= Appointments
                                                .Include(a => a.Doctor)
                                                .Where(a => a.DoctorId == DoctorId
                                                && a.AppointmentDate >= startDate 
                                                && a.AppointmentDate < endDate)
                                                .ToList();

            return UnAvailableAppointment;
        }

       

    }
}