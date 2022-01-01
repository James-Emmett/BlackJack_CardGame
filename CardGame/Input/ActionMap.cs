using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CardGame
{
    struct ActionKey
    {
        public Keys         m_Key;
        public Buttons      m_GamePadButton; // Could make this a list? LeftStick, dPad could do same action for instance?
        public MouseButton  m_MouseButton;

        public ActionKey(Keys key = Keys.A, Buttons gamePadButton = Buttons.A, MouseButton mouseButton = MouseButton.None)
        {
            m_Key           = key;
            m_GamePadButton = gamePadButton;
            m_MouseButton   = mouseButton;
        }
    };

    public enum InputAction
    {
        Menu,
        Ok,
        Back,
        MenuUp,
        MenuDown,

        // Player stuff:
        Action,
        MoveLeft,
        MoveRight,
        TotalCount // Always have this last its a count for how many Enum Members!
    };

    class ActionMap
    {
        List<ActionKey> m_ActionMap;

        public void Initialize()
        {
            // Note this could be data driven from a file, replace enum with just the Index in an array.
            m_ActionMap = new List<ActionKey>((int)InputAction.TotalCount);

            for (int i = 0; i < (int)InputAction.TotalCount; i++)
            {
                m_ActionMap.Add(new ActionKey());
            }

            m_ActionMap[(int)InputAction.Menu]      = new ActionKey(Keys.Escape,Buttons.Start);
            m_ActionMap[(int)InputAction.Ok]        = new ActionKey(Keys.Enter, Buttons.A, MouseButton.Left);
            m_ActionMap[(int)InputAction.Back]      = new ActionKey(Keys.B, Buttons.B);
            m_ActionMap[(int)InputAction.MenuUp]    = new ActionKey(Keys.W, Buttons.LeftThumbstickUp);
            m_ActionMap[(int)InputAction.MenuDown]  = new ActionKey(Keys.S, Buttons.LeftThumbstickDown);

            m_ActionMap[(int)InputAction.Action]    = new ActionKey(Keys.Space, Buttons.A);
            m_ActionMap[(int)InputAction.MoveLeft]  = new ActionKey(Keys.A,     Buttons.LeftThumbstickLeft);
            m_ActionMap[(int)InputAction.MoveRight] = new ActionKey(Keys.D,     Buttons.LeftThumbstickRight);
        }


        public bool IsAction(InputAction action, PlayerIndex index = PlayerIndex.One)
        {
            ActionKey key = m_ActionMap[(int)action];
            return Input.Keyboard.IsKey(key.m_Key) || Input.GetGamePad(index).IsButton(key.m_GamePadButton);
        }

        public bool IsActionDown(InputAction action, PlayerIndex index = PlayerIndex.One)
        {
            ActionKey key = m_ActionMap[(int)action];
            return Input.Keyboard.IsKeyDown(key.m_Key) || Input.GetGamePad(index).IsButtonDown(key.m_GamePadButton);
        }

        public bool IsActionUp(InputAction action, PlayerIndex index = PlayerIndex.One)
        {
            ActionKey key = m_ActionMap[(int)action];
            return Input.Keyboard.IsKeyUp(key.m_Key) || Input.GetGamePad(index).IsButtonUp(key.m_GamePadButton);
        }
    }
}
