using ZahimarProject.Models;
using ZahimarProject.Repos.TokenBlackListRepo;

namespace ZahimarProject.Services.TokenBlackListServices
{
    public interface ITokenBlackListService :IService<TokenBlacklist>
    {
        public ITokenBlackListRepository TokenBlackListRepository { get; }

    }
}
