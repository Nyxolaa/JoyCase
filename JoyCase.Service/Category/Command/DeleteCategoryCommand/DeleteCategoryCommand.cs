using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.Category.Command.DeleteCategoryCommand
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IRepository<Data.Category> _categoryRepository;

        public DeleteCategoryCommandHandler(IRepository<Data.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null) return false;

            await _categoryRepository.DeleteAsync(category.Id);
            await _categoryRepository.SaveChangesAsync();
            return true;
        }
    }

}
