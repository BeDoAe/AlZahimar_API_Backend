using ZahimarProject.Models;
using ZahimarProject.Repos.RelativeRepo;
using ZahimarProject.Repos.TokenBlackListRepo;

namespace ZahimarProject.Services.TokenBlackListServices
{
    public class TokenBlackListService :Service<TokenBlacklist> ,ITokenBlackListService
    {
        public ITokenBlackListRepository TokenBlackListRepository { get; }
        public TokenBlackListService(ITokenBlackListRepository TokenBlackListRepository)
        {
            this.TokenBlackListRepository = TokenBlackListRepository;
        }
    }
}
