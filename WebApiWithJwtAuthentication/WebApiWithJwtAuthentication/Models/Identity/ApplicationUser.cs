using Microsoft.AspNetCore.Identity;

namespace WebApiWithJwtAuthentication.Models.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    /* same as
        private ICollection<RefreshToken> _refreshTokens;

        public ApplicationUser()
        {
            _refreshTokens = new List<RefreshToken>();
        }

        public ICollection<RefreshToken> RefreshTokens
        {
            get { return _refreshTokens; }
            set { _refreshTokens = value; }
        }
    */
    }
}
