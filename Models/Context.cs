using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZahimarProject.Authentication;
using ZahimarProject.Helpers;

namespace ZahimarProject.Models
{
    public class Context : IdentityDbContext<AppUser>
    {
       

        public Context(DbContextOptions<Context> options) : base(options) { }
    
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Memmories> Memmories { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientStoryTest> PatientStoryTests { get; set; }
        public DbSet<PatientTest> PatientTests  { get; set; }
        public DbSet<Relative> Relatives  { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<StoryQuestionAndAnswer> StoryQuestionAndAnswers { get; set; }
        public DbSet<StoryTest> StoryTests { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestAnswerQuestions> TestAnswerQuestions{ get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }

        public DbSet<TokenBlacklist> TokenBlacklists { get; set; }
        public DbSet<PatientDoctorRequest> PatientDoctorRequests { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        //public DbSet<Order> Orders { get; set; }
        public DbSet<DoctorPayment> DoctorPayments { get; set; }
        public DbSet<RelativePayment> RelativePayments { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            foreach (var model in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(model.Name).Property<bool>("IsDeleted").HasDefaultValue(false); 
                
            }

            modelBuilder.Entity<Appointment>()
         .Property(a => a.Status)
         .HasConversion<string>();

            modelBuilder.Entity<Report>()
                            .Property<DateTime>("CreatedDate")
                            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Doctor>()
                            .Property<TimeSpan>("StartTimeOfDay")
                            .HasDefaultValue(GeneralClass.StartTimeOfDay);
            modelBuilder.Entity<Doctor>()
                            .Property<TimeSpan>("EndTimeOfDay")
                            .HasDefaultValue(GeneralClass.EndTimeOfDay);

            modelBuilder.Entity<Doctor>()
                            .Property<TimeSpan>("AppointmentDuration")
                            .HasDefaultValue(TimeSpan.FromMinutes(GeneralClass.AppointmentDuration));
            modelBuilder.Entity<DoctorPayment>()
                            .Property<int>("Quantity")
                            .HasDefaultValue(1);
            modelBuilder.Entity<DoctorPayment>()
                            .Property<DateTime>("OederDate")
                            .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<RelativePayment>()
                            .Property<int>("Quantity")
                            .HasDefaultValue(1);
            modelBuilder.Entity<RelativePayment>()
                            .Property<DateTime>("OederDate")
                            .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Doctor>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Memmories>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Patient>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PatientStoryTest>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PatientTest>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Relative>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Report>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<StoryQuestionAndAnswer>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<StoryTest>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Test>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<TestAnswerQuestions>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<ToDoList>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PatientDoctorRequest>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Test>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<TestAnswerQuestions>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Rating>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Appointment>().HasQueryFilter(e => !e.IsDeleted);


            

        }

    }
}
