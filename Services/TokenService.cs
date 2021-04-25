using Assessment.Helpers;
using Assessment.Interface;
using Assessment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Services
{
    public class TokenService : ITokenService
    {
        private List<ApiSubscriber> _apiSubscriber = new List<ApiSubscriber>
        {
            new ApiSubscriber { Id = "582bbd55-998d-4a8d-a452-02cc633f7915", FirstName = "Mary", LastName = "White"},
            new ApiSubscriber { Id = "2e7a0f1b-dcd9-4139-aa93-c8279f6110ee", FirstName = "Destiny", LastName = "MacDonald"},
            new ApiSubscriber { Id = "6ac8e021-52b7-4c6f-9731-f49892d82bd8", FirstName = "Jack", LastName = "Mitchell"},
            new ApiSubscriber { Id = "237f2b7d-e055-4438-894e-54edfadedc8d", FirstName = "Lucas", LastName = "Robinson"},
            new ApiSubscriber { Id = "cca3b871-2066-49df-a000-bf9abe30dabf", FirstName = "Chloe", LastName = "Gonzalez"},
            new ApiSubscriber { Id = "9904fb68-d631-4e44-a7f8-f3ad3a0714d7", FirstName = "Mary", LastName = "MacDonald"}

        };

        private List<MagazineModel> _apiMagazineModel = new List<MagazineModel>
        {
            new MagazineModel { Id =1, Name = "Fortune", Category = "News"},
            new MagazineModel { Id =2, Name = "Animals", Category = "Animals"},
            new MagazineModel { Id =3, Name = "Scientific American", Category = "Science"},
            new MagazineModel { Id =4, Name = "Th", Category = "News"},
            new MagazineModel { Id =5, Name = "Dogster", Category = "Animals"},
            new MagazineModel { Id =6, Name = "The Economist", Category = "News"},
            new MagazineModel { Id =7, Name = "Time", Category = "News"},
            new MagazineModel { Id =8, Name = "Astronomy", Category = "Science"},
            new MagazineModel { Id =9, Name = "Discover", Category = "Science"},

        };

        private readonly AppSettings _appSettings;
        public TokenService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<ApiResponse> Authenticate()
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "Vins")
                }),
                Expires = DateTime.UtcNow.AddYears(2),
                Issuer = "MyWebsite.com",
                Audience = "MyWebsite.com",
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var access_token = tokenHandler.WriteToken(token);
            bool success = false;
            if(access_token != null)
            {
                success = true;
            }
            return new ApiResponse (access_token, success);
        }

        public async Task<ApiResponse> SubscriberService(string token)
        {
            //int tokenCheck;
            bool success = false;
            string? tokenCheck = ValidateJwtToken(token);
            var subscriberList = _apiSubscriber.ToList();
            if (tokenCheck != "" && tokenCheck != null)
            {
                success = true;
                ApiResponse apiResponse = new ApiResponse(token, success);
                apiResponse.ApiSubscriber = subscriberList;
                return apiResponse;
            }
            else
            {
                return null;
            }            
        }

        public async Task<ApiResponse> CategoriesService(string token)
        {
            CategoriesModel categories = new();
            bool success = false;
            string? tokenCheck = ValidateJwtToken(token);
            var magazineList = _apiMagazineModel.Distinct().ToList();
            if (tokenCheck != "" && tokenCheck != null)
            {
                success = true;
                ApiResponse apiResponse = new ApiResponse(token, success);
                //foreach (var item in magazineList)
                //{
                //    categories(new CategoriesModel(item));
                //}
                //apiResponse.CategoriesModel.(magazineList);
                apiResponse.MagazineModel = magazineList;
                return apiResponse;
            }
            else
            {
                return null;
            }
        }

        public async Task<ApiResponse> GetMagazineService(string token, string category)
        {
            if(token != null && category != null)
            {
                bool success = false;
                string? tokenCheck = ValidateJwtToken(token);
                var magazineList = _apiMagazineModel.Where(x=>x.Category == category).ToList();
                if (tokenCheck != "" && tokenCheck != null)
                {
                    success = true;
                    ApiResponse apiResponse = new ApiResponse(token, success);
                    apiResponse.MagazineModel = (ICollection<MagazineModel>)magazineList;
                    return apiResponse;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private string generateJwtToken()
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string? ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = jwtToken.Claims.First(x => x.Value != null).Value;

                // return account id from JWT token if validation successful
                return accountId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
