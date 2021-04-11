using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.GlowEffect_Samples.Drawables
{
    public interface IDrawable : IDisposable
    {
        void Draw(SpriteBatch spriteBatch);
        bool Collision(Vector2 pos);
        Action OnClick { get; set; }
    }
}
