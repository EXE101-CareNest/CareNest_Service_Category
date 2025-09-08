using Shared.Helper;
using CareNest_Service_Category.Application.Exceptions;
using CareNest_Service_Category.Domain.Commons.Constant;
using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Exceptions.Validators;
using CareNest_Service_Category.Application.Interfaces.CQRS.Commands;
using CareNest_Service_Category.Application.Interfaces.UOW;

namespace CareNest_Service_Category.Application.Features.Commands.Update
{
    public class UpdateCommandHandler: ICommandHandler<UpdateCommand, ServiceCategory>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceCategory> HandleAsync(UpdateCommand command)
        {
            // Gọi validator để kiểm tra dữ liệu
            Validate.ValidateUpdate(command);

            // Tìm để cập nhật
            ServiceCategory? service = await _unitOfWork.GetRepository<ServiceCategory>().GetByIdAsync(command.Id)
               ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);

            if (command.Name != null) service.Name = command.Name;
            if (command.Service_Id != null) service.Service_Id = command.Service_Id;

            service.UpdatedAt = TimeHelper.GetUtcNow();

            _unitOfWork.GetRepository<ServiceCategory>().Update(service);
            await _unitOfWork.SaveAsync();
            return service;

        }
    }
}
