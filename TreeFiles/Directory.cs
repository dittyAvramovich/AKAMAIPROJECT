namespace TreeFiles
{
    public class Directory
    {
        public string Name { get; set; }
        public List<string> Files { get; set; }
        public List<Directory> Directories { get; set; }
    }
}
