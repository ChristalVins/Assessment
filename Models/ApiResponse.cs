using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public ICollection<ApiSubscriber>? ApiSubscriber { get; set; }
        public ICollection<CategoriesModel>? CategoriesModel { get; set; }
        public ICollection<MagazineModel>? MagazineModel { get; set; }

        public ApiResponse(string token, bool success)
        {
            Success = success;
            Token = token;
        }

    }
}
