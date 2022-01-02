using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CardGame
{
    enum PlayerState { Playing, Stand, Bust}

    /// <summary>
    /// Abstract Player containing base requirments for creating a Black Jack Player.
    /// </summary>
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

    /// <summary>
    /// Concrete Player providing user Input via buttons and runs User Logic.
    /// </summary>
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

        /// <summary>
        /// Executes the Logic for a players turn
        /// </summary>
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

    /// <summary>
    /// Concrete Player for the AI running logic for the Dealer
    /// </summary>
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

        /// <summary>
        /// Executes the logic for the dealer/AI
        /// </summary>
        public override void RunLogic()
        {
            // If its first time entering this round, then flip players card
            if(m_Flipped == false) { m_Hand.ShowCards(); SoundManager.PlayEffect("cardEffect"); m_Flipped = true; }
            
            // Timer to "Simulate" thinking.
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
