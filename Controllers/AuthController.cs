using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.Dtos.User;

namespace web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request){
            // we use more info so use User obj
            var response = await _authRepository.Register(
                new User {Username = request.Username} , request.Password
            );
            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request){
            // we use more info so use User obj
            var response = await _authRepository.Login(
                request.Username , request.Password
            );
            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }
        

    }
}