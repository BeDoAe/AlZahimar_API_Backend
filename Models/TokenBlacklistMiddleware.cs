using ZahimarProject.Helpers.UnitOfWorkFolder;

namespace ZahimarProject.Models
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var isBlacklisted = unitOfWork.TokenBlackListRepository.Any(tb => tb.Token == token && tb.ExpirationDate > DateTime.UtcNow);
                    if (isBlacklisted)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Unauthorized: Token has been revoked.");
                        return;
                    }
                }
            }

            await _next(context);
        }

    }
}
