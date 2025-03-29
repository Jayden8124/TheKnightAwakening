using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheKnightAwakening;

public class MainScene : Game
{
    // Graphics
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // GameObjects
    List<GameObject> _gameObjects;
    public int _numOjects;

    // Camera & Map & Cutscene & Drawing
    private Camera _camera;
    private Map _map;
    private CutScene _cutscene;
    private Drawing _drawing;

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

        _camera = new Camera(GraphicsDevice.Viewport);
        _map = new Map(GraphicsDevice);
        _cutscene = new CutScene(GraphicsDevice);
        _drawing = new Drawing(GraphicsDevice, _camera, _map, _cutscene);

        Singleton.Instance.AudioManager = new Audio();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load Sound for Class Audio 
        Singleton.Instance.AudioManager.LoadSounds(Content);
        Singleton.Instance.AudioManager.PlayMusic("Bgm", 0.5f);

        // Drawing
        _drawing.LoadContent(Content);
        
        Reset(); // Call Reset
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        Singleton.Instance.CurrentMouse = Mouse.GetState();

        _numOjects = _gameObjects.Count;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.GameStart:
                if (IsButtonClicked(_drawing.MenuPlay))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Cutscene;
                }
                else if (IsButtonClicked(_drawing.MenuExit))
                {
                    Exit();
                }
                break;

            case Singleton.GameState.Cutscene:
                _cutscene.Update(gameTime);
                break;

            case Singleton.GameState.GamePlaying:
                // Pause Game
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePaused;

                if (IsButtonClicked(_drawing.Setting))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePaused;
                }

                // Update GameObjects               
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

                // Update Camera
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

                // Paremeters: Health, Ultimate
                _drawing.Healthbar.Update(Singleton.Instance.player.Health);
                _drawing.HealthbarAnimated.Update(Singleton.Instance.player.Health, gameTime);

                _drawing.Ultimatebar.Update(Singleton.Instance.player.Ultimate);
                // _ultimatebarAnimated.Update(Singleton.Instance.player.Ultimate, gameTime);

                break;

            case Singleton.GameState.GamePaused: //Game Paused
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;

                if (IsButtonClicked(_drawing.PauseResume))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
                else if (IsButtonClicked(_drawing.PauseExit))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
                }else if ( IsButtonClicked(_drawing.Setting))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
                break;

            case Singleton.GameState.GameOver:
                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed && Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
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
            case Singleton.GameState.GameStart:
                {
                    _drawing._DrawStart(_spriteBatch);
                }
                break;
            case Singleton.GameState.Cutscene:
                {
                    _cutscene.Draw(_spriteBatch);
                }
                break;
            case Singleton.GameState.GamePlaying:
                {
                    _drawing._DrawPlaying(_spriteBatch, _gameObjects, _numOjects);
                }
                break;
            case Singleton.GameState.GamePaused:
                {
                    _drawing._DrawPause(_spriteBatch, _gameObjects, _numOjects);
                }
                break;
            case Singleton.GameState.GameOver:
                {
                    _drawing._DrawOver(_spriteBatch);
                }
                break;
        }

        _graphics.BeginDraw();

        base.Draw(gameTime);
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
        Texture2D bulletSheet = Content.Load<Texture2D>("Sword_Projectile");

        // Player Instance
        var _animationsPlayer = AnimationPlayer.LoadAnimations(knightSheet);
        var _animationsBullet = AnimationUlitmate.LoadAnimations(bulletSheet);

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
            Bullet = new Bullet(_animationsBullet)
            {
                Name = "BulletPlayer",
                Viewport = new Rectangle(25, 6, 54, 65),
            }
        };

        Singleton.Instance.player.LoadDebufIcons(Content);

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
            Score = 40,
            Viewport = new Rectangle(0, 0, 53, 65)
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
            Score = 40,
            Viewport = new Rectangle(0, 0, 30, 90)
        };

        MonsterType prototypeAC = new SKLT_AC(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SKLT_AC))
        {
            Name = "SKLT_AC",
            Score = 30,
            Viewport = new Rectangle(0, 0, 37, 68),
            bullet = new Bullet(monsterTextures[AnimationMonster.AnimationMonsterType.SKLT_AC])
            {
                Name = "BulletEnemy",
                Viewport = new Rectangle(384, 65, 45, 3)
            }
        };

        MonsterType prototypeMDS = new MDS(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.MDS))
        {
            Name = "MDS",
            Score = 500,
            Viewport = new Rectangle(0, 0, 68, 89),
            bullet = new Bullet(Content.Load<Texture2D>("skeleton_archer"))
            {
                Name = "BulletEnemy",
                Viewport = new Rectangle(384, 65, 45, 3)
            }
        };

        // // Postion each monster type
        List<Vector2> spawnPositionsSL = SL.SpawnPositions;
        List<Vector2> spawnPositionsWR = SKLT_WR.SpawnPositions;
        List<Vector2> spawnPositionsSM = SKLT_SM.SpawnPositions;
        List<Vector2> spawnPositionsAC = SKLT_AC.SpawnPositions;
        List<Vector2> spawnPositionsMDS = MDS.SpawnPositions;

        // // Clone prototype and Set Potion from spawnPositions each Monster Type
        // foreach (var pos in spawnPositionsSL)
        // {
        //     var clone = prototypeSL.Clone();
        //     clone.Position = pos;
        //     _gameObjects.Add(clone);
        // }

        foreach (var pos in spawnPositionsWR)
        {
            var clone = prototypeWR.Clone();
            clone.Position = pos;
            _gameObjects.Add(clone);
        }

        // foreach (var pos in spawnPositionsSM)
        // {
        //     var clone = prototypeSM.Clone();
        //     clone.Position = pos;
        //     _gameObjects.Add(clone);
        // }

        // foreach (var pos in spawnPositionsAC)
        // {
        //     var clone = prototypeAC.Clone();
        //     clone.Position = pos;
        //     _gameObjects.Add(clone);
        // }

        // foreach (var pos in spawnPositionsMDS)
        // {
        //     var clone = prototypeMDS.Clone();
        //     clone.Position = pos;
        //     _gameObjects.Add(clone);
        // }
    }


    public void ResetObject() // Clone 
    {
        // Chest Instance & Texture 
        var _animationsChest = AnimationChest.LoadAnimations(_drawing.ChestSheet);
        var goldChestAnimations = _animationsChest[ChestType.GoldChest];
        var _animationCoin = AnimationCoin.LoadAnimations(_drawing.CoinSheet);
        var _animationPotion = AnimationPotion.LoadAnimations(_drawing.PotionSheet);

        Chest prototypeChest = new Chest(goldChestAnimations)
        {
            Name = "Chest",
            openKey = Keys.E,
            coin = new Coin(_animationCoin)
            {
                Name = "Coin",
            },
            potion = new Potion(_animationPotion)
            {
                Name = "Potion",
            }
        };

        List<Vector2> spawnPositionsChest = Chest.SpawnChestPosition;

        foreach (var pos in spawnPositionsChest)
        {
            var clone = (Chest)prototypeChest.Clone();
            clone.Position = pos;
            _gameObjects.Add(clone);
        }


        // Flag Instance & Texture
        var _animationFlag = AnimationFlag.LoadAnimations(_drawing.FlagSheet);

        Flag prototypeFlag = new Flag(_animationFlag)
        {
            Name = "Flag",
            Viewport = new Rectangle(41, 20, 63, 172),
        };

        List<Vector2> spawnPositionsFlag = Flag.SpawnFlagPosition;

        foreach (var pos in spawnPositionsFlag)
        {
            var clone = (Flag)prototypeFlag.Clone();
            clone.Position = pos;
            clone.Name += "_" + pos.ToString();
            _gameObjects.Add(clone);
        }

    }

    private bool IsButtonClicked(Rectangle buttonRect)
    {
        return Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed && Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released && buttonRect.Contains(Singleton.Instance.CurrentMouse.Position);
    }
}