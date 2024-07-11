using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Helpers.SaveImages;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;
using ZahimarProject.Repos.PaymentRepo;
using ZahimarProject.Services.EmailServices;

namespace ZahimarProject.Repos.DoctorRepo
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {

        private readonly Context context;
        internal DbSet<Doctor> Doctors;
        private readonly IMailService mailService;
        private readonly IDoctorPaymentRepository doctorPaymentRepository;

        public DoctorRepository(Context _context, IMailService mailService , IDoctorPaymentRepository doctorPaymentRepository) : base(_context)
        {
            this.context = _context;
            this.Doctors = context.Doctors;
            this.mailService = mailService;
            this.doctorPaymentRepository = doctorPaymentRepository;
        }
        private void DeleteImage(string imageUrl)
        {
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string fullPath = Path.Combine(webRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task<string> UpdateDoctorPhoto(UploadImageDTO uploadImageDTO, Doctor doctor)
        {
            if (uploadImageDTO.Image != null)
            {
                if (!string.IsNullOrEmpty(doctor.PicURL))
                {
                    DeleteImage(doctor.PicURL);
                }

                doctor.PicURL = await ImageHelper.SaveImageAsync(uploadImageDTO.Image);
                Update(doctor);
                context.SaveChanges();
            }
            return doctor.PicURL;
        }


        public bool IsPatientFoundInDoctorList(int PatientId)
        {
            return Doctors
               .Any(d => d.Patients.Any(p => p.Id == PatientId));
        }

        public List<Doctor> SearchForDoctor(string Name)
        {
            string LowerName = Name.ToLower();
            List<Doctor> doctors = Doctors.Where(d=>d.UserName.ToLower().Contains(Name)).ToList();
            return doctors;
        }

        public List<Doctor> GetRandomDoctors(List<Doctor> allDoctors, int numberOfDoctors = 2)
        {
            Random rng = new Random();
            int n = allDoctors.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Doctor value = allDoctors[k];
                allDoctors[k] = allDoctors[n];
                allDoctors[n] = value;
            }

            return allDoctors.Take(numberOfDoctors).ToList();
        }

        public List<Doctor> GetFilteredDoctorsByPrice(List<Doctor> allDoctors)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.Price > 0
                orderby doctor.Price
                select doctor
            ).ToList();

            return filteredDoctors;
        }

        public List<Doctor> GetDoctorsByAgeGreaterThan(List<Doctor> allDoctors, int Age)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.Age >= Age
                select doctor
            ).ToList();

            return filteredDoctors;
        }

        public List<Doctor> GetDoctorsByAgeSmallerThan(List<Doctor> allDoctors, int Age)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.Age < Age
                select doctor
            ).ToList();

            return filteredDoctors;
        }


        public List<Doctor> GetDoctorsByPriceRange(List<Doctor> allDoctors , int EndRange = 500)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.Price <= EndRange
                select doctor
                ).ToList();
            return filteredDoctors;
        }

        public List<Doctor> GetFilteredDoctorsByAverageRating(List<Doctor> allDoctors)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.AverageRating > 0
                orderby doctor.AverageRating
                select doctor
            ).ToList();

            return filteredDoctors;
        }

        public List<Doctor> GetFilteredDoctorsBySpecificAverageRating(List<Doctor> allDoctors,int AvgRate)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.AverageRating <= AvgRate
                select doctor   
                ).ToList();
            return filteredDoctors;
        }

        public List<Doctor> GetTopRatedDoctors(List<Doctor> allDoctors, int TopNumber)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.AverageRating <= TopNumber
                orderby doctor.AverageRating descending
                select doctor
            ).Take(TopNumber).ToList();

            return filteredDoctors;
        }

        public List<Doctor> GetFilteredMaleDoctors(List<Doctor> allDoctors)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.Gender == 0
              
                select doctor
            ).ToList();

            return filteredDoctors;
        }

        public List<Doctor> GetFilteredFemaleDoctors(List<Doctor> allDoctors)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.Gender != 0
                select doctor
            ).ToList();

            return filteredDoctors;
        }
        public List<Doctor> GetFilteredDoctorsByAge(List<Doctor> allDoctors)
        {
            List<Doctor> filteredDoctors = (
                from doctor in allDoctors
                where doctor.Age > 24
                orderby doctor.Age
                select doctor
            ).ToList();

            return filteredDoctors;
        }

        public List<DoctorPendingRequest> GetAllPendingDoctorRequests()
        {
            List<DoctorPendingRequest> PendingDoctors = Doctors.Where(d => d.Status == DoctorAccountStatus.Pending)
                .Select(d => new DoctorPendingRequest()
                { Id=d.Id,Name=d.FirstName+" "+d.LastName,PicUrl=d.PicURL, Age = d.Age ,Gender = d.Gender == 0 ? "Male" : "Female"  , Date = DateTime.Now})
                .ToList();
            return PendingDoctors;
        }

        public int GetAcceptedDoctorsCount()
        {
            int number = Doctors.Where(d => d.Status == DoctorAccountStatus.Accepted).Count();
            return number;
        }
        
        public GeneralResponse AcceptPendingDoctorRequest(int doctorId)
        {
            Doctor doctor= GetDoctorToHandleLogin(d=>d.Id== doctorId);
            if(doctor != null && doctor.Status==DoctorAccountStatus.Pending)
            {
                doctor.Status = DoctorAccountStatus.Accepted;
                Doctors.Update(doctor); 
                context.SaveChanges();
                MailData mailData = new MailData()
                {
                    EmailToId = doctor.AppUser.Email,
                    EmailToName = doctor.FirstName+" "+doctor.LastName,
                    EmailBody = $"" +
                    $"Dear {doctor.AppUser.Email} Your Join Request has been Accepted \n" +
                    $"Welcome to our Website .\n" +
                    $"Thank you for choosing our services.\n" +
                    $"Best regards,\n\n" +
                    $"RemindMe Team\n"

                    ,
                    EmailSubject = "Acceptence of Join Request"
                };

                mailService.SendMail(mailData);


                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = "Accept Successfully"
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Doctor is Not Found"
            };
        }

        public GeneralResponse RejectPendingDoctorRequest(int doctorId)
        {
            Doctor doctor = GetDoctorToHandleLogin(d => d.Id == doctorId);
            if (doctor != null && doctor.Status == DoctorAccountStatus.Pending)
            {
                doctor.Status = DoctorAccountStatus.Rejected;
                Doctors.Update(doctor);
                context.SaveChanges();

                MailData mailData = new MailData()
                {
                    EmailToId = doctor.AppUser.Email,
                    EmailToName = doctor.FirstName + " " + doctor.LastName,
                    EmailBody = $"" +
                   $"Dear {doctor.AppUser.Email} \n" +
                   $"Sorry.. Your Join Request has been Rejected .\n" +
                   $"Try again later .\n" +
                   $"Thank you for choosing our services.\n" +
                   $"Best regards,\n\n" +
                   $"Your RemindMe Team\n"

                   ,
                    EmailSubject = "Rejection of Join Request"
                };

                mailService.SendMail(mailData);

                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = "Opps.. Rejected"
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Doctor is Not Found"
            };
        }

        public List<Doctor> GetAllPendingDoctors()
        {
            return Doctors.Where(d=>d.Status == DoctorAccountStatus.Pending).ToList();  
        }
        public override List<Doctor> GetAll()
        {
            List<Doctor> DoctorsinDB = Doctors.Where(d => d.Status == DoctorAccountStatus.Accepted).ToList() ;
            List<Doctor> PaymentDoctors=new List<Doctor>() ;
            foreach (var doctor in DoctorsinDB)
            {
                bool isPayment = doctorPaymentRepository.IsDoctorPayment(doctor.Id);
                if (isPayment)
                {
                    PaymentDoctors.Add(doctor);
                }
            }
            return PaymentDoctors;

        }

        public override Doctor Get(Expression<Func<Doctor, bool>> filter)
        {
            return Doctors.Where(d => d.Status == DoctorAccountStatus.Accepted).Include(d=>d.AppUser).FirstOrDefault(filter);
        }

        public Doctor GetDoctorToHandleLogin(Expression<Func<Doctor, bool>> filter)
        {
            return Doctors.Include(d=>d.AppUser).FirstOrDefault(filter);
        }

        public bool EditStartEndWork(int doctorId, DoctorToEditStartEndAppointmentDTO doctorToEdit)
        {
            Doctor doctor = Get(d => d.Id ==doctorId);
            if(doctor!=null)
            {
                doctor.StartTimeOfDay = doctorToEdit.StartTimeOfDay;
                doctor.EndTimeOfDay = doctorToEdit.EndTimeOfDay;
                Doctors.Update(doctor);
                return true;    
            }
                return false;

        }

        
    }
}
