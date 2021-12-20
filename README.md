# IdentityServer4.PhoneNumberAuth
Sample passwordless phone number authentication with IdentityServer4 in ASP.NET Core 3.1

> ATTENTION: This implementation is not fully rfc6749 complient, for creating custom `grant_type`'s follow instructions [in section 8.3](https://datatracker.ietf.org/doc/html/rfc6749#section-8.3)

> NOTE: To be able to test locally you can change `"ReturnVerifyTokenForTesting : true"` on `appsettings.json` it will returns us `verify_token` on response, however in production usages it must be removed and you should add real SMS service (Twilio, Nexmo, etc..) by implementing `ISmsServices`

```console
curl -H "Content-Type: application/json" \ 
     -X POST \ 
     -d '{"phonenumber":"+198989822"}' \ 
     http://localhost:62537/api/verify_phone_number
```
```json
{
    "resend_token": "CfDJ8F2fHxOfr9xAtc...",
    "verify_token": "373635"
}
```

Authentication by verification token

```console
curl -H "Content-Type: application/x-www-form-urlencoded" \
     -X POST \ 
     -d 'grant_type=phone_number_token&client_id=phone_number_authentication&client_secret=secret&phone_number=%2B198989822&verification_token=373635' \ 
      http://localhost:62537/connect/token
```

```json
{
    "access_token": "CfDJ8F2fHxOfr9xAtc......",
    "expires_in": 3600,
    "token_type": "Bearer",
    "refresh_token": "CfDJ8F2fHxOfr9xAtc...."
}
```

Test your api controller by Bearer token

```console
curl -i http://localhost:62732/api/Identity \
     -H "Authorization: Bearer CfDJ8F2fHxOfr9xAtc......"
```

```json
{
    "type": "phone_number",
    "value": "+198989822"
}
```
