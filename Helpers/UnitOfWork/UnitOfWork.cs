using Microsoft.AspNetCore.Identity;
using ZahimarProject.Authentication;
using ZahimarProject.Models;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        public SignInManager<AppUser> _signInManager { get; }
        public UserManager<AppUser> _userManager { get; }
        public IPatientService patientService { get; }
        public IPatientRepository PatientRepository { get; }
        public IRelativeService RelativeService { get; }
        public IRelativeRepository RelativeRepository { get; }
        public IDoctorService DoctorService { get; }
        public IDoctorRepository DoctorRepository { get; }
        public ITokenBlackListRepository TokenBlackListRepository { get; }
        public ITokenBlackListService TokenBlackListService { get; }
        public IPatientDoctorRequestRepository PatientDoctorRequestRepository { get; }
        public IPatientDoctorRequestService PatientDoctorRequestService { get; }
        public IMemmoriesRepository MemmoriesRepository { get; set; }
        public IMemoriesServices MemoriesServices { get; set; }
        public IToDoListRepository ToDoListRepository { get; }
        public IToDoListService ToDoListService { get; }
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


        public UnitOfWork(
            Context context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IPatientRepository patientRepository,
            IPatientService patientService,
            IRelativeRepository relativeRepository,
            IRelativeService relativeService,
            IDoctorRepository doctorRepository,
            IDoctorService doctorService,
            ITokenBlackListRepository tokenBlackListRepository,
            ITokenBlackListService tokenBlackListService,
            IPatientDoctorRequestRepository patientDoctorRequestRepository,
            IPatientDoctorRequestService patientDoctorRequestService,
            IMemmoriesRepository memmoriesRepository,
            IMemoriesServices memoriesServices,
            IToDoListRepository toDoListRepository,
            IToDoListService toDoListService,
            IReportRepository ReportRepository,
            IReportService ReportService,
            ITestRepository TestRepository,
            ITestService TestService,
            IPatientTestRepository PatientTestRepository,
            IRatingRepository RatingRepository,
            IRatingService RatingService,
            IStoryRepository storyRepository,
            IStoryServices storyServices,
            IPatientStoryRepository patientStoryRepository,
            IAppointmentRepository AppointmentRepository,
            IAppointmentService AppointmentService,
            IMailService mailService
            

            )

        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            this.patientService = patientService;
            PatientRepository = patientRepository;
            RelativeRepository = relativeRepository;
            RelativeService = relativeService;
            DoctorRepository = doctorRepository;
            DoctorService = doctorService;
            TokenBlackListRepository = tokenBlackListRepository;
            TokenBlackListService = tokenBlackListService;
            PatientDoctorRequestRepository = patientDoctorRequestRepository;
            PatientDoctorRequestService = patientDoctorRequestService;
            MemmoriesRepository = memmoriesRepository;
            MemoriesServices = memoriesServices;
            ToDoListRepository = toDoListRepository;
            ToDoListService = toDoListService;
            this.ReportRepository = ReportRepository;
            this.ReportService = ReportService;
            this.TestRepository = TestRepository;
            this.TestService = TestService;
            this.PatientTestRepository = PatientTestRepository;
            this.RatingRepository = RatingRepository;
            this.RatingService = RatingService;
            StoryServices = storyServices;
            StoryRepository = storyRepository;
            PatientStoryRepository = patientStoryRepository;
            this.AppointmentRepository = AppointmentRepository;
            this.AppointmentService = AppointmentService;
            this.MailService = mailService;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
