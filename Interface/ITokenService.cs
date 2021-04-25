using Assessment.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.Interface
{
    public interface ITokenService
    {
        Task<ApiResponse> Authenticate();
        Task<ApiResponse> SubscriberService(string token);
        Task<ApiResponse> CategoriesService(string token);
        Task<ApiResponse> GetMagazineService(string token, string category);

    }
}
