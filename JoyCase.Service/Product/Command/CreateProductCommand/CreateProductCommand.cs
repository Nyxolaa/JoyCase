using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Product.Command.CreateProductCommand
{
    public class CreateProductCommand : IRequest<long>
    {
        public string Name { get; set; } = null!;
        public long CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; } = null!;
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, long>
    {
        private readonly IRepository<Data.Product> _productRepository;

        public CreateProductCommandHandler(IRepository<Data.Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Data.Product
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                ImageUrl = request.ImageUrl,
                Price = request.Price,
                IsActive = request.IsActive,
                Description = request.Description,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            return product.Id;
        }
    }

}
