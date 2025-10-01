using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Exceptions;
using CareNest_Service_Category.Application.Interfaces.CQRS.Commands;
using CareNest_Service_Category.Application.Interfaces.UOW;
using CareNest_Service_Category.Domain.Commons.Constant;

namespace CareNest_Service_Category.Application.Features.Commands.Delete
{
    public class DeleteCommandHandler : ICommandHandler<DeleteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DeleteCommand command)
        {
            // Lấy shop theo ID
            ServiceCategory? shop = await _unitOfWork.GetRepository<ServiceCategory>().GetByIdAsync(command.Id)
                                              ?? throw new BadRequestException("Shop Id: " + MessageConstant.NotFound);

            _unitOfWork.GetRepository<ServiceCategory>().Delete(shop);

            await _unitOfWork.SaveAsync();

        }
    }
}
