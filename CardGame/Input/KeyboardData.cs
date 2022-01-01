using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    class KeyboardData
    {
        KeyboardState m_CurrentKeyboardState;
        KeyboardState m_PreviousKeyboardState;

        public bool IsKey(Keys key)
        {
            return m_CurrentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return m_CurrentKeyboardState.IsKeyDown(key) && m_PreviousKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return m_CurrentKeyboardState.IsKeyUp(key) && m_PreviousKeyboardState.IsKeyDown(key);
        }

        // Update the keyboard and previous, Call at start of update cycle!
        public void Update()
        {
            m_PreviousKeyboardState = m_CurrentKeyboardState;
            m_CurrentKeyboardState = Keyboard.GetState();
        }
    }
};