using ZahimarProject.DTOS.OrderDTOs;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.PaymentRepo
{
    public interface IDoctorPaymentRepository:IRepository<DoctorPayment>
    {
        public GeneralResponse CreateCheckoutSession(OrderDTO order, string UserType, int userID);

        public GeneralResponse CreateCheckoutSessionOfDoctor(int DoctorId);
        public bool IsDoctorPayment(int DoctorId);


    }
}