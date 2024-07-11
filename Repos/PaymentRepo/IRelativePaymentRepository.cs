using ZahimarProject.Models;

namespace ZahimarProject.Repos.PaymentRepo
{
    public interface IRelativePaymentRepository:IRepository<RelativePayment>
    {
        public bool IsRelativePayment(int RelativeId);
        public GeneralResponse CreateCheckoutSessionOfRelative(int RelativeId);

    }
}