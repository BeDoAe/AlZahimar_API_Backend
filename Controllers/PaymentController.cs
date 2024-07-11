using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZahimarProject.Authentication;
using ZahimarProject.DTOS.OrderDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;
using ZahimarProject.Repos.PaymentRepo;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IDoctorPaymentRepository doctorPaymentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRelativePaymentRepository relativePaymentRepository;

        public PaymentController(IDoctorPaymentRepository doctorPaymentRepository, IUnitOfWork unitOfWork , IRelativePaymentRepository relativePaymentRepository)
        {
            this.doctorPaymentRepository = doctorPaymentRepository;
            this.unitOfWork = unitOfWork;
            this.relativePaymentRepository = relativePaymentRepository;
        }
        Relative GetLogedRelative()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Relative relative = unitOfWork.RelativeRepository.Get(r=>r.AppUserId==LoggedInUserId);
            return relative;
        }
        private Doctor GetLogedInDoctor()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            return doctor;
        }
        //[HttpPost("checkout")]
        //public async Task<ActionResult<GeneralResponse>> CreateCheckoutSessionAsync(OrderDTO order)
        //{
        //    ClaimsPrincipal user = this.User;
        //    if (user == null)
        //    {
        //        return new GeneralResponse()
        //        {
        //            IsSuccess = false,
        //            Data = "Not Found"
        //        }; 
        //    }
        //    string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (LoggedInUserId == null)
        //    {
        //        return new GeneralResponse()
        //        {
        //            IsSuccess = false,
        //            Data = "Not Found"
        //        };
        //    }
        //    AppUser userFromDB = unitOfWork._userManager.Users.FirstOrDefault(u => u.Id == LoggedInUserId);
        //    if (userFromDB == null)
        //    {
        //        return new GeneralResponse()
        //        {
        //            IsSuccess = false,
        //            Data = "Not Found"
        //        };
        //    }
        //    var roles = await unitOfWork._userManager.GetRolesAsync(userFromDB);
        //    var LoggedInRole = roles.FirstOrDefault();
        //    if (LoggedInRole == null)
        //    {
        //        return new GeneralResponse()
        //        {
        //            IsSuccess = false,
        //            Data = "Not Found"
        //        };
        //    }

        //    if(LoggedInRole==UserRoles.Doctor)
        //    {
        //        Doctor doctor=GetLogedInDoctor();
        //        GeneralResponse generalResponse= paymentRepository
        //                           .CreateCheckoutSession(order, LoggedInRole, doctor.Id);
        //        return generalResponse;
        //    }

        //    else
        //    {
        //        Relative relative = GetLogedRelative();
        //        GeneralResponse generalResponse = paymentRepository
        //                           .CreateCheckoutSession(order, LoggedInRole,relative.Id );
        //        return generalResponse;
        //    }

        //}


        [HttpPost("DoctorPayment")]
        [Authorize(policy: UserRoles.Doctor)]
        public ActionResult<GeneralResponse> DoctorPayment()
        {
            Doctor doctor = GetLogedInDoctor();
            GeneralResponse generalResponse = doctorPaymentRepository.CreateCheckoutSessionOfDoctor( doctor.Id);
            return generalResponse;
        }

        [HttpPost("RelativePayment")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<GeneralResponse> RelativePayment()
        {
            Relative relative= GetLogedRelative();
            GeneralResponse generalResponse = relativePaymentRepository.CreateCheckoutSessionOfRelative( relative.Id);
            return generalResponse;
        }

        [HttpGet("IsDoctorPayment")]
        [Authorize(policy: UserRoles.Doctor)]
        public ActionResult<bool> IsDoctorPayment()
        {
            Doctor doctor = GetLogedInDoctor();
            bool isPayment = doctorPaymentRepository.IsDoctorPayment(doctor.Id);
            return isPayment;
        }

        [HttpGet("IsRelativePayment")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<bool> IsRelativePayment()
        {
            Relative relative = GetLogedRelative();
            bool isPayment = relativePaymentRepository.IsRelativePayment(relative.Id);
            return isPayment;
        }

    }
}
