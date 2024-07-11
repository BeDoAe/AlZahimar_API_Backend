using Stripe;
using Stripe.Checkout;
using ZahimarProject.Helpers;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.PaymentRepo
{
    public class RelativePaymentRepository : Repository<RelativePayment>, IRelativePaymentRepository
    {
        private readonly Context context;
        public RelativePaymentRepository(Context _context) : base(_context)
        {
            this.context = _context;
            StripeConfiguration.ApiKey = "sk_test_51PTkGVBpSxLjU7BBWwPVyJNCahGXXF1Wx6MXfb3IfzO3o4XNDrelxeU0m42bCjtaIWH7Bpd2OcdEnwOA7xtMobpc00dyy9e7ii";

        }

        public GeneralResponse CreateCheckoutSessionOfRelative(int RelativeId)
        {
            Relative relative = context.Relatives.FirstOrDefault(r => r.Id == RelativeId);
            if (relative == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found"
                };
            }

            var orderReference = Guid.NewGuid().ToString();

            var lineItems = new List<SessionLineItemOptions>();
            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = GeneralClass.PatientCostPerMonth * 100,  // Stripe expects the amount in the smallest currency unit
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = relative.UserName,
                        Description = orderReference
                    }
                },
                Quantity = 1,
            });

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/home",
                CancelUrl = "http://localhost:5251"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            RelativePayment relativePayment = new RelativePayment()
            {
                Name = relative.UserName,
                Price = GeneralClass.PatientCostPerMonth,
                RelativeID = RelativeId,

            };
            context.RelativePayments.Add(relativePayment);
            context.SaveChanges();

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = session.Url
            };
        }


        public bool IsRelativePayment(int RelativeId)
        {
            bool exists = context.RelativePayments.Any(p => p.RelativeID == RelativeId);
            return exists;
        }
    }
}
