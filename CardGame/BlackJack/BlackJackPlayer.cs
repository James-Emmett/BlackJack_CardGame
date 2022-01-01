using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    enum PlayerState { Playing, Stand, Bust}
    abstract class BlackJackPlayer
    {
        public Deck m_Deck;
        public Hand m_Hand;
        public PlayerState State { get; protected set; }

        public BlackJackPlayer(Deck deck)
        {
            m_Deck = deck;
            m_Hand = new Hand();
        }

        public virtual void  Reset() { State = PlayerState.Playing; }
        public virtual void  Update(float deltaTime) { m_Hand.Update(deltaTime); }
        public abstract void RunLogic();
        public virtual void  Draw(SpriteBatch spriteBatch) { m_Hand.Draw(spriteBatch); }
        public int Score() { return m_Hand.EvaluateHand(); }
    }

    class Player : BlackJackPlayer
    {
        Button m_Hit;
        Button m_Stand;

        public Player(Deck deck, ContentManager content):base(deck)
        {
            m_Hit = new Button(new Sprite(content.Load<Texture2D>("Textures\\Buttons\\Hit_Enabled")),
                   new Sprite(content.Load<Texture2D>("Textures\\Buttons\\Hit_Disabled")), new Vector2(805, 980));

            m_Stand = new Button(new Sprite(content.Load<Texture2D>("Textures\\Buttons\\Stand_Enabled")),
                                 new Sprite(content.Load<Texture2D>("Textures\\Buttons\\Stand_Disabled")), new Vector2(1115, 980));
        }

        public override void Reset()
        {
            m_Hand.Clear();
            m_Hand.AddCard(m_Deck.TakeCard());
            m_Hand.AddCard(m_Deck.TakeCard());
            m_Hit.Enabled = true;
            m_Stand.Enabled = true;
            base.Reset();
        }

        public override void Update(float deltaTime)
        {
            m_Hit.Update();
            m_Stand.Update();
            base.Update(deltaTime);
        }

        public override void RunLogic()
        {
            if (m_Hit.Clicked)
            {
                SoundManager.PlayEffect("cardEffect");
                m_Hand.AddCard(m_Deck.TakeCard());
            }
            else if (m_Stand.Clicked)
            {
                State = PlayerState.Stand;
                m_Hit.Enabled = false;
                m_Stand.Enabled = false;
            }

            // Bust
            if (m_Hand.EvaluateHand() > 21)
            {
                m_Hit.Enabled = false;
                m_Stand.Enabled = false;
                State = PlayerState.Bust;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            m_Hit.Draw(spriteBatch);
            m_Stand.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }

    class Dealer : BlackJackPlayer
    {
        const float ThinkTimer = 0.8f;
        float m_Timer = 0;
        bool m_Flipped = false;

        public Dealer(Deck deck, ContentManager content) : base(deck)
        {
            m_Timer = ThinkTimer;
        }

        public override void Reset()
        {
            m_Hand.Clear();
            m_Hand.AddCard(m_Deck.TakeCard(), true);
            m_Hand.AddCard(m_Deck.TakeCard());

            m_Timer = ThinkTimer;
            m_Flipped = false;
            base.Reset();
        }

        public override void Update(float deltaTime)
        {
            m_Timer -= deltaTime;
            base.Update(deltaTime);
        }

        public override void RunLogic()
        {
            if(m_Flipped == false) { m_Hand.ShowCards(); SoundManager.PlayEffect("cardEffect"); m_Flipped = true; }
            if(m_Timer <= 0)
            {
                if (m_Hand.EvaluateHand() < 16)
                {
                    SoundManager.PlayEffect("cardEffect");
                    m_Hand.AddCard(m_Deck.TakeCard());
                }
                else
                {
                    State = PlayerState.Stand;
                }

                // Bust
                if (m_Hand.EvaluateHand() > 21)
                {
                    State = PlayerState.Bust;
                }
                m_Timer = ThinkTimer;
            }
        }
    }
}
