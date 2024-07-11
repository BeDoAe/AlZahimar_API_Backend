using ZahimarProject.Authentication;
using ZahimarProject.DTOS.AppointmentDTOs;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.Helpers.SaveImages;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;
using ZahimarProject.Repos.DoctorRepo;
using ZahimarProject.Repos.PatientRepo;

namespace ZahimarProject.Services.DoctorServices
{
    public class DoctorService : Service<Doctor>, IDoctorService
    {
        public IDoctorRepository DoctorRepository { get; }

        public DoctorService(IDoctorRepository DoctorRepository)
        {
            this.DoctorRepository = DoctorRepository;
        }

       
        public DoctorGetDTO GetDoctorDTO(Doctor doctor)
        {
            DoctorGetDTO doctorGetDTO = new()
            {
                Address = doctor.Address,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                UserName = doctor.UserName,
                Age = doctor.Age,
                Gender = doctor.Gender,
                Phone = doctor.Phone,
                PicURL = doctor.PicURL,
                WorksIn=doctor.WorksIn,
                History= doctor.History,
                Price= doctor.Price,
                AverageRating = Math.Round(doctor.AverageRating ?? 0, 1),
                EndTimeOfDay = doctor.EndTimeOfDay,
                StartTimeOfDay = doctor.StartTimeOfDay, 
            };

            return doctorGetDTO;
        }


        //here
        public List<DoctorGetDTO> GetDoctorDTOs(List<Doctor> doctors)
        {
            return doctors.Select(doctor => new DoctorGetDTO
            {
                Address = doctor.Address,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                UserName = doctor.UserName,
                Age = doctor.Age,
                Gender = doctor.Gender,
                Phone = doctor.Phone,
                PicURL = doctor.PicURL,
                WorksIn = doctor.WorksIn,
                History = doctor.History,
                Price = doctor.Price,
                StartTimeOfDay= doctor.StartTimeOfDay,
                EndTimeOfDay= doctor.EndTimeOfDay,  
                AverageRating = Math.Round(doctor.AverageRating ?? 0, 1),
                GenderString = doctor.Gender == Gender.Male ? "Male" : "Female"
            }).ToList();
        }


        public async void GetUpdatedDoctor(DoctorDTO doctorDTO, Doctor UpdatedDoctor)
        {
            UpdatedDoctor.Age = doctorDTO.Age;
            UpdatedDoctor.CardNumber = doctorDTO.CardNumber;
            UpdatedDoctor.Gender = doctorDTO.Gender;
            UpdatedDoctor.Phone = doctorDTO.Phone;
            UpdatedDoctor.Address = doctorDTO.Address;
            UpdatedDoctor.FirstName = doctorDTO.FirstName;
            UpdatedDoctor.LastName = doctorDTO.LastName;
            UpdatedDoctor.UserName = doctorDTO.UserName;
            UpdatedDoctor.WorksIn = doctorDTO.WorksIn;
            UpdatedDoctor.Price = doctorDTO.Price;
            UpdatedDoctor.History = doctorDTO.History;

            //if (doctorDTO.Image != null)
            //{

            //    if (!string.IsNullOrEmpty(UpdatedDoctor.PicURL))
            //    {
            //        DeleteImage(UpdatedDoctor.PicURL);
            //    }

            //    UpdatedDoctor.PicURL = await ImageHelper.SaveImageAsync(doctorDTO.Image);
            //}
        }

       

        public void GetUpdatedAppUser(Doctor doctor, AppUser appUser)
        {
            appUser.UserName = doctor.UserName;
            appUser.FirstName = doctor.FirstName;
            appUser.LastName = doctor.LastName;
            appUser.PhoneNumber = doctor.Phone;
        }

