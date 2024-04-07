namespace main_prj.Models
{
    public class ProductModificationModel
    {
        public Product Product { get; set; } = null!;
        public Headphone? Headphone { get; set; }
        public Monitor? Monitor { get; set; }
        public Mouse? Mouse { get; set; }
        public Keyboard? Keyboard { get; set; }
        public IEnumerable<Specification> Specifications { get; set; } = new List<Specification>();
        public IFormFile? PreviewImage { get; set; }
        public IEnumerable<IFormFile>? DetailImages { get; set; } = new List<IFormFile>();
    }
}
