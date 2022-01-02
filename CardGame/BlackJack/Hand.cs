using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    /// <summary>
    /// Hand represents the players current cards, provides logic for evaluating score and drawing cards.
    /// </summary>
    class Hand
    {
        List<Card>  m_Hand = new List<Card>();
        int         m_Score = 0;
        bool        m_Dirty = true;

        public Vector2 Position;

        /// <summary>
        /// Appends cards to a card list
        /// </summary>
        /// <param name="card">Card being added</param>
        /// <param name="faceDown">Should this card be faced down?</param>
        public void AddCard(Card card, bool faceDown = false)
        {
            m_Hand.Add(card);
            m_Dirty = true;

            card.FaceDown = faceDown;
        }

        /// <summary>
        /// Remove all cards from hand
        /// </summary>
        public void Clear()
        {
            m_Hand.Clear();
            m_Dirty = true;
        }

        /// <summary>
        /// Evaluates the score of the cards in the hand.
        /// </summary>
        /// <param name="includeFaceDown">Weather facedown cards are invluded in the score tally.</param>
        /// <returns>The score of the hand</returns>
        public int EvaluateHand(bool includeFaceDown = false)
        {
            if (m_Dirty)
            {
                int aceCount = 0;
                m_Score = 0;

                for (int i = 0; i < m_Hand.Count; ++i)
                {
                    if (includeFaceDown == false && m_Hand[i].FaceDown) { continue; } // Ignore face down cards for dealer etc.
                    if (m_Hand[i].GetRank() == 0) { ++aceCount; }
                    m_Score += GetCardValue(m_Hand[i]);
                }

                while(aceCount > 0 && m_Score > 21)
                {
                    --aceCount;
                    m_Score -= 10;
                }

                m_Dirty = false;
            }

            return m_Score;
        }

        private int GetCardValue(Card card)
        {
            int rank = card.GetRank();
            if (rank > 9)
            {
                return 10;
            }
            // Edge case for Ace
            else if (rank == 0)
            {
                return 11;
            }

            // Plus one as cards are 0-12
            return rank + 1;
        }

        public void ShowCards()
        {
            if (m_Hand.Count > 0)
            {
                m_Hand[0].FaceDown = false;
                m_Dirty = true;
            }
        }

        //--Update Cards for Animations--
        public void Update(float deltaTime)
        {
            // Find center of cards in hand and minus off the position to keep things centred.
            float x = Position.X - (int)((Card.CARD_WIDTH + (Card.CARD_WIDTH * 0.5f * (m_Hand.Count - 1))) * 0.5f) + Card.CARD_WIDTH * 0.5f;
            for (int i = 0; i < m_Hand.Count; ++i)
            {
                Card card = m_Hand[i];
                card.Update(deltaTime);
                card.TargetPosition = new Vector2(x + (i * (Card.CARD_WIDTH * 0.5f)), Position.Y);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < m_Hand.Count; ++i)
            {
                m_Hand[i].Draw(spriteBatch);
            }
        }
    };
}
