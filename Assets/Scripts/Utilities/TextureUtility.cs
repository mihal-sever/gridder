using System;
using ExifLib;
using SFB;
using UnityEngine;

namespace Sever.Gridder
{
    public static class TextureUtility
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


        // orientation values:
        // 1 no nothing
        // 2 flip horizontally
        // 3 rotate 180 degrees
        // 4 flip vertically
        // 5 rotate 90 degrees clockwise and flip horizontally
        // 6 rotate 90 degrees clockwise
        // 7 rotate 90 degrees clockwise and flip vertically
        // 8 rotate 270 degrees clockwise
        public static Texture2D Rotate(this Texture2D texture, string path)
        {
            ushort orientation = GetImageOrientation(path);
            switch (orientation)
            {
                case 0:
                case 1:
                case 4:
                    return texture;

                case 2:
                case 3:
                    return texture.Rotate(true).Rotate(true);
                
                case 6:
                case 7:
                    return texture.Rotate(true);

                case 5:
                case 8:
                    return texture.Rotate(false);
                
                default:
                    return texture;
            }
        }


        private static ushort GetImageOrientation(string path)
        {
            try
            {
                using ExifReader reader = new ExifReader(path);
                return reader.GetTagValue(ExifTags.Orientation, out ushort orientation) ? orientation : (ushort) 0;
            }
            catch (ExifLibException e)
            {
                return 0;
            }
        }

        private static Texture2D Rotate(this Texture2D originalTexture, bool isClockwise)
        {
            Color32[] original = originalTexture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
            int w = originalTexture.width;
            int h = originalTexture.height;
            
            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; ++i)
                {
                    var iRotated = (i + 1) * h - j - 1;
                    var iOriginal = isClockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                    rotated[iRotated] = original[iOriginal];
                }
            }

            Texture2D rotatedTexture = new Texture2D(h, w);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            return rotatedTexture;
        }
    }
}