using Microsoft.AspNetCore.Identity;
using ZahimarProject.Authentication;
using ZahimarProject.Repos.DoctorRepo;
using ZahimarProject.Repos.MemmoriesRepo;
using ZahimarProject.Repos.PatientDoctorRequestRepo;
using ZahimarProject.Repos.PatientRepo;
using ZahimarProject.Repos.RelativeRepo;
using ZahimarProject.Repos.ToDoListRepo;
using ZahimarProject.Repos.ReportRepo;
using ZahimarProject.Repos.TokenBlackListRepo;
using ZahimarProject.Services.DoctorServices;
using ZahimarProject.Services.MemoriesServices;
using ZahimarProject.Services.PatientDoctorServices;
using ZahimarProject.Services.TestService;
using ZahimarProject.Services.RelativeServices;
using ZahimarProject.Services.ToDoListServices;
using ZahimarProject.Services.ReportServices;
using ZahimarProject.Services.TokenBlackListServices;
using ZahimarProject.Repos.TestRepo;
using ZahimarProject.Services.TestService;
using ZahimarProject.Services.PatientServices;
using ZahimarProject.Repos.PatientTestRepo;
using ZahimarProject.Repos.RatingRepo;
using ZahimarProject.Services.RatingServices;
using ZahimarProject.Repos.StoryRepo;
using ZahimarProject.Services.StoryServices;
using ZahimarProject.Repos.PatientStoryRepo;
using ZahimarProject.Repos.AppointmentRepo;
using ZahimarProject.Services.AppointmentServices;
using ZahimarProject.Services.EmailServices;

namespace ZahimarProject.Helpers.UnitOfWorkFolder
{
    public interface IUnitOfWork
    {
        public SignInManager<AppUser> _signInManager { get; }
        public UserManager<AppUser> _userManager { get; }
        //public IPatientTestService PatientService { get; }

        public void Save();
        public IRelativeService RelativeService { get; }
        public IRelativeRepository RelativeRepository { get; }
        public IDoctorService DoctorService { get; }
        public IDoctorRepository DoctorRepository { get; }
        public ITokenBlackListRepository TokenBlackListRepository { get; }
        public ITokenBlackListService TokenBlackListService { get; }
        public IPatientDoctorRequestRepository PatientDoctorRequestRepository { get; }
        public IPatientDoctorRequestService PatientDoctorRequestService { get; }
        public IMemmoriesRepository MemmoriesRepository { get; }
        public IMemoriesServices MemoriesServices { get; }
        public IToDoListRepository ToDoListRepository { get; }
        public IToDoListService ToDoListService { get; }
        public IPatientService patientService { get; }
        public IPatientRepository PatientRepository { get; }
        public IReportRepository ReportRepository { get; }
        public IReportService ReportService { get; }
        public ITestRepository TestRepository { get; }
        public ITestService TestService { get; }
        public IPatientTestRepository PatientTestRepository { get; }
        public IRatingRepository RatingRepository { get; }
        public IRatingService RatingService { get; }

        public IStoryRepository StoryRepository { get; }

        public IStoryServices StoryServices { get; }

        public IPatientStoryRepository PatientStoryRepository { get; }

        public IAppointmentRepository AppointmentRepository { get; }
        public IAppointmentService AppointmentService { get; }
        public IMailService MailService { get; }


    }
}
