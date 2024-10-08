﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame
{
    public static partial class GlowEffect
    {
        public static Texture2D transparentPixel;

        public static Texture2D CreateGlowSpriteFont(SpriteFont spriteFont, string text, Color textColor, Vector2 scale, Color glowColor, int glowWidth, float intensity, float spread, float totalGlowMultiplier, bool hideTexture = false)
        {
            var textTexture2D = DrawSpriteFontToTexture2D(spriteFont, text, textColor, scale);
            return CreateGlow(textTexture2D, glowColor, glowWidth, intensity, spread, totalGlowMultiplier, hideTexture);
        }

        private static Texture2D DrawSpriteFontToTexture2D(SpriteFont spriteFont, string text, Color textColor, Vector2 scale)
        {
            var textSize = spriteFont.MeasureString(text) * scale;
            if (textSize.X == 0 || textSize.Y == 0)
                return GetTransparentPixel();

            lock (graphics)
            {
                var target = new RenderTarget2D(graphics, (int)textSize.X, (int)textSize.Y);
                using (var spriteBatch = new SpriteBatch(graphics))
                {
                    graphics.SetRenderTarget(target);// Now the spriteBatch will render to the RenderTarget2D
                    graphics.Clear(Color.Transparent);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    spriteBatch.DrawString(spriteFont, text, Vector2.Zero, textColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 1);
                    spriteBatch.End();
                    graphics.SetRenderTarget(null);//This will set the spriteBatch to render to the screen again.
                    return target;
                }
            }
        }

        private static Texture2D GetTransparentPixel()
        {
            if (transparentPixel == null)
            {
                transparentPixel = new Texture2D(graphics, 1, 1);
                var color = new Color[1];
                color[0] = Color.Transparent;
                transparentPixel.SetData(color);
            }

            return transparentPixel;
        }
    }
}
