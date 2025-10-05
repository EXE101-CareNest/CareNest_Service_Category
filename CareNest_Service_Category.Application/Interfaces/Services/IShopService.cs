using CareNest_Service_Category.Application.Common;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareNest_Service_Category.Application.Interfaces.Services
{
    public interface IShopService
    {
        Task<ResponseResult<ShopResponse>> GetShopById(string? id);
    }
}
