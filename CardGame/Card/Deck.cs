using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    class Deck
    {
        Texture2D   m_DeckTexture;
        Rectangle[] m_Frames;
        Sprite      m_DeckBack;
        Card[]      m_Deck;
        Vector2     Position;
        int         m_Top = 0;
        int         m_Frame;

        public void LoadContent(ContentManager content)
        {
            m_DeckTexture = content.Load<Texture2D>("Textures\\Cards\\Deck");
            m_Frames = new Rectangle[4] { new Rectangle(0, 0, 154, 200), new Rectangle(155, 0, 150, 200), new Rectangle(308, 4, 145, 196), new Rectangle(456, 8, 142, 192) };
            m_Deck = new Card[52];
            m_DeckBack = new Sprite(content.Load<Texture2D>("Textures\\Cards\\Deck_Back"));

            Texture2D cardAtlas = content.Load<Texture2D>("Textures\\Cards\\Cards");
            Texture2D cardBack  = content.Load<Texture2D>("Textures\\Cards\\CardBack");
            for (int i = 0; i < 52; ++i)
            {
                m_Deck[i] = new Card(i, cardAtlas, cardBack);
            }

            m_Top = 0;
            Position = new Vector2(1680, 275);
        }

        public void Reset()
        {
            m_Top = 0;
            Shuffle();
        }

        public Card TakeCard()
        {
            if (m_Top >= 52) { m_Top = 0; }
            return m_Deck[m_Top++];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_DeckBack.Draw(spriteBatch, new Vector2(1750, 175));
            m_Frame = (int)(((float)m_Top / m_Deck.Length) * (m_Frames.Length - 1));
            spriteBatch.Draw(m_DeckTexture, Position, m_Frames[m_Frame], Color.White, 0, new Vector2(0, m_Frames[m_Frame].Height), 1, SpriteEffects.None, 0);
        }

        private void Shuffle(int range = 51)
        {
            for (int i = 0; i < range; ++i)
            {
                int randomPosition = Engine.Random.Next(i, range);
                Card value = m_Deck[randomPosition];
                value.Position = new Vector2(1750, 100);
                m_Deck[randomPosition] = m_Deck[i];
                m_Deck[i] = value;
            }
        }
    }
}