        public List<FilterDoctorDTO> GetSearchResult(List<Doctor> doctors)
        {
            return doctors.Select(doctor => new FilterDoctorDTO
            {
                Id = doctor.Id,
                UserName = doctor.UserName,
                Address= doctor.Address,
                Age=doctor.Age,
                AverageRating = Math.Round(doctor.AverageRating ?? 0, 1),
                FirstName = doctor.FirstName,
                LastName= doctor.LastName,
                PicURL= doctor.PicURL,
                Gender=  doctor.Gender,
                Price= doctor.Price,
                WorksIn= doctor.WorksIn,
                GenderString = doctor.Gender == Gender.Male ? "Male" : "Female"
            }).ToList();
        }

        public List<RandomDoctorDTO> GetRandomDoctorsDTO(List<Doctor> doctors)
        {
            return doctors.Select(doctor => new RandomDoctorDTO
            {
                Id = doctor.Id,
                UserName = doctor.UserName,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                History = doctor.History,
                PicURL = doctor.PicURL,
                Price = doctor.Price,
                AverageRating = Math.Round(doctor.AverageRating ?? 0, 1),


            }).ToList();
        }

        public List<FilterDoctorDTO> GetFilteredDoctorsDTO(List<Doctor> doctors)
        {
            return doctors.Select(doctor => new FilterDoctorDTO
            {
                Id = doctor.Id,
                UserName = doctor.UserName,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                WorksIn = doctor.WorksIn,
                PicURL = doctor.PicURL,
                Price = doctor.Price,
                AverageRating = Math.Round(doctor.AverageRating ?? 0, 1),
                Age = doctor.Age,
                Address= doctor.Address,
                Gender = doctor.Gender,
                GenderString = doctor.Gender == Gender.Male ? "Male" : "Female",

            }).ToList();

        }

         public List<SearchDoctorDTO> GetPendingDoctorsDTO(List<Doctor> doctors)
        {
            return doctors.Select(doctor => new SearchDoctorDTO
            {
                Id = doctor.Id,
                UserName = doctor.UserName,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                WorksIn = doctor.WorksIn,
                History = doctor.History,
                PicURL = doctor.PicURL,
                Gender = doctor.Gender,
                GenderString = doctor.Gender == Gender.Male ? "Male" : "Female",

            }).ToList();

        }

        public List<DoctorPendingRequest> GetAllPendingDoctorRequests()
        {
            return DoctorRepository.GetAllPendingDoctorRequests();
        }

        public GeneralResponse EditStartEndWorkOfDoctor(int DoctorId, DoctorToEditStartEndAppointmentDTO doctorToEdit)
        {
           bool isEdited= DoctorRepository.EditStartEndWork(DoctorId, doctorToEdit);
            if (isEdited)
            {
                return new GeneralResponse() { IsSuccess=true,Data="Updated Successfully" };
            }
            return new GeneralResponse() { IsSuccess = false, Data = "Not Found" };

        }

        public GeneralResponse EditDotorStartEndDuration(int DoctorId, AppointmentStartEndDurationDTO appointmentStartEndDurationDTO)
        {
            Doctor doctor = DoctorRepository.Get(d => d.Id == DoctorId);
            if (doctor == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "Not Found" };
            }
            doctor.StartTimeOfDay = appointmentStartEndDurationDTO.StartOfDay;
            doctor.EndTimeOfDay = appointmentStartEndDurationDTO.EndOfDay;
            doctor.AppointmentDuration = TimeSpan.FromMinutes( appointmentStartEndDurationDTO.DurationByMinutes);
            DoctorRepository.Update(doctor);
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = doctor
            };
        }

        public GeneralResponse GetDotorStartEndDuration(int DoctorId)
        {
            Doctor doctor = DoctorRepository.Get(d => d.Id == DoctorId);
            if (doctor == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "Not Found" };
            }
            AppointmentStartEndDurationDTO appointmentStartEndDurationDTO = new AppointmentStartEndDurationDTO()
            {
                DurationByMinutes = (int)doctor.AppointmentDuration.TotalMinutes,
                EndOfDay = doctor.EndTimeOfDay,
                StartOfDay = doctor.StartTimeOfDay

            };     
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = appointmentStartEndDurationDTO
            };
        }



    }
}
