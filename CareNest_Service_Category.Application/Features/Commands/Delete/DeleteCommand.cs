using CareNest_Service_Category.Application.Interfaces.CQRS.Commands;

namespace CareNest_Service_Category.Application.Features.Commands.Delete
{
    public class DeleteCommand: ICommand
    {
        public required string Id { get; set; }
    }
}
