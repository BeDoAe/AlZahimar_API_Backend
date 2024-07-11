//using ZahimarProject.Services.AppointmentServices;

//namespace ZahimarProject.BackgroundServices
//{
//    public class DailyResetService //: BackgroundService
//    {
//    //    private readonly IServiceProvider serviceProvider;

//    //    public DailyResetService(IServiceProvider serviceProvider)
//    //    {
//    //        this.serviceProvider = serviceProvider;
//    //    }

//    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    //    {
//    //        while (!stoppingToken.IsCancellationRequested)
//    //        {
//    //            var now = DateTime.Now;
//    //            var nextRunTime = DateTime.Today.AddDays(1); // Next run at midnight
//    //            var delay = nextRunTime - now;

//    //            if (delay.TotalMilliseconds > 0)
//    //            {
//    //                await Task.Delay(delay, stoppingToken);
//    //            }

//    //            if (!stoppingToken.IsCancellationRequested)
//    //            {
//    //                ResetDailyAppointments();
//    //            }
//    //        }
//    //    }

//    //    private void ResetDailyAppointments()
//    //    {
//    //        using (var scope = serviceProvider.CreateScope())
//    //        {
//    //            var appointmentService = scope.ServiceProvider.GetRequiredService<AppointmentService>();
//    //            appointmentService.ResetDailyAppointments();
//    //        }
//    //    }
//    //
//    }
//}
