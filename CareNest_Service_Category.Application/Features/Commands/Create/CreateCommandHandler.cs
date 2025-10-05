using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Exceptions.Validators;
using CareNest_Service_Category.Application.Interfaces.CQRS.Commands;
using CareNest_Service_Category.Application.Interfaces.Services;
using CareNest_Service_Category.Application.Interfaces.UOW;
using Shared.Helper;

namespace CareNest_Service_Category.Application.Features.Commands.Create
{
    public class CreateCommandHandler : ICommandHandler<CreateCommand, ServiceCategory>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShopService _service; 


        public CreateCommandHandler(IUnitOfWork unitOfWork, IShopService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<ServiceCategory> HandleAsync(CreateCommand command)
        {
            //Validate.ValidateCreate(command);

            //kiểm tra shop tồn tại
            var shop = await _service.GetShopById(command.ShopId);

            ServiceCategory serviceCategory = new()
            {
                Name = command.Name,
                ShopId = shop.Data!.Data!.Id,
                CreatedAt = TimeHelper.GetUtcNow()
            };
            await _unitOfWork.GetRepository<ServiceCategory>().AddAsync(serviceCategory);
            await _unitOfWork.SaveAsync();

            return serviceCategory;
        }

    }
}
