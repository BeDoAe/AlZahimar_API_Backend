using ZahimarProject.Models;

namespace ZahimarProject.Repos.TokenBlackListRepo
{
    public interface ITokenBlackListRepository : IRepository<TokenBlacklist>
    {
       
        bool Any(Func<TokenBlacklist, bool> predicate);
    }
}
