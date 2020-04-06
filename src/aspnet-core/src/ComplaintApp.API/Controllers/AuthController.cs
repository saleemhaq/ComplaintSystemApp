using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ComplaintApp.Application.Dtos;
using ComplaintApp.Core.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using ComplaintApp.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace ComplaintApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ComplaintDbContext _context;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, ComplaintDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
        }

        #endregion

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserForLoginDto userForLoginDto)
        {
            // We are using the option to find the user by name.
            // There are other options that can be used like find by Id or find by Email
            // The find by Id will not really be suitable in our case because we are
            // specifying that the user Id is of type <int> and not <string>
            var user = await _userManager.FindByNameAsync(userForLoginDto.Username);

            // We use the sign in manager to check for the password
            var result = await _signInManager.CheckPasswordSignInAsync(user,
                userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                // We are including the Photos in the user object
                var appUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == userForLoginDto.Username.ToUpper());

                // We will be using this 'user' object to be return back as an
                // anonymous object to the calling domain.  This is useful like having 
                // a user photo beside the user name
                // in the navigation bar once the user has successfully log-in
                // We are setting the mapping profile for the UserForListDto
                // in the AutoMapperProfiles class so that the main photo url
                // will automatically be mapped.
                
                var userToReturn = appUser;/*_mapper.Map<UserForListDto>(appUser);*/

                return Ok(new
                {
                    /**
                        If we don't add the .Result method, then the below is what we
                        can get under the browser dev tools -> network -> select 'login' 
                        -> preview

                        token: {,…}
                        asyncState: null
                        creationOptions: 0
                        exception: null
                        id: 2
                        isCanceled: false
                        isCompleted: true
                        isCompletedSuccessfully: true
                        isFaulted: false
                        result: "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwidW5pcXVlX25hbWUiOiJTdXNhbiIsInJvbGUiOlsiTWVtYmVyIiwiTW9kZXJhdG9yIl0sIm5iZiI6MTU0NDMzNjczOCwiZXhwIjoxNTQ0NDIzMTM4LCJpYXQiOjE1NDQzMzY3Mzh9.tik1k_OlVJbFYWMKAPnefERO8ciS4nIUiBXvsJPVVOelBilfnrxOaAguuUh807ltdg37vxrGuvfi8NHhfY8_OQ"
                        status: 5

                        We need to add the .Result to get the token only
                     **/
                    token = GenerateJwtToken(appUser).Result,
                    user = userToReturn
                });
            }

            // Return a general unauthorized instead if saying the username is correct
            // but the password is wrong so to avoid bruteforcing the password
            return Unauthorized();
        }

        [HttpPost("registerComplaint")]
        public async Task<IActionResult> RegisterComplaint(ComplaintDto complaintDtoInput)
        {
            return Ok();
        }

        #region Utilities

        private async Task<string> GenerateJwtToken(User user)
        {
            // Build-up a token that we are going to return to the user.
            // Our token will contain two bits of information about the user -- Id and UserName
            // We can have additional information to this token since this token can be validated by the server
            // without making a database call.  Once the server gets the token, it take a look inside it
            // and it does not need to go to the database to get the username or user Id
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // We will be getting the roles for this particular user
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // We also need a key to sign our token and this part is going to be hashed and it is not readable
            // inside our token itself.
            
            
            //var key = new SymmetricSecurityKey(Encoding.UTF8
            //    .GetBytes(_config.GetSection("SecuritySettings:Token").Value));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("random-created-characters-by-judyll-XSkdafREE92983*^&2ddf(234k)*#@s3sd_="));

            // We also need to generate some signing credentials based on the generated key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // We also need to create a security token descriptor which will contain our claims,
            // our expiry date for our tokens and the signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // We also need a token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // We can verify the generated token in
            // JWT.IO allows you to decode, verify and generate JWT. - https://jwt.io/
            return tokenHandler.WriteToken(token);
        }


        #endregion

        
    }
}
