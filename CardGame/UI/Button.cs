using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CardGame
{
    public class Button
    {
        Sprite          m_SpriteEnabled;
        Sprite          m_SpriteDisabled;
        Rectangle       m_Bounds;
        Vector2         m_Position;
        public Color    TintColor;
        public bool     Clicked { get; private set; }
        public Vector2  Position 
        { 
            get { return m_Position; } 
            set { m_Position = value; UpdateBounds(); } 
        }

        public bool Enabled = true;

        public Button(Sprite enabledSprite, Vector2 position)
        {
            m_SpriteEnabled = enabledSprite;
            m_SpriteDisabled = null;
            m_Position = position;

            TintColor = new Color(0.65f, 0.65f, 0.65f, 1.0f);
            UpdateBounds();
        }


        public Button(Sprite enabledSprite, Sprite disabledSprite, Vector2 position)
        {
            m_SpriteEnabled = enabledSprite;
            m_SpriteDisabled = disabledSprite;
            m_Position = position;

            TintColor = new Color(0.65f, 0.65f, 0.65f, 1.0f);
            UpdateBounds();
        }

        private void UpdateBounds()
        {
            if (Enabled)
            {
                m_Bounds = m_SpriteEnabled.CalcualteBounds(Position);
            }
            else
            {
                if (m_SpriteDisabled != null)
                {
                    m_Bounds = m_SpriteDisabled.CalcualteBounds(Position);
                }
            }
        }

        public void Update()
        {
            if (Enabled)
            {
                Vector2 mousePosition = Input.Mouse.GetMouseWorldPosition();
                if (m_Bounds.Contains(mousePosition))
                {
                    m_SpriteEnabled.m_Color = TintColor;

                    if (Input.Mouse.IsButtonDown(MouseButton.Left))
                    {
                        Clicked = true;
                        return;
                    }
                }
                else
                {
                    m_SpriteEnabled.m_Color = Color.White;
                }
            }

            Clicked = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Enabled)
            {
                m_SpriteEnabled.Draw(spriteBatch, m_Position, 0, 1);
            }
            else
            {
                if (m_SpriteDisabled != null)
                {
                    m_SpriteDisabled.Draw(spriteBatch, m_Position, 0, 1);
                }
            }
        }
    }
}
