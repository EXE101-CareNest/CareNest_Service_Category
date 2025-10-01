using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Features.Queries.GetAllPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareNest_Service_Category.Application.Interfaces.Services
{
    public interface IService
    {
        Task<ResponseResult<ServiceResponse>> GetServiceById(string? id);
    }
}
