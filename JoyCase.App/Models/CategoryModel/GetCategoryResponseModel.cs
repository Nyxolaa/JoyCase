namespace JoyCase.App.Models.CategoryModel
{
    public class GetCategoryResponseModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty; // NULL gelirse hata olmamasi icin
        public long? ParentId { get; set; }
        public string FullPath { get; set; } = string.Empty; // NULL gelirse hata olmaması icin
    }

}
