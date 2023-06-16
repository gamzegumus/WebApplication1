//using WebApplication1.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Components.Routing;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;


//namespace WebApplication1.Controllers
//{
//    [Route("api/[controller]")]
//    [Authorize]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly UserManager<Users> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IConfiguration _configuration;
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        public UsersController(UserManager<Users> userManager,
//            IConfiguration configuration, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _configuration = configuration;
//            _webHostEnvironment = webHostEnvironment;
//        }


//        [HttpPost("CreateRole")]
//        public async Task<IActionResult> CreateRole(CreateRole roleDTO)
//        {

//            var response = await _roleManager.CreateAsync(new IdentityRole
//            {
//                Name = roleDTO.RoleName
//            });

//            if (response.Succeeded)
//            {
//                return Ok("New Role Created");
//            }
//            else
//            {
//                return BadRequest(response.Errors);
//            }
//        }


//        [HttpPost("AssignRoleToUser")]
//        public async Task<IActionResult> AssignRoleToUser(AssignRole assignRoleToUserDTO)
//        {

//            var userDetails = await _userManager.FindByEmailAsync(assignRoleToUserDTO.UserName);

//            if (userDetails != null)
//            {

//                var userRoleAssignResponse = await _userManager.AddToRoleAsync(userDetails, assignRoleToUserDTO.RoleName);

//                if (userRoleAssignResponse.Succeeded)
//                {
//                    return Ok("Role Assigned to User: " + assignRoleToUserDTO.RoleName);
//                }
//                else
//                {
//                    return BadRequest(userRoleAssignResponse.Errors);
//                }
//            }
//            else
//            {
//                return BadRequest("There are no user exist with this email");
//            }


//        }

//        [AllowAnonymous]
//        [HttpPost("RefreshToken")]
//        public async Task<IActionResult> RefreshToken(RefreshTokenReq refreshTokenRequest)
//        {
//            var response = new MainResponse();
//            if (refreshTokenRequest is null)
//            {
//                response.ErrorMessage = "Invalid  request";
//                return BadRequest(response);
//            }

//            var principal = GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);

//            if (principal != null)
//            {
//                var email = principal.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email);

//                var user = await _userManager.FindByEmailAsync(email?.Value);

//                if (user is null || user.RefreshToken != refreshTokenRequest.RefreshToken)
//                {
//                    response.ErrorMessage = "Invalid Request";
//                    return BadRequest(response);
//                }

//                string newAccessToken = GenerateAccessToken(user);
//                string refreshToken = GenerateRefreshToken();

//                user.RefreshToken = refreshToken;
//                await _userManager.UpdateAsync(user);

//                response.IsSuccess = true;
//                response.Content = new LOgin_Response
//                {
//                    RefreshToken = refreshToken,
//                    AccessToken = newAccessToken
//                };
//                return Ok(response);
//            }
//            else
//            {
//                return ErrorResponse.ReturnErrorResponse("Invalid Token Found");
//            }

//        }



//        [AllowAnonymous]
//        [HttpPost("AuthenticateUser")]
//        public async Task<IActionResult> AuthenticateUser(Login_Kullanici authenticateUser)
//        {
//            var user = await _userManager.FindByNameAsync(authenticateUser.Username);
//            if (user == null) return Unauthorized();

//            bool isValidUser = await _userManager.CheckPasswordAsync(user, authenticateUser.Password);

//            if (isValidUser)
//            {
//                string accessToken = GenerateAccessToken(user);
//                var refreshToken = GenerateRefreshToken();
//                user.RefreshToken = refreshToken;
//                await _userManager.UpdateAsync(user);

//                var response = new MainResponse
//                {
//                    Content = new LOgin_Response
//                    {
//                        RefreshToken = refreshToken,
//                        AccessToken = accessToken
//                    },
//                    IsSuccess = true,
//                    ErrorMessage = ""
//                };
//                return Ok(response);
//            }
//            else
//            {
//                return Unauthorized();
//            }

//        }

//        private string GenerateAccessToken(Users user)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();

//            var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

//            var claims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.NameIdentifier, user.Username),
//                    new Claim(ClaimTypes.Name, $"{user.Name} { user.Surname}"),
//                    new Claim(ClaimTypes.Email, user.EMail),


//            };

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Audience = _configuration["JWT:Audience"],
//                Issuer = _configuration["JWT:Issuer"],
//                Expires = DateTime.UtcNow.AddMinutes(30),
//                Subject = new ClaimsIdentity(claims),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyDetail), SecurityAlgorithms.HmacSha256Signature)
//            };
//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            return tokenHandler.WriteToken(token);
//        }
//        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();

//            var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
//            var tokenValidationParameter = new TokenValidationParameters
//            {
//                ValidateIssuer = false,
//                ValidateAudience = false,
//                ValidateLifetime = false,
//                ValidateIssuerSigningKey = true,
//                ValidIssuer = _configuration["JWT:Issuer"],
//                ValidAudience = _configuration["JWT:Audience"],
//                IssuerSigningKey = new SymmetricSecurityKey(keyDetail),
//            };

//            SecurityToken securityToken;
//            var principal = tokenHandler.ValidateToken(token, tokenValidationParameter, out securityToken);
//            var jwtSecurityToken = securityToken as JwtSecurityToken;
//            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
//                throw new SecurityTokenException("Invalid token");
//            return principal;
//        }

//        private string GenerateRefreshToken()
//        {

//            var randomNumber = new byte[32];
//            using (var rng = RandomNumberGenerator.Create())
//            {
//                rng.GetBytes(randomNumber);
//                return Convert.ToBase64String(randomNumber);
//            }
//        }

//        [AllowAnonymous]
//        [HttpPost("RegisterUser")]
//        public async Task<IActionResult> RegisterUser(New_Kulanici registerUserDTO)
//        {

//            var userToBeCreated = new Users
//            {
//                EMail = registerUserDTO.Email,
//                Name = registerUserDTO.Name,
//                Surname = registerUserDTO.Surname,
//                Username = registerUserDTO.Username,
//                Password = registerUserDTO.Password,
//                Password_tekrar = registerUserDTO.Password_tekrar
//            };


//            if (!string.IsNullOrWhiteSpace(registerUserDTO.UserAvatar))
//            {
//                byte[] imgBytes = Convert.FromBase64String(registerUserDTO.UserAvatar);
//                string fileName = $"{Guid.NewGuid()}_{userToBeCreated.FirstName.Trim()}_{userToBeCreated.LastName.Trim()}.jpeg";
//                string avatar = await UploadFile(imgBytes, fileName);
//                userToBeCreated.UserAvatar = avatar;
//            }

//            var response = await _userManager.CreateAsync(userToBeCreated, registerUserDTO.Password);
//            if (response.Succeeded)
//            {
//                return Ok(new MainResponse
//                {
//                    IsSuccess = true,
//                });
//            }
//            else
//            {
//                return ErrorResponse.ReturnErrorResponse(response.Errors?.ToString() ?? "");
//            }
//        }

//        private async Task<string> UploadFile(byte[] bytes, string fileName)
//        {
//            string uploadsFolder = Path.Combine("Images", fileName);
//            Stream stream = new MemoryStream(bytes);
//            using (var ms = new FileStream(uploadsFolder, FileMode.Create))
//            {
//                await stream.CopyToAsync(ms);
//            }
//            return uploadsFolder;
//        }

//        [HttpDelete("DeleteUser")]
//        public async Task<IActionResult> DeleteUser(Delete_User userDetails)
//        {

//            var existingUser = await _userManager.FindByEmailAsync(userDetails.Email);
//            if (existingUser != null)
//            {
//                var response = await _userManager.DeleteAsync(existingUser);

//                if (response.Succeeded)
//                {
//                    return Ok(new MainResponse
//                    {
//                        IsSuccess = true,
//                    });
//                }
//                else
//                {
//                    return ErrorResponse.ReturnErrorResponse(response.Errors?.ToString() ?? "");
//                }
//            }
//            else
//            {
//                return ErrorResponse.ReturnErrorResponse("No User found with this email");
//            }
//        }
//    }
//}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Context;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebAPIDemo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser(New_Kulanici userDetails)
        {
            if (ModelState.IsValid)
            {
                var userDetailToBeCreated = new Users
                {
                   
                    FirstName = userDetails.Name,
                    LastName = userDetails.Surname,
                    Email = userDetails.Email,
                    Password = userDetails.Password,
                };

                var response = await _userManager.CreateAsync(userDetailToBeCreated, userDetails.Password);

                if (response.Succeeded)
                {
                    return Ok("User Created Successfully.");
                }
                else
                {
                    return BadRequest(response.Errors);
                }
            }
            return BadRequest();

        }


        [AllowAnonymous]
        [HttpPost]
        [Route("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(AssignRole userDetails)
        {
            if (ModelState.IsValid)
            {

                var response = await _userManager.FindByNameAsync(userDetails.UserName);

                if (response != null)
                {

                    if (await _userManager.IsInRoleAsync(response, userDetails.RoleName))
                    {
                        return BadRequest("This Role Already Assigned To User");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(response, userDetails.RoleName);
                    }
                    return Ok("Role Assigned to User.");
                }
                else
                {
                    return BadRequest("Something went wrong");
                }
            }
            return BadRequest();

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRole rolerRequest)
        {
            if (ModelState.IsValid)
            {
                var response = await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = rolerRequest.Role,
                });

                if (response.Succeeded)
                {

                    return Ok("Role Created Successfully.");
                }
                else
                {
                    return BadRequest(response.Errors);
                }
            }
            return BadRequest();

        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate(Login_Kullanici loginDetail)
        {

            if (loginDetail is null) return Unauthorized();
            if (!ModelState.IsValid) return Unauthorized();

            var user = await _userManager.FindByNameAsync(loginDetail.Username);
            if (user == null) return Unauthorized();

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, loginDetail.Password);
            if (isValidPassword)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

                var role = await _userManager.GetRolesAsync(user);

                int? roleID = null;
                switch (role?.FirstOrDefault()?.ToLower())
                {
                    case "student":
                        roleID = (int)RoleDetails.Student;
                        break;
                    case "teacher":
                        roleID = (int)RoleDetails.Teacher;
                        break;
                    case "admin":
                        roleID = (int)RoleDetails.Admin;
                        break;
                }

                var useDetail = new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = role?.FirstOrDefault(),
                    RoleID = roleID
                };

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _configuration["JWT:Issuer"],
                    Audience = _configuration["JWT:Audience"],
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new LOgin_Response { Token = tokenHandler.WriteToken(token), UserDetail = useDetail });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.Select(f => new UserList
            {
                Email = f.Email,
                FirstName = f.FirstName,
                LastName = f.LastName,
                UserName = f.UserName
            }).ToList();
            return Ok(users);
        }
    }
}

