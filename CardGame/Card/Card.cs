using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    class Card
    {
        public const int CARD_WIDTH = 140;
        public const int CARD_HEIGHT = 190;

        Texture2D           m_CardAtlas;
        Texture2D           m_CardBack;
        Rectangle           m_Source;
        int                 m_ID;
        int                 m_Suite;
        int                 m_Rank;
        bool                m_IsRed;
        public bool         FaceDown = false;

        //--Transform Stuff--
        public Vector2      Origin;
        public Vector2      TargetPosition;
        public Vector2      Position;
        public float        Scale = 1;

        public Card(int id, Texture2D cardAtlas, Texture2D cardBack)
        {
            m_CardAtlas = cardAtlas;
            m_CardBack  = cardBack;
            m_ID    = id;
            m_Suite = id / 13;
            m_Rank  = id % 13;
            m_IsRed = (m_Suite % 2) == 1;
            m_Source = new Rectangle(CARD_WIDTH * m_Rank, CARD_HEIGHT * m_Suite, CARD_WIDTH, CARD_HEIGHT);
            Origin = new Vector2(CARD_WIDTH * 0.5f, CARD_HEIGHT * 0.5f);
        }

        public int ID()
        {
            return m_ID;
        }

        public int GetSuite()
        {
            return m_Suite;
        }

        public int GetRank()
        {
            return m_Rank;
        }

        public bool IsRed() 
        { 
            return m_IsRed; 
        }

        public bool Contains(Point point)
        {
            if(point.X > (Position.X - Origin.X) && point.X < (Position.X + Origin.X) &&
               point.Y > (Position.Y - Origin.Y) && point.Y < (Position.Y + Origin.Y))
            {
                return true;
            }

            return false;
        }

        public void Update(float deltaTime)
        {
            Position = Vector2.Lerp(Position, TargetPosition, 10 * deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(FaceDown)
            {
                spriteBatch.Draw(m_CardBack, Position, new Rectangle(0,0,CARD_WIDTH, CARD_HEIGHT), Color.White, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(m_CardAtlas, Position, m_Source, Color.White, 0, Origin, Scale, SpriteEffects.None, 0);
            }
        }
    }
}
