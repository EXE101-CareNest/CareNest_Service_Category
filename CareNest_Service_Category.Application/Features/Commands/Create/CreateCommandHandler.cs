using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Exceptions.Validators;
using CareNest_Service_Category.Application.Interfaces.CQRS.Commands;
using CareNest_Service_Category.Application.Interfaces.UOW;
using Shared.Helper;

namespace CareNest_Service_Category.Application.Features.Commands.Create
{
    public class CreateCommandHandler : ICommandHandler<CreateCommand, ServiceCategory>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceCategory> HandleAsync(CreateCommand command)
        {
            Validate.ValidateCreate(command);

            ServiceCategory service = new()
            {
                Name = command.Name,   
                Service_Id = command.Service_Id,
                CreatedAt = TimeHelper.GetUtcNow()
            };
            await _unitOfWork.GetRepository<ServiceCategory>().AddAsync(service);
            await _unitOfWork.SaveAsync();

            return service;
        }

    }
}
