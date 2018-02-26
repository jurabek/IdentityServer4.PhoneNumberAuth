using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.PhoneNumberAuth.Models;
using IdentityServer4.PhoneNumberAuth.Services;
using IdentityServer4.PhoneNumberAuth.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer4.PhoneNumberAuth.Controllers
{
    [Route("api/verify_phone_number")]
    public class VerifyPhoneNumberController : ControllerBase
    {
        private readonly ISmsService _smsService;
        private readonly DataProtectorTokenProvider<ApplicationUser> _dataProtectorTokenProvider;
        private readonly PhoneNumberTokenProvider<ApplicationUser> _phoneNumberTokenProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerifyPhoneNumberController(
            ISmsService smsService,
            DataProtectorTokenProvider<ApplicationUser> dataProtectorTokenProvider,
            PhoneNumberTokenProvider<ApplicationUser> phoneNumberTokenProvider,
            UserManager<ApplicationUser> userManager)
        {
            _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
            _dataProtectorTokenProvider = dataProtectorTokenProvider ?? throw new ArgumentNullException(nameof(dataProtectorTokenProvider));
            _phoneNumberTokenProvider = phoneNumberTokenProvider ?? throw new ArgumentNullException(nameof(phoneNumberTokenProvider));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost]
        public async Task<IActionResult> Post(PhoneLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await GetUser(model);
            var result = await SendSmsRequet(model, user);
            if (!result)
            {
                return BadRequest("Sending sms failed");
            }

            var resendToken = await _dataProtectorTokenProvider.GenerateAsync("resend_token", _userManager, user);
            var body = new Dictionary<string, string> { { "resend_token", resendToken } };
            return Accepted(body);
        }
        
        [HttpPut]
        public async Task<IActionResult> Put(string resendToken, [FromBody]PhoneLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await GetUser(model);
            if (!await _dataProtectorTokenProvider.ValidateAsync("resend_token", resendToken, _userManager, user))
            {
                return BadRequest("Invalid resend token");
            }

            var result = await SendSmsRequet(model, user);

            if (!result)
            {
                return BadRequest("Sending sms failed");
            }

            var newResendToken = await _dataProtectorTokenProvider.GenerateAsync("resend_token", _userManager, user);
            var body = new Dictionary<string, string> { { "resend_token", newResendToken } };
            return Accepted(body);
        }

        private async Task<ApplicationUser> GetUser(PhoneLoginViewModel loginViewModel)
        {
            var phoneNumber = _userManager.NormalizeKey(loginViewModel.PhoneNumber);
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber) 
                ?? new ApplicationUser
                {
                    PhoneNumber = loginViewModel.PhoneNumber,
                    SecurityStamp = loginViewModel.PhoneNumber.Sha256()
                };
            return user;
        }

        private async Task<bool> SendSmsRequet(PhoneLoginViewModel model, ApplicationUser user)
        {
            var token = await _phoneNumberTokenProvider.GenerateAsync("verify_number", _userManager, user);
            var result = await _smsService.SendAsync(model.PhoneNumber, $"Your login verification code is: {token}");
            return result;
        }
    }
}
