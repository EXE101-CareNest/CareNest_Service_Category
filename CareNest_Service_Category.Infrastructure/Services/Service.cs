using CareNest_Service_Category.Application.Common;
using CareNest_Service_Category.Application.Features.Queries.GetAllPaging;
using CareNest_Service_Category.Application.Interfaces.Services;
using CareNest_Service_Category.Domain.Commons.Base;
using CareNest_Service_Category.Domain.Commons.Constant;
using CareNest_Service_Category.Infrastructure.ApiEndpoints;
using CareNest_Service_Category.Application.Features.Queries.GetServicesByCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareNest_Service_Category.Infrastructure.Services
{
    public class Service: IService
    {
        private readonly IAPIService _apiService;

        public Service(IAPIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<ResponseResult<ServiceResponse>> GetServiceById(string? id)
        {
            var appointment = await _apiService.GetAsync<ServiceResponse>("service", ServiceEndpoint.GetById(id));
            if (!appointment.IsSuccess)
            {
                throw BaseException.BadRequestBadRequestResponse("Service Id : " + MessageConstant.NotFound);
            }
            return appointment;
        }

        public async Task<ResponseResult<List<ServiceByCategoryResponse>>> GetServicesByCategoryIds(List<string> categoryIds)
        {
            var result = await _apiService.PostAsync<List<ServiceByCategoryResponse>>(ServiceEndpoint.GetByCategories(), categoryIds);
            return result;
        }
    }
}
