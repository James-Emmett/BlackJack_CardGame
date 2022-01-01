using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    class Background
    {
        Texture2D m_Background;
        Rectangle m_Source;

        public Background(Texture2D texture, Rectangle? source = null)
        {
            m_Background = texture;

            if (source == null)
            {
                m_Source = new Rectangle(0, 0, texture.Width, texture.Height);
            }
            else
            {
                m_Source = (Rectangle)source;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_Background, Vector2.Zero, m_Source, Color.White);
        }
    }
}
