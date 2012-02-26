namespace Oleg.Kleyman.Core
{
    public class Output
    {
        public Output(){}
        public Output(string fileName, string targetDirectory)
        {
            FileName = fileName;
            TargetDirectory = targetDirectory;
        }

        public string FileName { get; set; }
        public string TargetDirectory { get; set; }
    }
}