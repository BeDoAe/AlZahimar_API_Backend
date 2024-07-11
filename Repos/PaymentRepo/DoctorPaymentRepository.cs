using ZahimarProject.Models;
using ZahimarProject.Repos.PatientRepo;

using Stripe;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore;
using ZahimarProject.Helpers;
using ZahimarProject.DTOS.OrderDTOs;
namespace ZahimarProject.Repos.PaymentRepo
{
    public class DoctorPaymentRepository : Repository<DoctorPayment>, IDoctorPaymentRepository
    {
        private readonly Context context;
        public DoctorPaymentRepository(Context _context) : base(_context)
        {
            this.context = _context;
            StripeConfiguration.ApiKey = "sk_test_51PTkGVBpSxLjU7BBWwPVyJNCahGXXF1Wx6MXfb3IfzO3o4XNDrelxeU0m42bCjtaIWH7Bpd2OcdEnwOA7xtMobpc00dyy9e7ii";

        }
        public GeneralResponse CreateCheckoutSessionOfDoctor(int DoctorId)
        {
            Doctor doctor=context.Doctors.FirstOrDefault(d=>d.Id==DoctorId);
            if (doctor==null)
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
                    UnitAmountDecimal = GeneralClass.DoctorCostCostPerMonth * 100,  // Stripe expects the amount in the smallest currency unit
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = doctor.UserName,
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
                SuccessUrl = "http://localhost:5251/order-success",
                CancelUrl = "http://localhost:5251"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            DoctorPayment doctorPayment = new DoctorPayment()
            {
                Name = doctor.UserName,
                Price = GeneralClass.DoctorCostCostPerMonth,
                DoctorId = DoctorId,
                
            };
            context.DoctorPayments.Add(doctorPayment);
            context.SaveChanges();

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = session.Url
            };
        }

        
        public GeneralResponse CreateCheckoutSession(OrderDTO order , string UserType , int userID)
        {
            if(order==null||userID==0||UserType==null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data="Not Found"
                };
            }
            var orderReference = Guid.NewGuid().ToString();

            var lineItems = new List<SessionLineItemOptions>();
            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = order.Price * 100,  // Stripe expects the amount in the smallest currency unit
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = order.Name,
                        Description = orderReference
                    }
                },
                Quantity=1,
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

            // Save order information to the database
            if (UserType==UserRoles.Doctor)
            {
                DoctorPayment doctorPayment= new DoctorPayment()
                { 
                    Name= order.Name,
                    Price= order.Price,
                    
                    DoctorId= userID,
                };
                context.DoctorPayments.Add(doctorPayment);
                context.SaveChanges();
            }
            else if (UserType==UserRoles.Relative)
            {
                RelativePayment relativePayment = new RelativePayment()
                {
                    Name = order.Name,
                    Price = order.Price,
                    
                    RelativeID = userID,
                };
                context.RelativePayments.Add(relativePayment);
                context.SaveChanges();
            }
            else
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found"
                };
            }
            
            
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = session.Url
            };
            
        }


        public bool IsDoctorPayment(int DoctorId)
        {
            bool exists = context.DoctorPayments.Any(p => p.DoctorId == DoctorId);
            return exists;
        }

        

    }
}
