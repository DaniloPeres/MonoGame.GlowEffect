using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Reflection;

namespace MonoGame;

public static partial class GlowEffect
{
    private static Effect glowEffectCache;
    private static ContentManager contentManager;
    private static GraphicsDevice graphics;

    public static void InitializeAndLoad(ContentManager contentManager, GraphicsDevice graphics)
    {
        GlowEffect.contentManager = contentManager;
        GlowEffect.graphics = graphics;
        glowEffectCache = GetEffectGlow();
    }

    public static Texture2D CreateGlow(Texture2D src, Color color, int glowWidth, float intensity, float spread, float totalGlowMultiplier, bool hideTexture = false)
    {
        if (glowEffectCache == null)
            throw new System.Exception("GlowEffect not initialized. Please call the InitializeAndLoad method before using this function.");

        lock (graphics)
        {
            var size = 50;

            // crate a render target with margins
            using var renderTargetResize = new RenderTarget2D(graphics, src.Width + size * 2, src.Height + size * 2);
            graphics.SetRenderTarget(renderTargetResize);
            graphics.Clear(Color.Transparent);

            using (SpriteBatch spriteBatch = new(graphics))
            {
                // Create a new texture with the new size
                spriteBatch.Begin();

                // draw the texture with the margin
                spriteBatch.Draw(src, new Vector2(size), Color.White);

                spriteBatch.End();
            }

            var renderTarget = new RenderTarget2D(graphics, renderTargetResize.Width, renderTargetResize.Height);

            // Apply my effect
            glowEffectCache.Parameters["textureSize"].SetValue(new Vector2(renderTargetResize.Width, renderTargetResize.Height));
            glowEffectCache.Parameters["glowWidth"].SetValue(glowWidth);
            glowEffectCache.Parameters["intensity"].SetValue(intensity);
            glowEffectCache.Parameters["spread"].SetValue(spread);
            glowEffectCache.Parameters["totalGlowMultiplier"].SetValue(totalGlowMultiplier);
            glowEffectCache.Parameters["glowColor"].SetValue(color.ToVector4());
            glowEffectCache.Parameters["hideTexture"].SetValue(hideTexture);

            // Draw the img with the effect
            graphics.SetRenderTarget(renderTarget);
            graphics.Clear(Color.Transparent);

            using (SpriteBatch spriteBatch = new(graphics))
            {
                //spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.WriteToAlpha, effect: effect);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, glowEffectCache);
                spriteBatch.Draw(renderTargetResize, new Vector2(0, 0), Color.White);
                spriteBatch.End();
                graphics.SetRenderTarget(null);
            }

            return renderTarget;
        }
    }

    private static Effect GetEffectGlow()
    {
        if (glowEffectCache == null)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"MonoGame.GlowEffect.Content.bin.DesktopGL.Content.GlowEffect.xnb";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var content = new XnbContentManager(ms, graphics);
            glowEffectCache = content.Load<Effect>();
        }

        return glowEffectCache;
    }
}
