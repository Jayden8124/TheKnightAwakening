using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Singleton
    {
        // Size of the screen
        public const int SCREENWIDTH = 1280;
        public const int SCREENHEIGHT = 720;

        // Utility variables
        public int Score;
        public long Timer;
        public Random Random;
        public Player player;

        // Map Hit Block
        public List<Rectangle> HitblockTiles;

        // Rectacngle
        public Texture2D _rect;

        // Section Monster 
        public int CurrentSection = 0;
        public int PreviousSection = -1;


        // Game state
        public enum GameState
        {
            GameStart,
            Cutscene,
            GamePlaying,
            GamePaused,
            GameOver
        }
        public GameState CurrentGameState;

        // Cutscene state
        public enum CutsceneType
        {
            StartGame,
            BossRoom,
            BossDefeated,
            BossKilled,
            BossSpared,
            EndCredits
        }
        public CutsceneType currentCutscene;


        public Audio AudioManager;

        // Input State
        public KeyboardState PreviousKey, CurrentKey;
        public MouseState PreviousMouse, CurrentMouse;

        // Singleton instance
        private static Singleton instance;
        private Singleton() { }
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}