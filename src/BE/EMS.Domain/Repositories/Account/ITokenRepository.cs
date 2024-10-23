using Common.Data;
using EMS.Domain.Models.Account;

namespace EMS.Domain.Repositories.Account
{
    public interface ITokenRepository : IBaseRepository<RefreshToken>
    {
        Task<bool> IsTokenValidAsync(string tokenId);
        Task<bool> RevokeToken(string tokenId);
        bool SaveToken(RefreshToken token);
        Task<bool> DeleteTokensByUserIdAsync(string userId);
        Task<RefreshToken> GetTokenByCodeAsync(string tokenCode);

    }
}
