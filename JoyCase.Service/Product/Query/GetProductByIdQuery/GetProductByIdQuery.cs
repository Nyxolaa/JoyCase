using JoyCase.Application.Product.Dto;
using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Product.Query.GetProductByIdQuery
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public long Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IRepository<Data.Product> _productRepository;

        public GetProductByIdQueryHandler(IRepository<Data.Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.SelectOneAsync(
                filter: p => p.Id == request.Id,
                selector: p => new ProductDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    IsActive = p.IsActive,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl
                }
            );
        }
    }
}
