using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Reflection;

namespace MonoGame
{
    public enum GraphicsApi
    {
        OPEN_GL,
        DIRECT_X
    }

    public static partial class GlowEffect
    {
        private static Effect glowEffectCache;
        private static string CurrentShaderExtension => GetShaderExtension() == GraphicsApi.OPEN_GL ? "OpenGL" : "DirectX";


        public static Texture2D CreateGlow(Texture2D src, Color color, float blurX, float blurY, float dist, float angle, float alpha, float strength, float inner, float knockout, float circleSamplingTimes, float linearSamplingTimes, GraphicsDevice graphics)
        {
            var effect = GetEffectGlow(graphics);

            var size = 50;

            // crate a render target with margins
            using (var renderTargetResize = new RenderTarget2D(graphics, src.Width + size * 2, src.Height + size * 2))
            {
                graphics.SetRenderTarget(renderTargetResize);
                graphics.Clear(Color.Transparent);

                using (SpriteBatch spriteBatch = new SpriteBatch(graphics))
                {
                    // Create a new texture with the new size
                    spriteBatch.Begin();

                    // draw the texture with the margin
                    spriteBatch.Draw(src, new Vector2(size), Color.White);

                    spriteBatch.End();
                }

                var renderTarget = new RenderTarget2D(graphics, renderTargetResize.Width, renderTargetResize.Height);

                // Apply my effect
                effect.Parameters["pixelSize"].SetValue(Vector2.One / new Vector2(renderTargetResize.Width, renderTargetResize.Height));
                effect.Parameters["blurX"].SetValue(blurX);
                effect.Parameters["blurY"].SetValue(blurY);
                effect.Parameters["dist"].SetValue(dist);
                effect.Parameters["angle"].SetValue(angle);
                effect.Parameters["alpha"].SetValue(alpha);
                effect.Parameters["strength"].SetValue(strength);
                effect.Parameters["inner"].SetValue(inner);
                effect.Parameters["knockout"].SetValue(knockout);
                effect.Parameters["circleSamplingTimes"].SetValue(circleSamplingTimes);
                effect.Parameters["linearSamplingTimes"].SetValue(linearSamplingTimes);
                effect.Parameters["color"].SetValue(color.ToVector4());

                // Draw the img with the effect
                graphics.SetRenderTarget(renderTarget);
                graphics.Clear(Color.Transparent);

                using (SpriteBatch spriteBatch = new SpriteBatch(graphics))
                {
                    //spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.WriteToAlpha, effect: effect);
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, effect);
                    spriteBatch.Draw(renderTargetResize, new Vector2(0, 0), Color.White);
                    spriteBatch.End();
                    graphics.SetRenderTarget(null);
                }

                return renderTarget;
            }
        }

        private static Effect GetEffectGlow(GraphicsDevice graphics)
        {
            if (glowEffectCache == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"MonoGame.GlowEffect.Content.{CurrentShaderExtension}.GlowEffect.xnb";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        var content = new XnbContentManager(ms, graphics);
                        glowEffectCache = content.Load<Effect>("MyMemoryStreamAsset");
                    }
                }
            }

            return glowEffectCache;
        }


        private static GraphicsApi GetShaderExtension()
        {
            // Use reflection to figure out if Shader.Profile is OpenGL (0) or DirectX (1).
            // May need to be changed / fixed for future shader profiles.
            const string SHADER_TYPE = "Microsoft.Xna.Framework.Graphics.Shader";
            const string PROFILE = "Profile";

            var assembly = typeof(Game).GetTypeInfo().Assembly;
            if (assembly == null)
                throw new Exception(
                    "Error determining shader profile. Couldn't find assembly. Falling back to OpenGL.");

            var shaderType = assembly.GetType(SHADER_TYPE);
            if (shaderType == null)
                throw new Exception(
                    $"Error determining shader profile. Couldn't find shader type of '{SHADER_TYPE}'. Falling back to OpenGL.");

            var shaderTypeInfo = shaderType.GetTypeInfo();
            if (shaderTypeInfo == null)
                throw new Exception(
                    "Error determining shader profile. Couldn't get TypeInfo of shadertype. Falling back to OpenGL.");

            // https://github.com/MonoGame/MonoGame/blob/develop/MonoGame.Framework/Graphics/Shader/Shader.cs#L47
            var profileProperty = shaderTypeInfo.GetDeclaredProperty(PROFILE);
            var value = (int)profileProperty.GetValue(null);

            switch (value)
            {
                case 0:
                    return GraphicsApi.OPEN_GL;
                case 1:
                    return GraphicsApi.DIRECT_X;
                default:
                    throw new Exception("Unknown shader profile.");
            }
        }
    }
}
