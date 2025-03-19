using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheKnightAwakening;

public class MainScene : Game
{
    // Graphics
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Font
    SpriteFont _font;

    // GameObjects
    List<GameObject> _gameObjects;
    public int _numOjects;

    // Health Bar
    private Healthbar _healthbar;
    private HealthbarAnimated _healthbarAnimated;

    // Ultimate
    private Ultimatebar _ultimatebar;
    private UltimatebarAnimated _ultimatebarAnimated;

    // Camera & Map
    private Camera _camera;
    private Map _map;

    // Textures
    private Texture2D _background, _menuIcon, _menuButton;
    private Texture2D chestSheet, coinSheet, potionSheet;

    // Button
    Rectangle _menuPlay, _menuExit;

    // Cutscene
    private CutScene _cutscene;


    public MainScene()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
        _graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
        _graphics.ApplyChanges();

        _gameObjects = new List<GameObject>();

        _camera = new Camera(GraphicsDevice.Viewport); // Initialize camera
        _map = new Map(GraphicsDevice); // Initialize map
        _cutscene = new CutScene(GraphicsDevice);

        _menuPlay = new Rectangle(565, 410, 165, 98);
        _menuExit = new Rectangle(565, 524, 165, 98);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _background = Content.Load<Texture2D>("bg");
        _menuIcon = Content.Load<Texture2D>("icon_game");
        _menuButton = Content.Load<Texture2D>("menu");
        _font = Content.Load<SpriteFont>("game_font");

        chestSheet = Content.Load<Texture2D>("chest");
        coinSheet = Content.Load<Texture2D>("gold_coin");
        potionSheet = Content.Load<Texture2D>("health_potion");

        {   // Health Bar
            Texture2D healthbarTexture = Content.Load<Texture2D>("health_bar");
            Rectangle bgSource = new Rectangle(0, 0, 163, 23);
            Rectangle fgSource = new Rectangle(21, 36, 140, 7);

            _healthbar = new Healthbar(healthbarTexture, bgSource, fgSource, 100);
            _healthbarAnimated = new HealthbarAnimated(healthbarTexture, bgSource, fgSource, 100);
        }

        {   // Ultimate Bar
            Texture2D ultimateTexture = Content.Load<Texture2D>("ultimate");
            Rectangle bgSourceUltimate = new Rectangle(25, 0, 41, 44);
            Rectangle fgSourceUltimate = new Rectangle(0, 12, 17, 19);

            _ultimatebar = new Ultimatebar(ultimateTexture, bgSourceUltimate, fgSourceUltimate, 5);
            _ultimatebarAnimated = new UltimatebarAnimated(ultimateTexture, bgSourceUltimate, fgSourceUltimate, 5);
        }

        // Load Content
        Singleton.Instance._rect = new Texture2D(_graphics.GraphicsDevice, 20, 20);
        Color[] data = new Color[20 * 20];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        Singleton.Instance._rect.SetData(data);

        _map.LoadContent(Content);
        _cutscene.LoadContent(Content);

        Singleton.Instance.HitblockTiles = _map.GetCollisionRectangles();

        Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        Singleton.Instance.CurrentMouse = Mouse.GetState();

