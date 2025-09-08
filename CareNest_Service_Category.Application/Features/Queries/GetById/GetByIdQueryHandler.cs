using CareNest_Service_Category.Domain.Commons.Constant;
using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;
using CareNest_Service_Category.Application.Interfaces.UOW;

namespace CareNest_Service_Category.Application.Features.Queries.GetById
{
    public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, ServiceCategory>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceCategory> HandleAsync(GetByIdQuery query)
        {
            ServiceCategory? service = await _unitOfWork.GetRepository<ServiceCategory>().GetByIdAsync(query.Id);

            if (service == null)
            {
                throw new Exception(MessageConstant.NotFound);
            }
            return service;
        }
    }
}
