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
        private readonly IService _service; 


        public CreateCommandHandler(IUnitOfWork unitOfWork, IService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<ServiceCategory> HandleAsync(CreateCommand command)
        {
            Validate.ValidateCreate(command);

            //kiểm tra service tồn tại
            var service = await _service.GetServiceById(command.Service_Id);

            ServiceCategory serviceCategory = new()
            {
                Name = command.Name,
                Service_Id = service.Data!.Data!.Id,
                CreatedAt = TimeHelper.GetUtcNow()
            };
            await _unitOfWork.GetRepository<ServiceCategory>().AddAsync(serviceCategory);
            await _unitOfWork.SaveAsync();

            return serviceCategory;
        }

    }
}
