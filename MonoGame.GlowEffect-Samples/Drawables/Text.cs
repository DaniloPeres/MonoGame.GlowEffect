using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.GlowEffect_Samples.Drawables;

public class Text : IDrawable
{
    public Action OnClick { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    SpriteFont spriteFont;
    string text;
    Vector2 position;
    Color color;

    public Text(SpriteFont spriteFont, string text, Vector2 position, Color color)
    {
        this.spriteFont = spriteFont;
        this.text = text;
        this.position = position;
        this.color = color;
    }

    public bool Collision(Vector2 pos)
    {
        return false;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(spriteFont, text, position, color);
    }

    public void SetText(string text)
    {
        this.text = text;
    }
}
