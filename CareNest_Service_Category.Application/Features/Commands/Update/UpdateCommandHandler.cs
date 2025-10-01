using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Exceptions;
using CareNest_Service_Category.Application.Exceptions.Validators;
using CareNest_Service_Category.Application.Interfaces.CQRS.Commands;
using CareNest_Service_Category.Application.Interfaces.Services;
using CareNest_Service_Category.Application.Interfaces.UOW;
using CareNest_Service_Category.Domain.Commons.Constant;
using Shared.Helper;

namespace CareNest_Service_Category.Application.Features.Commands.Update
{
    public class UpdateCommandHandler : ICommandHandler<UpdateCommand, ServiceCategory>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;

        public UpdateCommandHandler(IUnitOfWork unitOfWork, IService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<ServiceCategory> HandleAsync(UpdateCommand command)
        {
            // Gọi validator để kiểm tra dữ liệu
            Validate.ValidateUpdate(command);

            // Tìm để cập nhật
            ServiceCategory? serviceCategory = await _unitOfWork.GetRepository<ServiceCategory>().GetByIdAsync(command.Id)
               ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);

            if (command.Name != null) serviceCategory.Name = command.Name;
            if (command.Service_Id != null)
            {
                //kiểm tra service tồn tại
                var service = await _service.GetServiceById(command.Service_Id);
                serviceCategory.Service_Id = service.Data!.Data!.Id;

            }

            serviceCategory.UpdatedAt = TimeHelper.GetUtcNow();

            _unitOfWork.GetRepository<ServiceCategory>().Update(serviceCategory);
            await _unitOfWork.SaveAsync();
            return serviceCategory;

        }
    }
}
