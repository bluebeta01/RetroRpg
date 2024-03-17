using System.Drawing;
using System.Drawing.Imaging;
using BigGustave;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace RetroRpg;

public class Texture : Asset
{
    public int TextureId { get; set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    public override void LoadFromDisk(string filePath)
    {
        try
        {
            TextureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            var bitmap = new Bitmap(filePath);
            var textureData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppPArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            Width = bitmap.Width;
            Height = bitmap.Height;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to load texture {filePath}. {e.Message}");
            TextureId = -1;
            GL.DeleteTexture(TextureId);
        }
    }
}