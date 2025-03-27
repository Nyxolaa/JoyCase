using JoyCase.Data;
using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Product.Command.UpdateProductCommand
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
        public string UpdatedBy { get; set; } = null!;
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IRepository<Data.Product> _productRepository;

        public UpdateProductCommandHandler(IRepository<Data.Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) return false;

            product.Name = request.Name;
            product.CategoryId = request.CategoryId;
            product.ImageUrl = request.ImageUrl;
            product.Price = request.Price;
            product.IsActive = request.IsActive;
            product.Description = request.Description;
            product.UpdatedBy = request.UpdatedBy;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }
    }
}
