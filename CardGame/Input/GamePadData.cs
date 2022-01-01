using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    class GamePadData
    {
		private PlayerIndex  m_PlayerIndex;
		private GamePadState m_CurrentState;
		private GamePadState m_PreviousState;
		private float		 m_DeadZone = 0.2f; // How far analog moves to count as "pressed"
		private float        m_RumbleTime = 0.0f;
		private float        m_RumbleStrength = 0.0f;

		public GamePadData(PlayerIndex index)
        {
			m_PlayerIndex = index;
        }

		public GamePadState CurrentGamePadState()
        {
			return m_CurrentState;
        }

		public GamePadState PreviousGamePadState()
        {
			return m_PreviousState;
        }

		public void StartRumble(float strength, float time)
        {
			if(m_RumbleTime <= 0.0f || m_RumbleStrength < strength || (strength == m_RumbleStrength && time > m_RumbleTime))
            {
				GamePad.SetVibration(m_PlayerIndex, strength, strength);
				m_RumbleStrength = strength;
				m_RumbleTime = time;
            }
		}

		public void StopRumble()
        {
			GamePad.SetVibration(m_PlayerIndex, 0.0f, 0.0f);
			m_RumbleStrength = 0;
			m_RumbleTime = 0.0f;
        }

		public void SetDeadZone(float deadzone = 0.25f)
        {
			m_DeadZone = deadzone;
        }

		//--ThumStick Stuff--
		// Basic non radial deadzone yuk
		public Vector2 GetLeftStick()
        {
			// Only return if the stick is greater than the deadzone (radial in this case)
			Vector2 product = m_CurrentState.ThumbSticks.Left;
			if (product.LengthSquared() < m_DeadZone * m_DeadZone)
			{
				product = Vector2.Zero;
			}
			return product;
		}

		public Vector2 GetRightStick()
        {
			// Only return if the stick is greater than the deadzone (radial in this case)
			Vector2 product = m_CurrentState.ThumbSticks.Right;
			if (product.LengthSquared() < m_DeadZone * m_DeadZone)
			{
				product = Vector2.Zero;
			}
			return product;
		}
		 
		public bool IsButton(Buttons button)
        {
			return m_CurrentState.IsButtonDown(button);
        }

		// True if down during frame Only (think one shot vs full auto)
		public bool IsButtonDown(Buttons button)
        {
			return m_PreviousState.IsButtonUp(button) && m_CurrentState.IsButtonDown(button);
        }
		// True if up during frame Only (think one shot vs full auto)
		public bool IsButtonUp(Buttons button)
		{
			return m_PreviousState.IsButtonDown(button) && m_CurrentState.IsButtonUp(button);
		}

		// Update the gamepad
		public void Update()
		{
			m_PreviousState = m_CurrentState;
			m_CurrentState = GamePad.GetState(m_PlayerIndex, GamePadDeadZone.None);

			if (m_RumbleTime > 0.0f)
			{
				m_RumbleTime -= Engine.Instance.DeltaTime;
			}
			else
            {
				StopRumble();
            }
		}
    }
}
