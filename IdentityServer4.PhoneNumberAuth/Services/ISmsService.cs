using System.Threading.Tasks;

namespace IdentityServer4.PhoneNumberAuth.Services
{
    public interface ISmsService
    {
        Task<bool> SendAsync(string phoneNumber, string body);
    }
}