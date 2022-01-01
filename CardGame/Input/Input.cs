using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    static class Input
    {
        private static KeyboardData  s_KeyboardData;
        private static MouseData     s_MouseData;
        private static GamePadData[] s_GamePads;
        public  static ActionMap ActionMap;

        public static KeyboardData Keyboard { get { return s_KeyboardData; } private set { s_KeyboardData = value; } }
        public static MouseData Mouse { get { return s_MouseData; } private set { s_MouseData = value; } }

        public static void Initialise()
        {
            s_GamePads =  new GamePadData[] { new GamePadData(PlayerIndex.One), 
                                              new GamePadData(PlayerIndex.Two), 
                                              new GamePadData(PlayerIndex.Three), 
                                              new GamePadData(PlayerIndex.Four) };
            s_KeyboardData = new KeyboardData();
            s_MouseData = new MouseData();

            Keyboard = s_KeyboardData;
            Mouse = s_MouseData;
            ActionMap = new ActionMap();
            ActionMap.Initialize();
        }

        public static GamePadData GetGamePad(PlayerIndex index)
        {
            return s_GamePads[(int)index];
        }

        public static void Update()
        {
            s_MouseData.Update();
            s_KeyboardData.Update();
            for (int i = 0; i < 4; ++i)
            {
                s_GamePads[i].Update();
            }
        }

        public static void ShutDown()
        {
            for (int i = 0; i < 4; ++i)
            {
                s_GamePads[i].StopRumble();
            }
        }
    }

}
