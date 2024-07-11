using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ZahimarProject.Authentication;
using ZahimarProject.Models;
using ZahimarProject.Repos.DoctorRepo;
using ZahimarProject.Repos.MemmoriesRepo;
using ZahimarProject.Repos.PatientDoctorRequestRepo;
using ZahimarProject.Repos.PatientRepo;
using ZahimarProject.Repos.RelativeRepo;
using ZahimarProject.Repos.ToDoListRepo;
using ZahimarProject.Repos.TokenBlackListRepo;
using ZahimarProject.Services.DoctorServices;
using ZahimarProject.Services.MemoriesServices;
using ZahimarProject.Services.PatientDoctorServices;
using ZahimarProject.Services.TestService;
using ZahimarProject.Services.RelativeServices;
using ZahimarProject.Services.ToDoListServices;
using ZahimarProject.Services.TokenBlackListServices;
using ZahimarProject.Controllers;
using ZahimarProject.Repos.ReportRepo;
using ZahimarProject.Services.ReportServices;
using ZahimarProject.Repos.TestRepo;
using ZahimarProject.Services.TestService;
using ZahimarProject.Services.PatientServices;
using ZahimarProject.Repos.PatientTestRepo;
using ZahimarProject.Repos.RatingRepo;
using ZahimarProject.Services.RatingServices;
using ZahimarProject.Services.StoryServices;
using ZahimarProject.Repos.StoryRepo;
using ZahimarProject.Repos.PatientStoryRepo;
using ZahimarProject.Repos.AppointmentRepo;
using ZahimarProject.Services.AppointmentServices;
using ZahimarProject.Services.EmailServices;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;

using Microsoft.Extensions.Options;

using ZahimarProject.Repos.PaymentRepo;

//using MimeKit;

namespace ZahimarProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Read JWT settings from appsettings.json
            var jwtSettings = builder.Configuration.GetSection("JWT");

            var secretKey = jwtSettings["SecritKey"];
            var validIssuer = jwtSettings["ValidIss"];
            var validAudience = jwtSettings["ValidAud"];

            // Add services to the container.
            builder.Services.AddControllers();


            ////Serialization
            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });


            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Zahimar Project API",
                    Description = "API documentation for the Zahimar Project"
                });

                // Configure Swagger to use JWT
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Configure Entity Framework and Identity
            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

            // Configure JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            // Configure Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(UserRoles.Relative, policy =>
                    policy.RequireRole(UserRoles.Relative));
                options.AddPolicy(UserRoles.Doctor, policy =>
                    policy.RequireRole(UserRoles.Doctor));

                options.AddPolicy(UserRoles.Admin, policy =>
                    policy.RequireRole(UserRoles.Admin));

                options.AddPolicy("AdminOrRelative", policy =>
                {
                    policy.RequireRole(UserRoles.Admin, UserRoles.Relative);
                });

                options.AddPolicy("RelativeOrDoctor", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.IsInRole(UserRoles.Relative) ||
                        context.User.IsInRole(UserRoles.Doctor));

                });

                options.AddPolicy("EachRole", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.IsInRole(UserRoles.Admin)||
                        context.User.IsInRole(UserRoles.Relative) ||
                        context.User.IsInRole(UserRoles.Doctor));

                });

            });


            // Register application services
            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IRelativeRepository, RelativeRepository>();
            builder.Services.AddScoped<IRelativeService, RelativeService>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<ITokenBlackListRepository, TokenBlackListRepository>();
            builder.Services.AddScoped<ITokenBlackListService, TokenBlackListService>();
            builder.Services.AddScoped<IPatientDoctorRequestRepository, PatientDoctorRequestRepository>();
            builder.Services.AddScoped<IPatientDoctorRequestService, PatientDoctorRequestService>();
            builder.Services.AddScoped<IMemmoriesRepository, MemmoriesRepository>();
            builder.Services.AddScoped<IMemoriesServices, MemoryServices>();
            builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
            builder.Services.AddScoped<IToDoListService, ToDoListService>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<ITestRepository, TestRepository>();
            builder.Services.AddScoped<ITestService, TestService>();
            builder.Services.AddScoped<IPatientTestRepository, PatientTestRepository >();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<IStoryServices,StoryServices>();
            builder.Services.AddScoped<IStoryRepository, StoryRepository>();
            builder.Services.AddScoped<IPatientStoryRepository,PatientStoryRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IDoctorPaymentRepository, DoctorPaymentRepository>();
            builder.Services.AddScoped<IRelativePaymentRepository, RelativePaymentRepository>();


            //builder.Services.AddScoped<IMailService, MailService>();

            // builder.Services.AddHostedService<DailyResetService>();


            

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200") // Adjust the origin as needed
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // sending email
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IMailService, MailService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAngularApp");
            app.UseMiddleware<TokenBlacklistMiddleware>();

            app.UseStaticFiles();
             
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