        _numOjects = _gameObjects.Count;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.Start:
                if (IsButtonClicked(_menuPlay))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Cutscene;
                }
                else if (IsButtonClicked(_menuExit))
                {
                    Exit();
                }
                break;
            case Singleton.GameState.Cutscene:
                _cutscene.Update(gameTime);
                break;
            case Singleton.GameState.GamePlaying:
                // Mouse State
                // if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                //     Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                // {
                //     Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
                // }

                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePaused;

                for (int i = 0; i < _numOjects; i++)
                {
                    if (_gameObjects[i].IsActive)
                    {
                        _gameObjects[i].Update(gameTime, _gameObjects);
                    }
                }

                for (int i = 0; i < _numOjects; i++)
                {
                    if (!_gameObjects[i].IsActive)
                    {
                        _gameObjects.RemoveAt(i);
                        i--;
                        _numOjects--;
                    }
                }

                if (Singleton.Instance.player != null)
                {
                    _camera.Follow(Singleton.Instance.player);
                }

                // ตรวจสอบและแก้ไข collision สำหรับ Monster แต่ละตัว for Collision   
                foreach (var obj in _gameObjects)
                {
                    CollisionManager.ResolveCollision(obj, Singleton.Instance.HitblockTiles);
                    CollisionManager.UpdateOnGround(obj, Singleton.Instance.HitblockTiles);
                }

                // หลังจากที่ทำ Collision กับ tile map แล้ว
                foreach (var obj in _gameObjects)
                {
                    if (obj is MonsterType monster)
                    {
                        if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, monster.Rectangle))
                        {
                            CollisionManager.ResolveCharacterCollision(Singleton.Instance.player, monster);

                            if (!(monster is SKLT_WR))
                            {
                                Singleton.Instance.player.TakeDamage(1, obj.Position);
                            }
                        }
                    }
                }

                // foreach (var obj in _gameObjects)
                // {
                //     if (obj is Checkpoint checkpoint)
                //     {
                //         if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, checkpoint.Rectangle))
                //         {
                //             if (!checkpoint.Activated)
                //             {
                //                 checkpoint.Activate();
                //                 // คุณอาจเพิ่มเสียงหรือเอฟเฟคที่นี่
                //             }
                //             // อัปเดต LastCheckpoint ของผู้เล่นให้เป็นตำแหน่งของ checkpoint นี้
                //             Singleton.Instance.player.LastCheckpoint = checkpoint.Position;
                //         }
                //     }
                // }

                // // ตรวจสอบการตกออกนอกแผนที่ (เช่น ตกลงไปมากกว่า Y = 50000)
                // if (Singleton.Instance.player.Position.Y > 50000)
                // {
                //     // รีเซ็ตตำแหน่งผู้เล่นกลับไปที่ Checkpoint ล่าสุด
                //     Singleton.Instance.player.Position = Singleton.Instance.player.LastCheckpoint;
                //     Singleton.Instance.player.Velocity = Vector2.Zero;
                // }

                _healthbar.Update(Singleton.Instance.player.Health);
                _healthbarAnimated.Update(Singleton.Instance.player.Health, gameTime);

                _ultimatebar.Update(Singleton.Instance.player.Ultimate);
                // _ultimatebarAnimated.Update(Singleton.Instance.player.Ultimate, gameTime);

                break;

            case Singleton.GameState.GamePaused: //Game Paused
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                break;
            case Singleton.GameState.GameOver:
                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed && Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
                break;
        }

        Singleton.Instance.PreviousMouse = Singleton.Instance.CurrentMouse;
        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.Start:
                {
                    _DrawStart();
                }
                break;
            case Singleton.GameState.Cutscene:
                {
                    _cutscene.Draw(_spriteBatch);
                }
                break;
            case Singleton.GameState.GamePlaying:
                {
                    _DrawPlaying();
                }
                break;
            case Singleton.GameState.GamePaused:
                {
                    _DrawPause();
                }
                break;
            case Singleton.GameState.GameOver:
                {
                    _DrawOver();
                }
                break;
        }

        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    public void _DrawStart()
    {
        // Layer 1: Background
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

        _spriteBatch.End();

        // Layer 2: Icon and Button
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_menuIcon, new Rectangle(394, 0, 492, 427), Color.White); // Icon
        _spriteBatch.Draw(_menuButton, _menuPlay, new Rectangle(167, 1, 165, 98), Color.White); // Play
        _spriteBatch.Draw(_menuButton, _menuExit, new Rectangle(0, 0, 165, 98), Color.White); // Exit

        _spriteBatch.End();
    }
    public void _DrawPause()
    {
        // Layer 1: Background
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

        _spriteBatch.End();

        // Layer 2: Button UI Pause 
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Code Button (Left Click Press)

        _spriteBatch.End();
    }

    public void _DrawPlaying()
    {
        // Layer 1: Background
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

        _spriteBatch.End();

        // Layer 2: Map & Camera
        _spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);


        _map.Draw(_spriteBatch);

        for (int i = 0; i < _numOjects; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        _spriteBatch.End();

        // Layer 3: UI
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Setting Menu Button 
        // วาด HealthBar
        _healthbar.Draw(_spriteBatch);
        _healthbarAnimated.Draw(_spriteBatch);
        _ultimatebar.Draw(_spriteBatch);

        // _ultimatebarAnimated.Draw(_spriteBatch);


        _spriteBatch.DrawString(_font, Singleton.Instance.Score.ToString() ,new Vector2(1050, 55), Color.White);
        _spriteBatch.Draw(coinSheet, new Vector2(1100, 50), new Rectangle(0, 0, 27, 27), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0); // Background

        _spriteBatch.End();
    }
    public void _DrawOver()
    {
        // Layer 1: Background
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

        _spriteBatch.End();

        // Layer 2: Button To Exit
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Code Button To Exit to Main Menu or Restart

        _spriteBatch.End();
    }

    protected void Reset()
    {
        Singleton.Instance.Score = 0;
        Singleton.Instance.Timer = 0;
        Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;

        _gameObjects.Clear();

        ResetPlayer(); // Call PLayer Object
        ResetMonster(); // Call Monster Object
        ResetObject(); // Call Object

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }

    public void ResetPlayer() // Adjust
    {
        // Load Texture Singleton.Instance.player
        Texture2D knightSheet = Content.Load<Texture2D>("player");

        // Player Instance
        var _animationsPlayer = AnimationPlayer.LoadAnimations(knightSheet);

        Singleton.Instance.player = new Player(_animationsPlayer)
        {
            Name = "Player",
            Viewport = new Rectangle(5, 0, 43, 64),
            // Position = new Vector2(200, 100),
            LastCheckpoint = new Vector2(200, 0),
            Left = Keys.Left,
            Right = Keys.Right,
            Up = Keys.Up,
            Down = Keys.Down,
            Fire = Keys.Space,
            Defend = Keys.LeftControl,
            Attack2 = Keys.F,
            Attack3 = Keys.G,
            Bullet = new Bullet(knightSheet)
            {
                Name = "BulletPlayer",
                Viewport = new Rectangle(0, 0, 10, 10),
                Velocity = new Vector2(-500f, 0)
            }
        };

        _gameObjects.Add(Singleton.Instance.player);
    }

    public void ResetMonster()
    {
        // Load Texture each Monster Type
        var monsterTextures = new Dictionary<AnimationMonster.AnimationMonsterType, Texture2D>
        {
            { AnimationMonster.AnimationMonsterType.SKLT_WR, Content.Load<Texture2D>("skeleton_warrior") },
            { AnimationMonster.AnimationMonsterType.SKLT_SM, Content.Load<Texture2D>("skeleton_spearman") },
            { AnimationMonster.AnimationMonsterType.SKLT_AC, Content.Load<Texture2D>("skeleton_archer") },
            { AnimationMonster.AnimationMonsterType.SL, Content.Load<Texture2D>("blue_slime") },
            { AnimationMonster.AnimationMonsterType.MDS, Content.Load<Texture2D>("medusa") }
        };

        // Load Animation Monster
        AnimationMonster _animationMonster = new AnimationMonster();
        _animationMonster.LoadAllAnimations(monsterTextures);

        // Prototype each Monster Type
        MonsterType prototypeWR = new SKLT_WR(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SKLT_WR))
        {
            Name = "SKLT_WR",
            Score = 10,
            Viewport = new Rectangle(0, 0, 53, 58)
        };

        MonsterType prototypeSL = new SL(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SL))
        {
            Name = "SL",
            Score = 20,
            Viewport = new Rectangle(0, 0, 47, 32)
        };

        MonsterType prototypeSM = new SKLT_SM(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SKLT_SM))
        {
            Name = "SKLT_SM",
            Score = 30,
            Viewport = new Rectangle(0, 0, 30, 80)
        };

        MonsterType prototypeAC = new SKLT_AC(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SKLT_AC))
        {
            Name = "SKLT_AC",
            Score = 40,
            Viewport = new Rectangle(0, 0, 37, 64),
            Bullet = new Bullet(Content.Load<Texture2D>("skeleton_archer"))
            {
                Name = "BulletEnemy",
                Viewport = new Rectangle(384, 65, 45, 3),
                Velocity = new Vector2(0, 600f)
            }
        };

        MonsterType prototypeMDS = new MDS(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.MDS))
        {
            Name = "MDS",
            Score = 50,
            Viewport = new Rectangle(0, 0, 65, 77),
            Position = new Vector2(6740, 4049)
        };

        // // Postion each monster type
        // List<Vector2> spawnPositionsSL = SL.SpawnPositions;
        // List<Vector2> spawnPositionsWR = SKLT_WR.SpawnPositions;
        // List<Vector2> spawnPositionsSM = SKLT_SM.SpawnPositions;
        // List<Vector2> spawnPositionsAC = SKLT_AC.SpawnPositions;
        // List<Vector2> spawnPositionsMDS = SKLT_AC.SpawnPositions;

        // // Clone prototype and Set Potion from spawnPositions each Monster Type
        // foreach (var pos in spawnPositionsWR)
        // {
        //     var clone = prototypeWR.Clone();
        //     clone.Position = pos;
        //     clone.Name += "_" + pos.ToString();
        //     _gameObjects.Add(clone);
        // }

        // foreach (var pos in spawnPositionsSL)
        // {
        //     var clone = prototypeSL.Clone();
        //     clone.Position = pos;
        //     clone.Name += "_" + pos.ToString();
        //     _gameObjects.Add(clone);
        // }

        // foreach (var pos in spawnPositionsSM)
        // {
        //     var clone = prototypeSM.Clone();
        //     clone.Position = pos;
        //     clone.Name += "_" + pos.ToString();
        //     _gameObjects.Add(clone);
        // }

        // foreach (var pos in spawnPositionsAC)
        // {
        //     var clone = prototypeAC.Clone();
        //     clone.Position = pos;
        //     clone.Name += "_" + pos.ToString();
        //     _gameObjects.Add(clone);
        // }

        // foreach (var pos in spawnPositionsMDS)
        // {
        //     var clone = prototypeMDS.Clone();
        //     clone.Position = pos;
        //     clone.Name += "_" + pos.ToString();
        //     _gameObjects.Add(clone);
        // }
    }


    public void ResetObject() // Clone 
    {
        // Chest Instance
        var _animationsChest = AnimationChest.LoadAnimations(chestSheet);
        var goldChestAnimations = _animationsChest[ChestType.GoldChest];

        var _animationCoin = AnimationCoin.LoadAnimations(coinSheet);

        var _animationPotion = AnimationPotion.LoadAnimations(potionSheet);


        Chest chest = new Chest(goldChestAnimations)
        {
            Name = "Chest",
            openKey = Keys.E,
            Position = new Vector2(300, 750),
            coin = new Coin(_animationCoin)
            {
                Name = "Coin",
            },
            potion = new Potion(_animationPotion)
            {
                Name = "Potion",
            }
        };

        // Add GameObjects
        _gameObjects.Add(chest);

        Texture2D flagSheet = Content.Load<Texture2D>("Flag_Raise");

        var _animationFlag = AnimationFlag.LoadAnimations(flagSheet);

        Flag flag1 = new Flag(_animationFlag)
        {
            Name = "Flag",
            Viewport = new Rectangle(41, 20, 63, 172),
            Position = new Vector2(500, 500)
        };

        _gameObjects.Add(flag1);
        Flag flag2 = new Flag(_animationFlag)
        {
            Name = "Flag",
            Viewport = new Rectangle(41, 20, 63, 172),
            Position = new Vector2(1500, 500)
        };

        _gameObjects.Add(flag2);
    }

    private bool IsButtonClicked(Rectangle buttonRect)
    {
        return Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed && Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released && buttonRect.Contains(Singleton.Instance.CurrentMouse.Position);
    }
}