using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace CardGame
{
    class ScoreBox
    {
        SpriteFont  m_SpriteFont;
        Sprite      m_ScoreBack;
        string      m_Text;
        Vector2     m_TextSize;

        public Vector2 Position;
        public string Text { get { return m_Text; } set { if (value != m_Text) { m_Text = value; m_TextSize = m_SpriteFont.MeasureString(m_Text); } } }

        public ScoreBox(ContentManager content, Vector2 position)
        {
            m_ScoreBack = new Sprite(content.Load<Texture2D>("Textures\\Buttons\\Score_Back"));
            m_SpriteFont = content.Load<SpriteFont>("Fonts\\GameFont");
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_ScoreBack.Draw(spriteBatch, Position);
            spriteBatch.DrawString(m_SpriteFont, m_Text, new Vector2(Position.X - (m_TextSize.X * 0.5f), Position.Y - (m_TextSize.Y * 0.5f) + 4), Color.White);
        }
    }
}
