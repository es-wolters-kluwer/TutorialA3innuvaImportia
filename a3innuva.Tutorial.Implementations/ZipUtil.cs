namespace a3innuva.Tutorial.Implementations
{
    using System.IO;
    using System.IO.Compression;
    using Interfaces;

    public class ZipUtil : IZipUtil
    {
        public void CreateZipFile(string path, string file)
        {
            string resultFile = $"{Path.GetFileNameWithoutExtension(file)}.zip";

            string tempPath = $"{path}\\zips";
            string fileName = file.Remove(0, file.LastIndexOf('\\') + 1);

            Directory.CreateDirectory(tempPath);

            File.Copy(file, $"{tempPath}/{fileName}", true);

            File.Delete($"{path}\\{resultFile}");

            ZipFile.CreateFromDirectory(tempPath, $"{path}\\{resultFile}");

            Directory.Delete(tempPath, true);
        }
    }
}
