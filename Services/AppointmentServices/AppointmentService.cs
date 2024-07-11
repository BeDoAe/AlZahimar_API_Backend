using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ZahimarProject.DTOS.AppointmentDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Models;
using ZahimarProject.Repos.AppointmentRepo;
using ZahimarProject.Repos.DoctorRepo;
using ZahimarProject.Repos.RelativeRepo;
using ZahimarProject.Services.EmailServices;

namespace ZahimarProject.Services.AppointmentServices
{
    public class AppointmentService : Service<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository AppointmentRepository;
        private readonly IRelativeRepository relativeRepository;

        private readonly IMailService mailService;


        public AppointmentService(IRelativeRepository relativeRepository ,IAppointmentRepository AppointmentRepository, Context _context, IMailService mailService)
        {
            this.AppointmentRepository = AppointmentRepository;
            this.mailService=mailService;
            this.relativeRepository = relativeRepository;
        }

        private Appointment GetRequestedAppointment(RequestAppointmentDTO requestAppointmentDTO, int PatientId, int? DoctorId)
        {
            Appointment appointment = new Appointment()
            {

                AppointmentDate = requestAppointmentDTO.AppointmentDate,
                AppointmentTime = requestAppointmentDTO.AppointmentTime,
                DoctorId = (int)DoctorId,
                PatientId = PatientId,
                Reason = requestAppointmentDTO.Reason,
                Status = AppointmentStatus.Pending,
            };
            return appointment;
        }
        public GeneralResponse AddAppointmentRequest(RequestAppointmentDTO requestAppointmentDTO, int PatientId, int? DoctorId)
        {
            DateTime today = DateTime.Today;


            DateTime oneWeekFromToday = today.AddDays(7);

            // Check if the requested appointment date is in the past
            if (requestAppointmentDTO.AppointmentDate.Date < today)
            {
                return new GeneralResponse
                {
                    Data = "This appointment date is in the past. Please select a future date.",
                    IsSuccess = false,
                };
            }

            // Check if the requested appointment date is beyond one week from today
            if (requestAppointmentDTO.AppointmentDate.Date > oneWeekFromToday)
            {
                return new GeneralResponse
                {
                    Data = "You can only select appointments within one week from today. Please select another date.",
                    IsSuccess = false,
                };
            }

            // Check if the requested appointment time has already passed today
            if (requestAppointmentDTO.AppointmentDate.Date == today && requestAppointmentDTO.AppointmentTime < DateTime.Now.TimeOfDay)
            {
                return new GeneralResponse
                {
                    Data = "The appointment time has already passed for today. Please select a future time.",
                    IsSuccess = false,
                };
            }

            //int time = DateTime.Now.Hour;
            //int tim2= requestAppointmentDTO.AppointmentTime;
            //if(requestAppointmentDTO.(in)AppointmentTime<time)
            //{

            //}

            bool IsAppointmentFree = AppointmentRepository.IsRequestAppointmentAvailable(requestAppointmentDTO, (int)DoctorId);
            if (IsAppointmentFree)
            {
                Appointment appointment = GetRequestedAppointment(requestAppointmentDTO, PatientId, DoctorId);
                AppointmentRepository.Insert(appointment);
                return new GeneralResponse
                {
                    Data = requestAppointmentDTO,
                    IsSuccess = true,
                };
            }
            return new GeneralResponse
            {
                Data = "This appointment is not available. Please select another one.",
                IsSuccess = false,
            };
        }
        public bool AcceptAppointment(int appointmentId)
        {
            Appointment appointment = AppointmentRepository.GetAppointmentForAccept(appointmentId);
            if (appointment == null || appointment.Status != AppointmentStatus.Pending)
            {
                return false;
            }
            else
            {
                appointment.Status = AppointmentStatus.Accepted;
                AppointmentRepository.Update(appointment);
                string RelativeMail = relativeRepository.GetMailOfRelativeToSendEmail((int)appointment.PatientId);
                MailData mailData = new MailData()
                {
                    EmailToId = RelativeMail,
                    EmailToName = "the Relative of Patient",
                   EmailBody =$"" +
                   $"Dear {RelativeMail} Your appointment has been accepted\n" +
                   $"Appointment Details:\n" +
                   $"\tDate: {appointment.AppointmentDate:ddd MMM dd}\n" +
                   $"\tTime: {appointment.AppointmentTime}\n" +
                   $"\tDr: {appointment.Doctor.UserName}\n" +
                   $"\tDr Address: {appointment.Doctor.Address}\n" +
                   $"\tDr Phone: {appointment.Doctor.Phone}\n" +
                   $"\tThank you for choosing our services.\n\n" +
                   $"Best regards\n," +
                   $"Your RemindMe Team\n" 
                  
                  ,  EmailSubject = "Acceptance of Reserved Appointment"
                };
                mailService.SendMail(mailData);
                return true;
            }
        }
        public bool RejectAppointment(int appointmentId)
        {
            Appointment appointment = AppointmentRepository.GetAppointmentForAccept(appointmentId);
            if (appointment == null || appointment.Status != AppointmentStatus.Pending)
            {
                return false;
            }
            else
            {
                appointment.Status = AppointmentStatus.Rejected;
                AppointmentRepository.Update(appointment);
                string RelativeMail = relativeRepository.GetMailOfRelativeToSendEmail((int)appointment.PatientId);
                MailData mailData = new MailData()
                {
                    EmailToId = RelativeMail,
                    EmailToName = "the Relative of Patient",
                    EmailBody = $"" +
                   $"Dear {RelativeMail} Your appointment has been Rejected Please Re Choose Another Appointment\n" +           
                   $"Thank you for choosing our services.\n" +
                   $"Best regards,\n\n" +
                   $"RemindMe Team\n"

                  ,
                    EmailSubject = "Rejected of Reserve Appointment"
                };


                mailService.SendMail(mailData);
                return true;
            }
        }
        public bool CompleteAppointment(int appointmentId)
        {
            Appointment appointment = AppointmentRepository.GetAppointmentForAccept(appointmentId);
            if (appointment == null || appointment.Status != AppointmentStatus.Accepted)
            {
                return false;
            }
            else
            {
                appointment.Status = AppointmentStatus.Completed;
                AppointmentRepository.Update(appointment);
                string RelativeMail = relativeRepository.GetMailOfRelativeToSendEmail((int)appointment.PatientId);
                MailData mailData = new MailData()
                {
                    EmailToId = RelativeMail,
                    EmailToName = "the Relative of Patient",
                    EmailBody = $"" +
                   $"Dear {RelativeMail} Your appointment has been Completed\n" +
                   $"We Hope your Health Improves Quickly\n" +
                   $"Please, Remember To Rate\n" + //remember to put Websit link when we publish it               
                   $"Thank you for choosing our services.\n\n" +
                   $"Best regards\n" +
                   $"RemindMe Team\n"

                  ,
                    EmailSubject = "Completion of Examination"
                };
                mailService.SendMail(mailData);
                return true;

            }
        }
        public GeneralResponse GetAllPending(int DoctorId)
        {
            List<AppointmentToGet> appointmentToGets= AppointmentRepository.GetAllPending(DoctorId);    
            if(appointmentToGets.Count > 0)
            {
                return new GeneralResponse() 
                {
                    Data = appointmentToGets,
                    IsSuccess=true,
                };
            }
            return new GeneralResponse()
            {
                Data = "No Appointments Requested Yet",
                IsSuccess = false,
            };
        }
        public GeneralResponse GetAllAccepted(int DoctorId)
        {
            List<AppointmentToGet> appointmentToGets = AppointmentRepository.GetAllAccepted(DoctorId);
            if (appointmentToGets.Count > 0)
            {
                return new GeneralResponse()
                {
                    Data = appointmentToGets,
                    IsSuccess = true,
                };
            }
            return new GeneralResponse()
            {
                Data = "No Accepted Appointments Yet",
                IsSuccess = false,
            };
        }
        public GeneralResponse GetAllRejected(int DoctorId)
        {
            List<AppointmentToGet> appointmentToGets = AppointmentRepository.GetAllRejected(DoctorId);
            if (appointmentToGets.Count > 0)
            {
                return new GeneralResponse()
                {
                    Data = appointmentToGets,
                    IsSuccess = true,
                };
            }
            return new GeneralResponse()
            {
                Data = "No Rejected Appointments Yet",
                IsSuccess = false,
            };
        }
        public GeneralResponse GetAllCompleted(int DoctorId)
        {
            List<AppointmentToGet> appointmentToGets = AppointmentRepository.GetAllCompleted(DoctorId);
            if (appointmentToGets.Count > 0)
            {
                return new GeneralResponse()
                {
                    Data = appointmentToGets,
                    IsSuccess = true,
                };
            }
            return new GeneralResponse()
            {
                Data = "No Completed Appointments Yet",
                IsSuccess = false,
            };
        }
        public GeneralResponse GetAllDeleted(int DoctorId)
        {
            List<AppointmentToGet> appointmentToGets = AppointmentRepository.GetAllDeleted(DoctorId);
            if (appointmentToGets.Count > 0)
            {
                return new GeneralResponse()
                {
                    Data = appointmentToGets,
                    IsSuccess = true,
                };
            }
            return new GeneralResponse()
            {
                Data = "No Deleted Appointments Yet",
                IsSuccess = false,
            };
        }
        public GeneralResponse GetAllAppointmentsCountOfDoctor(int DoctorId)
        {
            int count = AppointmentRepository.GetAllAppointmentsCountOfDoctor(DoctorId);
            if (count > 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = count,
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found",
            };
        }
        public GeneralResponse GetPendingAppointmentsCountOfDoctor(int DoctorId)
        {
            int count = AppointmentRepository.GetPendingAppointmentsCountOfDoctor(DoctorId);
            if (count > 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = count,
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found Pending Appointments",
            };
        }
        public GeneralResponse GetAcceptedAppointmentsCountOfDoctor(int DoctorId)
        {
            int count = AppointmentRepository.GetAcceptedAppointmentsCountOfDoctor(DoctorId);
            if (count > 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = count,
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found Accepted Appointments",
            };
        }
        public GeneralResponse GetRejectedAppointmentsCountOfDoctor(int DoctorId)
        {
            int count = AppointmentRepository.GetRejectedAppointmentsCountOfDoctor(DoctorId);
            if (count > 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = count,
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found Rejected Appointments",
            };
        }
        public GeneralResponse GetCompletedAppointmentsCountOfDoctor(int DoctorId)
        {
            int count = AppointmentRepository.GetCompletedAppointmentsCountOfDoctor(DoctorId);
            if (count > 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = count,
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found Completed Appointments",
            };
        }
        public GeneralResponse GetDeletedAppointmentsCountOfDoctor(int DoctorId)
        {
            int count = AppointmentRepository.GetDeletedAppointmentsCountOfDoctor(DoctorId);
            if (count > 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = count,
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found Deleted Appointments",
            };
        }
        public GeneralResponse GetPreviousAppointmentsOfToday()
        {
            List<Appointment> appointments = AppointmentRepository.GetAcceptedPreviousAppointmentsOfToday();
            if (appointments.Count==0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found Accepted Appointments",
                };
            }
            List<AppointmentToGet> appointmentToGets = new List<AppointmentToGet>();
            foreach(Appointment appointment in appointments)
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
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = appointmentToGets,
            };

        }


        public GeneralResponse GetAfterAppointmentsOfToday()
        {
            List<Appointment> appointments = AppointmentRepository.GetAcceptedAfterAppointmentsOfToday();
            if (appointments.Count == 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found Accepted Appointments",
                };
            }
            List<AppointmentToGet> appointmentToGets = new List<AppointmentToGet>();
            foreach (Appointment appointment in appointments)
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
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = appointmentToGets,
            };

        }


        public GeneralResponse GetAppointmentsOfToday()
        {
            List<Appointment> appointments = AppointmentRepository.GetAcceptedAppointmentsOfToday();
            if (appointments.Count == 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found Accepted Appointments",
                };
            }
            List<AppointmentToGet> appointmentToGets = new List<AppointmentToGet>();
            foreach (Appointment appointment in appointments)
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
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = appointmentToGets,
            };

        }


        public GeneralResponse GetAfterAppointmentsOfTodayOfPatient(int PatientId)
        {
            List<Appointment> appointments = AppointmentRepository.GetAcceptedAfterAppointmentsOfTodayOfPatient(PatientId);
            if (appointments.Count == 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found Accepted Appointments",
                };
            }
            List<AppointmentToGet> appointmentToGets = new List<AppointmentToGet>();
            foreach (Appointment appointment in appointments)
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
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = appointmentToGets,
            };

        }


        public GeneralResponse DeleteAppointment(int DoctorId , AppointmentDateAndTimeDTO dateAndTimeDTO)
        {
            bool IsDeleted=AppointmentRepository.DeleteAppointment(DoctorId, dateAndTimeDTO);
            if (IsDeleted)
            {
                return new GeneralResponse()
                { 
                    IsSuccess=true,
                    Data="Deleted Successfully"
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found"
            };
        }

        public GeneralResponse DeleteAppointmentById(int appointmentId)
        {
            bool isDeleted = AppointmentRepository.DeleteAppointmentById(appointmentId);

            if (isDeleted)
            {
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = "Deleted Successfully"
                };
            }

            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found"
            };
        }



        public GeneralResponse GetUnavailableAppointmentOfDoctor(int DoctorId)
        {
            List<Appointment> UnavailableAppointment=AppointmentRepository.GetUnavailableAppointmentsOfDoctor(DoctorId);
            List<AppointmentToKnowAvailabilityDTO> UnAvailableAppointmentDTO = new List<AppointmentToKnowAvailabilityDTO>();

            foreach (var appointment in UnavailableAppointment)
            {
                AppointmentToKnowAvailabilityDTO UnAvailableDTO =
                    new AppointmentToKnowAvailabilityDTO()
                    {
                        Date = appointment.AppointmentDate.ToString("ddd MMM dd"),
                        StartTime = appointment.AppointmentTime,
                        EndTime = appointment.AppointmentTime.Add(appointment.Doctor.AppointmentDuration),
                    };
                UnAvailableAppointmentDTO.Add(UnAvailableDTO);
            }
            return new GeneralResponse() 
            { 
                IsSuccess=true,
                Data= UnAvailableAppointmentDTO
            };
        }

        public GeneralResponse GetAvailableAppointmentsOfDoctor(Doctor doctor)
        {
            List<Appointment> unavailableAppointments = AppointmentRepository.GetUnavailableAppointmentsOfDoctor(doctor.Id);
            List<DailyAppointmentSlotsDTO> availableAppointmentSlots = GetAvailableAppointmentSlotsForWeek(doctor, unavailableAppointments);

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = availableAppointmentSlots
            };
        }

        private List<DailyAppointmentSlotsDTO> GetAvailableAppointmentSlotsForWeek(Doctor doctor, List<Appointment> unavailableAppointments)
        {
            List<DailyAppointmentSlotsDTO> weeklyAvailableSlots = new List<DailyAppointmentSlotsDTO>();

            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(7);

            for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
            {
                List<AppointmentSlotDTO> dailyAvailableSlots = new List<AppointmentSlotDTO>();
                TimeSpan currentSlotStartTime = doctor.StartTimeOfDay;

                // Skip today if all times are already passed
                if (date == startDate && DateTime.Now.TimeOfDay >= doctor.EndTimeOfDay)
                {
                    continue;
                }

                while (currentSlotStartTime < doctor.EndTimeOfDay)
                {
                    TimeSpan currentSlotEndTime = currentSlotStartTime.Add(doctor.AppointmentDuration);

                    // Skip slots that are already passed today
                    if (date == startDate && currentSlotStartTime < DateTime.Now.TimeOfDay)
                    {
                        currentSlotStartTime = currentSlotEndTime;
                        continue;
                    }

                    bool isUnavailable = unavailableAppointments.Any(a => a.AppointmentDate.Date == date.Date && a.AppointmentTime == currentSlotStartTime);

                    if (!isUnavailable)
                    {
                        dailyAvailableSlots.Add(new AppointmentSlotDTO
                        {
                            StartTime = currentSlotStartTime,
                            EndTime = currentSlotEndTime
                        });
                    }

                    currentSlotStartTime = currentSlotEndTime;
                }

                // Only add the day to the weekly available slots if there are available slots
                if (dailyAvailableSlots.Any())
                {
                    weeklyAvailableSlots.Add(new DailyAppointmentSlotsDTO
                    {
                        Date = date,
                        AvailableSlots = dailyAvailableSlots
                    });
                }
            }

            return weeklyAvailableSlots;
        }


    }
}