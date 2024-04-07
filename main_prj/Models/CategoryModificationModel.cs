namespace main_prj.Models
{
    public class CategoryModificationModel
    {
        public Category Category { get; set; } = null!;
        public IFormFile? imageFile { get; set; }
    }
}
