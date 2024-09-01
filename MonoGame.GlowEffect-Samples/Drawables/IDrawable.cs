using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.GlowEffect_Samples.Drawables;

public interface IDrawable : IDisposable
{
    void Draw(SpriteBatch spriteBatch);
    bool Collision(Vector2 pos);
    Action OnClick { get; set; }
}
