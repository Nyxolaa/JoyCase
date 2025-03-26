using JoyCase.Application.Category.Dto;
using JoyCase.Application.Product.Dto;
using JoyCase.Application.Product.Query.GetProductsByCategoryQuery;
using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Category.Query.GetRecursiveCategoriesQuery
{
    public class GetRecursiveCategoriesQuery : IRequest<IEnumerable<CategoryDto>> { }
    public class GetRecursiveCategoriesQueryHandler : IRequestHandler<GetRecursiveCategoriesQuery, IEnumerable<CategoryDto>> 
    {
        private readonly IRepository<Data.Category> _categoryRepository;
        public GetRecursiveCategoriesQueryHandler(IRepository<Data.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetRecursiveCategoriesQuery request, CancellationToken cancellationToken)
        {
            var products = await _categoryRepository.ExecuteStoredProcedureAsync<CategoryDto>("dbo.GetRecursiveCategories");

            return products;
        }
    }
}
