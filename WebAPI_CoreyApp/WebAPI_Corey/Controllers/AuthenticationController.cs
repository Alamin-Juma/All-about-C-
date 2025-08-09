using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Corey.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    public record AuthenticationData(string? Username, string? Password);
    public record UserData(int? UserId, string? UserName);
    //api/Authentication/token
    [HttpPost("token")]
    public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
    {
        var user = ValidateCredentials(data);

        if(user is null)
        {
            return Unauthorized();
        }

        //In a real-world application, you would generate a JWT token here.
    }


    private string GenerateToke(UserData user)
    {

    }
    private UserData  ValidateCredentials(AuthenticationData data)
    {
        //This is not for production use. In a real-world application, you would validate the credentials against a database or an authentication service.
        if(CompareValues(data.Username, "admin") && CompareValues(data.Password, "password"))
        {
            return new UserData(1, data.Username!);
        }
        if (CompareValues(data.Username, "alamin") && CompareValues(data.Password, "alamin123"))
        {
            return new UserData(2, data.Username!);
        }

        return null;
    }


    private bool CompareValues(string? actual, string? expected)
    {
        if (actual is not null) {
            if(actual.Equals(expected))  
            {
                return true;
            }
        }
        return false;
    }
}
