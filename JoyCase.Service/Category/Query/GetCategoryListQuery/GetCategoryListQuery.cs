using JoyCase.Application.Category.Dto;
using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Category.Query.GetCategoryListQuery
{
    public class GetCategoryListQuery : IRequest<List<CategoryDto>>
    {
    }

    public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, List<CategoryDto>>
    {
        private readonly IRepository<Data.Category> _categoryRepository;

        public GetCategoryListQueryHandler(IRepository<Data.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.SelectAsync(
                filter: c => true, // tum kategorileri getir
                selector: c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentId = c.ParentId
                }
            );
        }
    }
}
