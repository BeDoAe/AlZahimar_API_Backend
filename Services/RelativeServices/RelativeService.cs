using ZahimarProject.Authentication;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Helpers.SaveImages;
using ZahimarProject.Models;
using ZahimarProject.Repos.RelativeRepo;

namespace ZahimarProject.Services.RelativeServices
{
    public class RelativeService : Service<Relative> , IRelativeService
    {
        public IRelativeRepository RelativeRepository { get; }
        public RelativeService(IRelativeRepository RelativeRepository)
        {
            this.RelativeRepository = RelativeRepository;
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
        public List<RelativeGetDTO> GetRelativeDTOs(List<Relative> relatives)
        {
            return relatives.Select(relative => new RelativeGetDTO
            {
                Address = relative.Address,
                FirstName = relative.FirstName,
                LastName = relative.LastName,
                UserName = relative.UserName,
                PicURL = relative.PicURL,
                PhoneNumber= relative.PhoneNumber,
                PatientId = relative.PatientId,
                PatientName= relative.Patient.FirstName,
                GenderString = relative.Gender == Gender.Male ? "Male" : "Female"
            }).ToList();
        }

        public RelativeGetProfileDTO RelativeGetProfile (Relative relative)
        {
            RelativeGetProfileDTO relativeDTO = new()
            {
                RelativeAddress = relative.Address,
                RelativeFirstName = relative.FirstName,
                RelativeLastName = relative.LastName,
                RelativeUserName = relative.UserName,
                RelativePhoneNumber = relative.PhoneNumber,
                RelativeGender = relative.Gender,
                PatientId = relative.PatientId,
                PatientName= relative.Patient.FirstName,
                PatientLastName = relative.Patient.LastName,
                PatientAddress = relative.Patient.Address,
                PatientAge= relative.Patient.Age,               
                PatientPicURL = relative.Patient.PicURL,
               

            };

            relativeDTO.RelativeGenderString = relativeDTO.RelativeGender == Gender.Male ? "Male" : "Female";
            relativeDTO.PatientGenderString = relativeDTO.PatientGender == Gender.Male ? "Male" : "Female";
            return relativeDTO;
        }

        public async void GetUpdatedRelative(RelativeDTO relativeDTO , Relative UpdatedRelative)
        {
            UpdatedRelative.FirstName = relativeDTO.RelativeFirstName;
            UpdatedRelative.LastName = relativeDTO.RelativeLastName;
            UpdatedRelative.Gender = relativeDTO.RelativeGender;
            UpdatedRelative.Address= relativeDTO.RelativeAddress;
            UpdatedRelative.PhoneNumber = relativeDTO.RelativePhoneNumber;
            UpdatedRelative.UserName= relativeDTO.RelativeUserName;
            UpdatedRelative.Patient.FirstName = relativeDTO.PatientName;
            UpdatedRelative.Patient.LastName = relativeDTO.PatientLastName;
            UpdatedRelative.Patient.Gender = relativeDTO.PatientGender;
            UpdatedRelative.Patient.Age = relativeDTO.PatientAge;
            UpdatedRelative.Patient.Address = relativeDTO.PatientAddress;

            //if (relativeDTO.Image != null)
            //{
            //    // Delete the old image if it exists
            //    if (!string.IsNullOrEmpty(UpdatedRelative.Patient.PicURL))
            //    {
            //        DeleteImage(UpdatedRelative.Patient.PicURL);
            //    }

            //    // Save the new image
            //    UpdatedRelative.Patient.PicURL = await ImageHelper.SaveImageAsync(relativeDTO.Image);
            //}

        }
        public void GetUpdatedAppUser(Relative relative, AppUser appUser)
        {
            appUser.UserName = relative.UserName;
            appUser.FirstName = relative.FirstName;
            appUser.LastName = relative.LastName;
            appUser.PhoneNumber = relative.PhoneNumber;
            
        }

    }
}
