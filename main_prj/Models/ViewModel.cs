namespace main_prj.Models
{
    public class ViewModel
    {
        public Product? Product { get; set; }
        public Monitor? Monitor { get; set; }
        public Mouse? Mouse { get; set; }    
        public Keyboard? Keyboard { get; set; }
        public Headphone? Headphone { get; set; }
        public virtual ICollection<string> Images { get; set; } = new List<string>();
    }
}
