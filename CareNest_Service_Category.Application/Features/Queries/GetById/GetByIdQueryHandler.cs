using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Exceptions;
using CareNest_Service_Category.Application.Features.Queries.GetAllPaging;
using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;
using CareNest_Service_Category.Application.Interfaces.Services;
using CareNest_Service_Category.Application.Interfaces.UOW;
using CareNest_Service_Category.Domain.Commons.Constant;

namespace CareNest_Service_Category.Application.Features.Queries.GetById
{
    public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, ServiceResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShopService _service;

        public GetByIdQueryHandler(IUnitOfWork unitOfWork, IShopService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<ServiceResponse> HandleAsync(GetByIdQuery query)
        {
            ServiceCategory? service = await _unitOfWork.GetRepository<ServiceCategory>().GetByIdAsync(query.Id);

            if (service == null)
            {
                throw new BadRequestException("Service Category Id: " + MessageConstant.NotFound);
            }
            //kiểm tra shop tồn tại
            var shop = await _service.GetShopById(service.ShopId);
            return new ServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                ShopId = service.ShopId,
                ShopName = shop.Data?.Data?.Name,
            };
        }
    }
}
