using SFB;

namespace Sever.Gridder
{
    public static class FileManager
    {
        public static string ChooseImageFromDisk()
        {
            var extensions = new[]
            {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
            };

            var filePaths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            if (filePaths == null || filePaths.Length == 0)
            {
                return null;
            }

            return filePaths[0];
        }
    }
}