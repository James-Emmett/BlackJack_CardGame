using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
	enum MouseButton
    {
		None,
		Left,
		Middle,
		Right
    }

    class MouseData
    {
		MouseState m_CurrentMouseState;
		MouseState m_PreviousMouseState;

		public bool IsButton(MouseButton button)
		{
            switch (button)
            {
                case MouseButton.Left:
					return m_CurrentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Middle:
					return m_CurrentMouseState.MiddleButton == ButtonState.Pressed;
				case MouseButton.Right:
					return m_CurrentMouseState.RightButton == ButtonState.Pressed;
				default:
					return false;
            }
		}

		public bool IsButtonDown(MouseButton button)
		{
			switch (button)
			{
				case MouseButton.Left:
					return  m_CurrentMouseState.LeftButton == ButtonState.Pressed &&
							m_PreviousMouseState.LeftButton == ButtonState.Released;
				case MouseButton.Middle:
					return  m_CurrentMouseState.MiddleButton == ButtonState.Pressed &&
							m_PreviousMouseState.MiddleButton == ButtonState.Released;
				case MouseButton.Right:
					return  m_CurrentMouseState.RightButton == ButtonState.Pressed &&
							m_PreviousMouseState.RightButton == ButtonState.Released;
				default:
					return false;
			}

		}

		public bool IsButtonUp(MouseButton button)
		{
			switch (button)
			{
				case MouseButton.Left:
					return m_CurrentMouseState.LeftButton == ButtonState.Released &&
							m_PreviousMouseState.LeftButton == ButtonState.Pressed;
				case MouseButton.Middle:
					return m_CurrentMouseState.MiddleButton == ButtonState.Released &&
							m_PreviousMouseState.MiddleButton == ButtonState.Pressed;
				case MouseButton.Right:
					return m_CurrentMouseState.RightButton == ButtonState.Released &&
							m_PreviousMouseState.RightButton == ButtonState.Pressed;
				default:
					return false;
			}
		}

		public int GetMouseScroll()
		{
			return m_CurrentMouseState.ScrollWheelValue - m_PreviousMouseState.ScrollWheelValue;
		}

		// Mouse in screen space [0 - Window Width]
		public Point GetMousePosition()
		{
			return m_CurrentMouseState.Position;
		}

		// Mouse in world scale resolution [0 - Target Width]
		public Vector2 GetMouseWorldPosition()
        {
			return Vector2.Transform(new Vector2(m_CurrentMouseState.Position.X, m_CurrentMouseState.Position.Y), DisplayManager.Instance.InvScaleMatrix);
        }

		public Point GetMouseDelta()
		{
			return m_CurrentMouseState.Position - m_PreviousMouseState.Position;
		}

		public void Update()
		{
			m_PreviousMouseState = m_CurrentMouseState;
			m_CurrentMouseState = Mouse.GetState();
		}
    }
}
