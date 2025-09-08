using CareNest.Domain.Entitites;
using CareNest_Service_Category.Application.Interfaces.CQRS.Queries;

namespace CareNest_Service_Category.Application.Features.Queries.GetById
{
    public class GetByIdQuery : IQuery<ServiceCategory>
    {
        public required string Id { get; set; }
    }
}
