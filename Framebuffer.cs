using OpenTK.Graphics.OpenGL;

namespace RetroRpg;

public class Framebuffer
{
    public int GlId { get; private set; } = -1;
    public int Width { get; private set; }
    public int Height { get; private set; }
    private int _colorTextureAttachment = -1;
    private int _depthTextureAttachment = -1;

    public Framebuffer(int width, int height)
    {
        Width = width;
        Height = height;
        GenerateFramebuffer();
    }

    public void Resize(int width, int height)
    {
        Width = width;
        Height = height;
        DeleteFramebuffer();
        GenerateFramebuffer();
    }

    private void DeleteFramebuffer()
    {
        GL.DeleteTexture(_colorTextureAttachment);
        GL.DeleteTexture(_depthTextureAttachment);
        GL.DeleteFramebuffer(GlId);
    }

    private void GenerateFramebuffer()
    {
        GlId = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, GlId);
        
        _colorTextureAttachment = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _colorTextureAttachment);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, 0);
        
        _depthTextureAttachment = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _depthTextureAttachment);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, Width, Height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, 0);
        
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _colorTextureAttachment, 0);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, _depthTextureAttachment, 0);
        
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
}