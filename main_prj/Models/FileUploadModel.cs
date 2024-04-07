namespace main_prj.Models
{
    public class FileUploadModel
    {
        public IFormFile? SingleFile {  get; set; }
        public IEnumerable<IFormFile>? FileList { get; set; } = new List<IFormFile>();
    }
}
