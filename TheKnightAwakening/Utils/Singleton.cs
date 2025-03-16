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

        // Game state
        public enum GameState
        {
            Start,
            Cutscene,
            GamePlaying,
            GamePaused,
            GameOver
        }
        public GameState CurrentGameState;

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