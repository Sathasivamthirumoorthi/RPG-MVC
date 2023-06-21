using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace web.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        // IConfiguration for JWT to get secret key
        public AuthRepository(DataContext context , IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string userename, string password)
        {
            var serviceResponse = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Username.ToLower().Equals(userename.ToLower()));
            if(user is null){
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found";
            }
            else if(!verifyPasswordHash(password, user.PasswordHash , user.PasswordSalt )){
                serviceResponse.Success = false;
                serviceResponse.Message = "Wrong password"; 
            }
            else{
                serviceResponse.Data = CreateToken(user);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {   
            ServiceResponse<int> serviceResponse = new ServiceResponse<int>();
            if(await UserExists(user.Username)){
                serviceResponse.Success = false;
                serviceResponse.Message = "User already exist";
                return serviceResponse;
            }
            CreatePasswordHash(password , out byte[] passwordHash , out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            serviceResponse.Data = user.Id;
            serviceResponse.Success = true;

            return serviceResponse;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower())){
                return true;
            }
            return false;
        }


        // to hash a password
        private void CreatePasswordHash(string password , out byte[] PasswordHash , out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()){
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool verifyPasswordHash(string password , byte[] passwordHash , byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user){
            
            //  list of claims representing the user's identity and additional information. In this example, two claims are added: NameIdentifier

            List<Claim> cliams = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Name , user.Username)
            };
            // get secret key from appsettings
            // retrieves the secret key used for signing the JWT from the application settings. It converts the key from a string to a byte array and creates a SymmetricSecurityKey instance.
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding
            .UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            // SigningCredentials using the previously created symmetric security key and specifies the signing algorithm (HmacSha512Signature in this case).
            SigningCredentials credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            
            // SecurityTokenDescriptor that represents the details of the JWT to be generated. It sets the subject (claims identity), expiration date (1 day from the current time), and signing credentials.

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(cliams),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            // instance of JwtSecurityTokenHandler, which is responsible for creating and manipulating JWTs.
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            // calls the CreateToken method of the JwtSecurityTokenHandler, passing the security token descriptor. This generates a JWT based on the provided information.

            SecurityToken token = tokenHandler.CreateToken(descriptor); 

            // JwtSecurityTokenHandler to write the generated token as a string using the WriteToken 
            return tokenHandler.WriteToken(token); // Token
        }

    }

}