using JoyCase.Application.Product.Dto;
using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Product.Query.GetProductsByCategoryQuery
{
    public class GetProductsByCategoryQuery : IRequest<IEnumerable<ProductDto>> { }

    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, IEnumerable<ProductDto>>
    {
        private readonly IRepository<Data.Product> _productRepository;
        public GetProductsByCategoryQueryHandler(IRepository<Data.Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.ExecuteStoredProcedureAsync<ProductDto>("GetProductsByCategory");

            return products;
        }
    }
}
