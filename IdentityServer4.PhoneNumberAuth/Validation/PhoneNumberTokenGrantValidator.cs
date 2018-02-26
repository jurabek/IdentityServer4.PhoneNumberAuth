using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace IdentityServer4.PhoneNumberAuth.Validation
{
    public class PhoneNumberTokenGrantValidator : IExtensionGrantValidator
    {
        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            throw new NotImplementedException();
        }

        public string GrantType { get; }
    }
}
