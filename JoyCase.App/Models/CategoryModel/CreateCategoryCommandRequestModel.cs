namespace JoyCase.App.Models.CategoryModel
{
    public class CreateCategoryCommandRequestModel
    {
        public string Name { get; set; } = null!;
        public long? ParentId { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
