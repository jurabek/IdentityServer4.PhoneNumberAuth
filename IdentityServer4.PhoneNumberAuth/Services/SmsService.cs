using System.Threading.Tasks;

namespace IdentityServer4.PhoneNumberAuth.Services
{
    public class SmsService : ISmsService
    {
        public Task<bool> SendAsync(string phoneNumber, string body)
        {
            return Task.FromResult(true);
        }
    }
}