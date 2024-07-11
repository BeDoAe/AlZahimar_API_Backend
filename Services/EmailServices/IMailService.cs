using ZahimarProject.Models;

namespace ZahimarProject.Services.EmailServices
{
    public interface IMailService
    {
       public bool SendMail(MailData mailData);

    }
}
