using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.PhoneNumberAuth.Services
{
    public class SmsService : ISmsService
    {
        public Task<bool> SendAsync(string phoneNumber, string body)
        {
            throw new NotImplementedException();
        }
    }
}
