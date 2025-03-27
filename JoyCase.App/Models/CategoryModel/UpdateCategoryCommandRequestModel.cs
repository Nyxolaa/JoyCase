namespace JoyCase.App.Models.CategoryModel
{
    public class UpdateCategoryCommandRequestModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long? ParentId { get; set; }
        public string UpdatedBy { get; set; } = null!;
    }
}
