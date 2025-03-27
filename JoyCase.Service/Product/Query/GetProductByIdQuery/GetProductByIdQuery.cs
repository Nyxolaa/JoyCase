using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Product.Query.GetProductByIdQuery
{
    public class GetProductByIdQuery : IRequest<Data.Product>
    {
        public long Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Data.Product>
    {
        private readonly IRepository<Data.Product> _productRepository;

        public GetProductByIdQueryHandler(IRepository<Data.Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Data.Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetByIdAsync(request.Id);
        }
    }
}
