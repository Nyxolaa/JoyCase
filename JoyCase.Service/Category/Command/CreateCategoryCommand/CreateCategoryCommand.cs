using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Category.Command.CreateCategoryCommand
{
    public class CreateCategoryCommand : IRequest<long>
    {
        public string Name { get; set; } = null!;
        public long? ParentId { get; set; }
        public string CreatedBy { get; set; } = null!;
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, long>
    {
        private readonly IRepository<Data.Category> _categoryRepository;
        public CreateCategoryCommandHandler(IRepository<Data.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<long> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Data.Category
            {
                Name = request.Name,
                ParentId = request.ParentId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return category.Id;
        }
    }
}
