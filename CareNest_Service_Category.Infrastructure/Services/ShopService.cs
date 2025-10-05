using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Interfaces.Services;
using CareNest_Service_Category.Domain.Commons.Base;
using CareNest_Service_Category.Domain.Commons.Constant;
using CareNest_Service_Category.Infrastructure.ApiEndpoints;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareNest_Service_Category.Infrastructure.Services
{
    public class ShopService : IShopService
    {
        private readonly IAPIService _apiService;

        public ShopService(IAPIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<ResponseResult<ShopResponse>> GetShopById(string? id)
        {
            var shop = await _apiService.GetAsync<ShopResponse>("shop", ShopEndpoint.GetById(id));
            if (!shop.IsSuccess)
            {
                throw BaseException.BadRequestBadRequestResponse("Shop Id : " + MessageConstant.NotFound);
            }
            return shop;
        }
    }
}
