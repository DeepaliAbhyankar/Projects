using Microsoft.AspNetCore.Http;

namespace CustomerDirectory
{
    public interface IUserProvider
    {
        string Username { get; }
    }
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Username
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.Identity.Name ?? string.Empty;
            }
        }
    }
}
