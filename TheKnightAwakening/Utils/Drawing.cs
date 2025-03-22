using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Drawing
    {
        // Graphics Device
        public GraphicsDevice GraphicsDevice { get; private set; }

        // Camera
        public Camera Camera { get; private set; }

        // Map
        public Map Map { get; private set; }

        // Cutscene
        public CutScene Cutscene { get; private set; }

        // Font
        public SpriteFont Font { get; private set; }

        // Textures
        public Texture2D Background { get; private set; }
        public Texture2D MenuIcon { get; private set; }
        public Texture2D MenuButton { get; private set; }
        public Texture2D ChestSheet { get; private set; }
        public Texture2D CoinSheet { get; private set; }
        public Texture2D PotionSheet { get; private set; }
        public Texture2D IconSheet { get; private set; }

        // Button Rectangles
        public Rectangle MenuPlay { get; private set; }
        public Rectangle MenuExit { get; private set; }
        public Rectangle Setting { get; private set; }

        // Health Bar
        public Healthbar Healthbar { get; private set; }
        public HealthbarAnimated HealthbarAnimated { get; private set; }

        // Ultimate Bar
        public Ultimatebar Ultimatebar { get; private set; }
        public UltimatebarAnimated UltimatebarAnimated { get; private set; }

        public Drawing(GraphicsDevice graphicsDevice, Camera camera, Map map, CutScene cutScene)
        {
            GraphicsDevice = graphicsDevice;
            Cutscene = cutScene;
            Camera = camera;
            Map = map;

            MenuPlay = new Rectangle(565, 410, 165, 98);
            MenuExit = new Rectangle(565, 524, 165, 98);
            Setting = new Rectangle(16, 128, 14, 14);
        }

        public void LoadContent(ContentManager Content)
        {
            // Background Game
            Background = Content.Load<Texture2D>("bg");

            // Main Menu
            MenuIcon = Content.Load<Texture2D>("icon_game");
            MenuButton = Content.Load<Texture2D>("menu");

            // Font
            Font = Content.Load<SpriteFont>("game_font");

            // Objects
            ChestSheet = Content.Load<Texture2D>("chest");
            CoinSheet = Content.Load<Texture2D>("gold_coin");
            PotionSheet = Content.Load<Texture2D>("health_potion");
            IconSheet = Content.Load<Texture2D>("Icon-sheet");

            // Health Bar
            Texture2D healthbarTexture = Content.Load<Texture2D>("health_bar");
            Rectangle bgSource = new Rectangle(0, 0, 163, 23);
            Rectangle fgSource = new Rectangle(21, 36, 140, 7);

            Healthbar = new Healthbar(healthbarTexture, bgSource, fgSource, 100);
            HealthbarAnimated = new HealthbarAnimated(healthbarTexture, bgSource, fgSource, 100);

            // Ultimate Bar
            Texture2D ultimateTexture = Content.Load<Texture2D>("ultimate");
            Rectangle bgSourceUltimate = new Rectangle(25, 0, 41, 44);
            Rectangle fgSourceUltimate = new Rectangle(0, 12, 17, 19);

            Ultimatebar = new Ultimatebar(ultimateTexture, bgSourceUltimate, fgSourceUltimate, 5);
            UltimatebarAnimated = new UltimatebarAnimated(ultimateTexture, bgSourceUltimate, fgSourceUltimate, 5);

            // Load Content
            Singleton.Instance._rect = new Texture2D(this.GraphicsDevice, 20, 20);
            Color[] data = new Color[20 * 20];
            for (int i = 0; i < data.Length; i++) data[i] = Color.White;
            Singleton.Instance._rect.SetData(data);

            // Map
            Map.LoadContent(Content);
            Singleton.Instance.HitblockTiles = Map.GetCollisionRectangles();

            // Cutscene
            Cutscene.LoadContent(Content);
        }

        public void _DrawStart(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(Background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
            _spriteBatch.End();

            // Layer 2: Icon and Button
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(MenuIcon, new Rectangle(394, 0, 492, 427), Color.White); // Icon
            _spriteBatch.Draw(MenuButton, MenuPlay, new Rectangle(167, 1, 165, 98), Color.White); // Play
            _spriteBatch.Draw(MenuButton, MenuExit, new Rectangle(0, 0, 165, 98), Color.White); // Exit
            _spriteBatch.End();
        }

        public void _DrawPause(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(Background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
            _spriteBatch.End();

            // Layer 2: Button UI Pause
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.End();
        }

        public void _DrawPlaying(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(Background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
            _spriteBatch.End();

            // Layer 2: Map & Camera
            _spriteBatch.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);
            Map.Draw(_spriteBatch);

            for (int i = 0; i < _numOjects; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }
            _spriteBatch.End();

            // Layer 3: UI
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Healthbar.Draw(_spriteBatch);
            HealthbarAnimated.Draw(_spriteBatch);
            Ultimatebar.Draw(_spriteBatch);
            UltimatebarAnimated.Draw(_spriteBatch);

            _spriteBatch.DrawString(Font, Singleton.Instance.Score.ToString(), new Vector2(1050, 55), Color.White);
            _spriteBatch.Draw(CoinSheet, new Vector2(1100, 50), new Rectangle(0, 0, 27, 27), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0); // Background
            _spriteBatch.Draw(IconSheet, new Vector2(1175, 53), Setting, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0);
            _spriteBatch.End();
        }

        public void _DrawOver(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(Background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
            _spriteBatch.End();

            // Layer 2: Button To Exit
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            // Update Logic
        }
    }
}
