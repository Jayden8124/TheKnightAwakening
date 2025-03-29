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
        public Texture2D ButtonUI { get; private set; }
        public Texture2D Background { get; private set; }
        public Texture2D MenuBackground { get; private set; }
        public Texture2D MenuIcon { get; private set; }
        public Texture2D MenuButton { get; private set; }
        public Texture2D ChestSheet { get; private set; }
        public Texture2D CoinSheet { get; private set; }
        public Texture2D PotionSheet { get; private set; }
        public Texture2D IconSheet { get; private set; }
        public Texture2D FlagSheet { get; private set; }
        public Texture2D House { get; private set; }
        public Texture2D HowToPlay { get; private set; }
        public Texture2D HealthBarTexture { get; private set; }
        public Texture2D UltimateTexture { get; private set; }

        // Button Rectangles
        public Rectangle MenuPlay { get; private set; }
        public Rectangle MenuExit { get; private set; }
        public Rectangle Setting { get; private set; }
        public Rectangle PauseExit { get; private set; }
        public Rectangle PauseResume { get; private set; }
        public Rectangle BackgroundSourceHealthBar { get; private set; }
        public Rectangle ForegroundSourceHealthBar { get; private set; }
        public Rectangle BackgroundSourceUltimate { get; private set; }
        public Rectangle ForegroundSourceUltimate { get; private set; }

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

            // Button Rectangles for Contain Mouse Destination
            MenuPlay = new Rectangle(565, 410, 165, 98);
            MenuExit = new Rectangle(565, 524, 165, 98);
            Setting = new Rectangle(1175, 53, 35, 35);
            PauseExit = new Rectangle(565, 364, 167, 100);
            PauseResume = new Rectangle(565, 250, 167, 100);

            // Health Bar Source Rectangles
            BackgroundSourceHealthBar = new Rectangle(0, 0, 163, 23);
            ForegroundSourceHealthBar = new Rectangle(21, 36, 140, 7);

            // Ultimate Bar Source Rectangles
            BackgroundSourceUltimate = new Rectangle(25, 0, 41, 44);
            ForegroundSourceUltimate = new Rectangle(0, 12, 17, 19);
        }

        public void LoadContent(ContentManager Content)
        {
            {   // Background Game
                Background = Content.Load<Texture2D>("bg");
                House = Content.Load<Texture2D>("House");
                HowToPlay = Content.Load<Texture2D>("how_to_play");
            }

            {   // Main Menu
                MenuBackground = Content.Load<Texture2D>("bg_menu");
                MenuIcon = Content.Load<Texture2D>("icon_game");
                MenuButton = Content.Load<Texture2D>("menu");
            }

            {   // Font
                Font = Content.Load<SpriteFont>("game_font");
            }

            {    // Objects
                ButtonUI = Content.Load<Texture2D>("Button Ui");
                ChestSheet = Content.Load<Texture2D>("chest");
                CoinSheet = Content.Load<Texture2D>("gold_coin");
                PotionSheet = Content.Load<Texture2D>("health_potion");
                IconSheet = Content.Load<Texture2D>("Icon-sheet");
                FlagSheet = Content.Load<Texture2D>("Flag_Raise");
            }

            {   // Health Bar
                HealthBarTexture = Content.Load<Texture2D>("health_bar");
                Healthbar = new Healthbar(HealthBarTexture, BackgroundSourceHealthBar, ForegroundSourceHealthBar, 100);
                HealthbarAnimated = new HealthbarAnimated(HealthBarTexture, BackgroundSourceHealthBar, ForegroundSourceHealthBar, 100);
            }

            {   // Ultimate Bar
                UltimateTexture = Content.Load<Texture2D>("ultimate");
                Ultimatebar = new Ultimatebar(UltimateTexture, BackgroundSourceUltimate, ForegroundSourceUltimate, 5);
                UltimatebarAnimated = new UltimatebarAnimated(UltimateTexture, BackgroundSourceUltimate, ForegroundSourceUltimate, 5);
            }

            {   // Load Content
                Singleton.Instance._rect = new Texture2D(this.GraphicsDevice, 20, 20);
                Color[] data = new Color[20 * 20];
                for (int i = 0; i < data.Length; i++) data[i] = Color.White;
                Singleton.Instance._rect.SetData(data);
            }

            {   // Map
                Map.LoadContent(Content);
                Singleton.Instance.HitblockTiles = Map.GetCollisionRectangles();

                // Cutscene
                Cutscene.LoadContent(Content);
            }
        }

        public void _DrawStart(SpriteBatch _spriteBatch)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(MenuBackground, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
            _spriteBatch.End();

            // Layer 2: Icon and Button
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(MenuIcon, new Rectangle(394, 0, 492, 427), Color.White); // Icon
            _spriteBatch.Draw(MenuButton, MenuPlay, new Rectangle(167, 1, 165, 98), Color.White); // Play
            _spriteBatch.Draw(MenuButton, MenuExit, new Rectangle(0, 0, 165, 98), Color.White); // Exit
            _spriteBatch.End();
        }

        public void _DrawPause(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(MenuBackground, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
            _spriteBatch.End();

            // Layer 2: Icon and Button
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(MenuIcon, new Rectangle(394, 0, 492, 427), Color.White); // Icon
            _spriteBatch.Draw(MenuButton, MenuPlay, new Rectangle(167, 1, 165, 98), Color.White); // Play
            _spriteBatch.Draw(MenuButton, MenuExit, new Rectangle(0, 0, 165, 98), Color.White); // Exit
            _spriteBatch.End();
            {
                // Layer 1: Background
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

                // Background Texture
                _spriteBatch.Draw(Background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

                _spriteBatch.End();

                // Layer 2: Map & Camera
                _spriteBatch.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);
                Map.Draw(_spriteBatch);


                // How To Play Texture
                _spriteBatch.Draw(HowToPlay, new Rectangle(422, 246, 233, 152), new Rectangle(10, 212, 233, 152), Color.White); // Walk
                _spriteBatch.Draw(HowToPlay, new Rectangle(5221, 326, 173, 70), new Rectangle(10, 10, 173, 70), Color.White); // Jump
                _spriteBatch.Draw(HowToPlay, new Rectangle(6651, 100, 251, 245), new Rectangle(10, 384, 251, 245), Color.White); // Attack
                _spriteBatch.Draw(HowToPlay, new Rectangle(11259, 105, 215, 92), new Rectangle(10, 100, 215, 92), Color.White); // Interact

                // House Texture
                _spriteBatch.Draw(House, new Rectangle(1277, 394, 155, 133), new Rectangle(0, 0, 155, 133), Color.White); // House 1
                _spriteBatch.Draw(House, new Rectangle(3602, 394, 155, 133), new Rectangle(0, 0, 155, 133), Color.White); // House 2 
                _spriteBatch.Draw(House, new Rectangle(8063, 395, 155, 133), new Rectangle(0, 0, 155, 133), Color.White); // House 3

                for (int i = 0; i < _numOjects; i++)
                {
                    _gameObjects[i].Draw(_spriteBatch);
                }
                _spriteBatch.End();

                // Layer 3: Button UI Pause
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Healthbar.Draw(_spriteBatch);
                HealthbarAnimated.Draw(_spriteBatch);
                Ultimatebar.Draw(_spriteBatch);
                // UltimatebarAnimated.Draw(_spriteBatch);

                _spriteBatch.DrawString(Font, Singleton.Instance.Score.ToString(), new Vector2(1050, 55), Color.White);
                _spriteBatch.Draw(CoinSheet, new Vector2(1100, 50), new Rectangle(0, 0, 27, 27), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0); // Coin Score
                _spriteBatch.Draw(IconSheet, Setting, new Rectangle(16, 128, 14, 14), Color.White); // Setting Icon

                _spriteBatch.Draw(ButtonUI, PauseResume, new Rectangle(37, 329, 594, 296), Color.White); // Resune
                _spriteBatch.Draw(ButtonUI, PauseExit, new Rectangle(681, 13, 594, 296), Color.White); // Exit
                _spriteBatch.End();
            }
        }

        public void _DrawPlaying(SpriteBatch _spriteBatch, List<GameObject> _gameObjects, int _numOjects)
        {
            // Layer 1: Background
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Background Texture
            _spriteBatch.Draw(Background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

            _spriteBatch.End();

            // Layer 2: Map & Camera
            _spriteBatch.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);
            Map.Draw(_spriteBatch);


            // How To Play Texture
            _spriteBatch.Draw(HowToPlay, new Rectangle(422, 246, 233, 152), new Rectangle(10, 212, 233, 152), Color.White); // Walk
            _spriteBatch.Draw(HowToPlay, new Rectangle(5221, 326, 173, 70), new Rectangle(10, 10, 173, 70), Color.White); // Jump
            _spriteBatch.Draw(HowToPlay, new Rectangle(6651, 100, 251, 245), new Rectangle(10, 384, 251, 245), Color.White); // Attack
            _spriteBatch.Draw(HowToPlay, new Rectangle(11259, 105, 215, 92), new Rectangle(10, 100, 215, 92), Color.White); // Interact

            // House Texture
            _spriteBatch.Draw(House, new Rectangle(1277, 394, 155, 133), new Rectangle(0, 0, 155, 133), Color.White); // House 1
            _spriteBatch.Draw(House, new Rectangle(3602, 394, 155, 133), new Rectangle(0, 0, 155, 133), Color.White); // House 2 
            _spriteBatch.Draw(House, new Rectangle(8063, 395, 155, 133), new Rectangle(0, 0, 155, 133), Color.White); // House 3

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
            // UltimatebarAnimated.Draw(_spriteBatch);

            _spriteBatch.DrawString(Font, Singleton.Instance.Score.ToString(), new Vector2(1050, 55), Color.White);
            _spriteBatch.Draw(CoinSheet, new Vector2(1100, 50), new Rectangle(0, 0, 27, 27), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0); // Coin Score
            _spriteBatch.Draw(IconSheet, Setting, new Rectangle(16, 128, 14, 14), Color.White); // Setting Icon
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
