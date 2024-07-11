using Microsoft.EntityFrameworkCore;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.TokenBlackListRepo
{
    public class TokenBlackListRepository:Repository<TokenBlacklist> , ITokenBlackListRepository
    {
        public readonly Context context;
        internal DbSet<TokenBlacklist> TokenBlacklists;
        public TokenBlackListRepository(Context _context) : base(_context)
        {
            this.context = _context;
            this.TokenBlacklists = context.TokenBlacklists;
        }

        public bool Any(Func<TokenBlacklist, bool> predicate)
        {
            return TokenBlacklists.Any(predicate);
        }

    }
}
