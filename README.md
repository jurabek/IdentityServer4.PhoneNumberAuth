# IdentityServer4.PhoneNumberAuth
Sample passwordless phone number authentication with IdentityServer4 in ASP.NET Core 2.0 

To be able to test locally change `"ReturnVerifyTokenForTesting : true"` on `appsettings.json` it will returns us `verify_token` on response, however in production it should be false and you should implement real SMS service. 

```console
curl -H "Content-Type: application/json" \ 
     -X POST \ 
     -d '{"phone":"+198989822"}' \ 
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
     -d grant_type=phone_number_token&client_id=phone_number_authentication&client_secret=secret&phone_number=+198989822&verification_token=373635 \ 
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
