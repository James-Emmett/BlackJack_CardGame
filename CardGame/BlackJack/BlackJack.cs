using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public enum TurnState { Player, Dealer, None}
    class BlackJack : Scene
    {
        Background          m_Background;
        Deck                m_Deck;
        Player              m_Player;
        Dealer              m_Dealer;
        ScoreBox            m_PlayerScoreBox;
        ScoreBox            m_DealerScoreBox;
        Button              m_Replay;
        SpriteFont          m_SF_Calibri;
        TurnState           m_TurnState;
        string              m_WinText = "";
        bool                m_Playing = false;

        public override void LoadContent() 
        {
            ContentManager content = Engine.Instance.Content;
            m_Background = new Background(content.Load<Texture2D>("Textures\\Backgrounds\\Blue_Background"));
            m_Replay     = new Button(new Sprite(content.Load<Texture2D>("Textures\\Buttons\\Replay")), new Vector2(1820, 980));
            m_Deck       = new Deck();
            m_Deck.LoadContent(content);

            m_Player = new Player(m_Deck, content);
            m_Player.m_Hand.Position = new Vector2(960, 792);

            m_Dealer = new Dealer(m_Deck, content);
            m_Dealer.m_Hand.Position = new Vector2(960, 217);

            m_PlayerScoreBox = new ScoreBox(content, new Vector2(960, 646));
            m_DealerScoreBox = new ScoreBox(content, new Vector2(960, 76));
            m_SF_Calibri = content.Load<SpriteFont>("Fonts\\Calibri");

            Reset();

            IsLoaded = true; 
        }

        public override void OnEnter() 
        {
            if (IsLoaded) { Reset(); }
        }

        public override void Update(float deltaTime) 
        {
            m_Player.Update(deltaTime);
            m_Dealer.Update(deltaTime);

            if (m_Playing)
            {
                if (m_TurnState == TurnState.Player)
                {
                    m_Player.RunLogic();

                    if(m_Player.State == PlayerState.Stand)
                    {
                        m_TurnState = TurnState.Dealer;
                    }
                    else if (m_Player.State == PlayerState.Bust)
                    {
                        // Bust so just do score.
                        m_Playing = false;
                        m_TurnState = TurnState.None;
                        m_WinText = "Player Bust, Dealer Wins!!";
                    }
                }
                else if (m_TurnState == TurnState.Dealer)
                {
                    m_Dealer.RunLogic();

                    if (m_Dealer.State == PlayerState.Stand)
                    {
                        m_TurnState = TurnState.None;
                    }
                    else if (m_Dealer.State == PlayerState.Bust)
                    {
                        // Bust so just do score.
                        m_TurnState = TurnState.None;
                        m_Playing = false;
                        m_WinText = "Dealer Bust, Player Wins!!";
                    }
                }
                else
                {
                    EvaluateWinner();
                }
            }
            else // Show the Score/reset buttons
            {
                m_Replay.Update();

                if(m_Replay.Clicked)
                {
                    Reset();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, DisplayManager.Instance.ScaleMatrix);
            m_Background.Draw(spriteBatch);
            m_Deck.Draw(spriteBatch);
            m_Player.Draw(spriteBatch);
            m_Dealer.Draw(spriteBatch);

            m_PlayerScoreBox.Text = "Score: " + m_Player.Score();
            m_DealerScoreBox.Text = "Score: " + m_Dealer.Score();

            m_PlayerScoreBox.Draw(spriteBatch);
            m_DealerScoreBox.Draw(spriteBatch);

            if (m_Playing == false)
            {
                Vector2 size = m_SF_Calibri.MeasureString(m_WinText);
                spriteBatch.DrawString(m_SF_Calibri, m_WinText, new Vector2(960 - (size.X * 0.5f), 460 - (size.Y * 0.5f)), Color.White);
                m_Replay.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public void EvaluateWinner()
        {
            if(m_Player.Score() > m_Dealer.Score())
            {
                m_WinText = "Player Wins!";
            }
            else if (m_Player.Score() < m_Dealer.Score())
            {
                m_WinText = "Dealer Wins!";
            }
            else
            {
                m_WinText = "Draw!";
            }

            m_Playing = false;
        }


        public void Reset()
        {
            m_WinText = "";
            m_Deck.Reset();
            m_Player.Reset();
            m_Dealer.Reset();
            m_Playing = true;
            m_TurnState = TurnState.Player;
        }
    }
}
