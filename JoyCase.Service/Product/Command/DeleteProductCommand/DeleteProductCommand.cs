using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Product.Command.DeleteProductCommand
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IRepository<Data.Product> _productRepository;

        public DeleteProductCommandHandler(IRepository<Data.Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) return false;

            await _productRepository.DeleteAsync(product.Id);
            await _productRepository.SaveChangesAsync();
            return true;
        }
    }
}
