using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Category.Command.UpdateCategoryCommand
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long? ParentId { get; set; }
        public string UpdatedBy { get; set; } = null!;
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly IRepository<Data.Category> _categoryRepository;

        public UpdateCategoryCommandHandler(IRepository<Data.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null) return false;

            category.Name = request.Name;
            category.ParentId = request.ParentId;
            category.UpdatedBy = request.UpdatedBy;
            category.UpdatedAt = DateTime.UtcNow;

            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return true;
        }
    }

}
