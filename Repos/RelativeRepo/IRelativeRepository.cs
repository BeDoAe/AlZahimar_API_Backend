using System.Linq.Expressions;
using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.RelativeRepo
{
    public interface IRelativeRepository:IRepository<Relative>
    {
        public Relative GetRelative(int PatientId);
        public  Relative Get(Expression<Func<Relative, bool>> filter);

        public string GetMailOfRelativeToSendEmail(int PatientId);


    }
}
