using System;
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
    private Dictionary<string, MonsterType> monsterPrototypes;


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
        monsterPrototypes = new();

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
                    // if (!_cutscene.hasCutsceneBeenPlayed || _cutscene.hasGameBeenCompleted)
                    // {
                    //     Singleton.Instance.CurrentGameState = Singleton.GameState.Cutscene;
                    //     Singleton.Instance.currentCutscene = Singleton.CutsceneType.StartGame;
                    // }
                    // else
                    // {
                    //     Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                    // }
                        // ทุกครั้งที่กด Play ให้เริ่มใหม่เสมอ
                    Reset();
                    _cutscene.ResetGame(); // reset cutscene flags ด้วย (hasCutsceneBeenPlayed / hasGameBeenCompleted)

                    Singleton.Instance.CurrentGameState = Singleton.GameState.Cutscene;
                    Singleton.Instance.currentCutscene = Singleton.CutsceneType.StartGame;

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
                // Audio
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.M) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    if (Singleton.Instance.AudioManager.IsMuted())
                    {
                        Singleton.Instance.AudioManager.UnmuteAll();
                    }
                    else
                    {
                        Singleton.Instance.AudioManager.MuteAll();
                    }
                }
                // Update Camera
                if (Singleton.Instance.player != null)
                {
                    _camera.Follow(Singleton.Instance.player);
                }

                // Update GameObjects               
                for (int i = 0; i < _numOjects; i++)
                {
                    if (_camera.IsVisible(_gameObjects[i].Rectangle))
                    {
                        if (_gameObjects[i].IsActive)
                        {
                            _gameObjects[i].Update(gameTime, _gameObjects);
                        }
                    }
                }

                for (int i = 0; i < _numOjects; i++)
                {
                    if (_camera.IsVisible(_gameObjects[i].Rectangle))
                    {
                        if (!_gameObjects[i].IsActive)
                        {
                            _gameObjects.RemoveAt(i);
                            i--;
                            _numOjects--;
                        }
                    }
                }

                _numOjects = _gameObjects.Count;


                // ตรวจสอบและแก้ไข collision สำหรับ Monster แต่ละตัว for Collision   
                foreach (var obj in _gameObjects)
                {
                    if (_camera.IsVisible(obj.Rectangle))
                    {
                        CollisionManager.ResolveCollision(obj, Singleton.Instance.HitblockTiles);
                        CollisionManager.UpdateOnGround(obj, Singleton.Instance.HitblockTiles);

                    }
                }

                // หลังจากที่ทำ Collision กับ tile map แล้ว
                foreach (var obj in _gameObjects)
                {
                    if (_camera.IsVisible(obj.Rectangle))
                    {

                        if (obj is MonsterType monster)
                        {
                            if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, monster.Rectangle))
                            {
                                CollisionManager.ResolveCharacterCollision(Singleton.Instance.player, monster);

                                if (!(monster is SKLT_WR))
                                {
                                    // Singleton.Instance.player.TakeDamage(1, obj.Position);
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < _gameObjects.Count; i++)
                {
                    if (_gameObjects[i] is Flag flag && !flag.Checked)
                    {
                        if (flag.Name == "Flag_{X:5940 Y:71}")
                        {
                            if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, flag.Rectangle))
                            {
                                flag.Collected();

                                Singleton.Instance.CurrentSection = 1;
                                SpawnMonstersForSection(Singleton.Instance.CurrentSection);
                            }
                        }
                        if (flag.Name == "Flag_{X:10400 Y:164}")
                        {
                            if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, flag.Rectangle))
                            {
                                flag.Collected();

                                Singleton.Instance.CurrentSection = 2;
                                SpawnMonstersForSection(Singleton.Instance.CurrentSection);
                            }
                        }
                        if (flag.Name == "Flag_{X:4097 Y:932}")
                        {
                            if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, flag.Rectangle))
                            {
                                flag.Collected();

                                Singleton.Instance.CurrentSection = 3;
                                SpawnMonstersForSection(Singleton.Instance.CurrentSection);
                            }
                        }

                        if (flag.Name == "Flag_{X:3070 Y:1509}")
                        {
                            if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, flag.Rectangle))
                            {
                                flag.Collected();

                                Singleton.Instance.CurrentSection = 4;
                                SpawnMonstersForSection(Singleton.Instance.CurrentSection);
                            }
                        }
                        if (flag.Name == "Flag_{X:7971 Y:3013}")
                        {
                            if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, flag.Rectangle))
                            {
                                flag.Collected();

                                Singleton.Instance.CurrentSection = 5;
                                SpawnMonstersForSection(Singleton.Instance.CurrentSection);
                            }
                        }
                        if (flag.Name == "Flag_{X:4000 Y:3847}")
                        {
                            if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, flag.Rectangle))
                            {
                                flag.Collected();

                                Singleton.Instance.currentCutscene = Singleton.CutsceneType.BossRoom;
                                _cutscene.LoadSceneData();
                                Singleton.Instance.CurrentGameState = Singleton.GameState.Cutscene;

                                Singleton.Instance.player.Position = new Vector2(4200, 3847);
                                Singleton.Instance.player.LastCheckpoint = new Vector2(4200, 3847);
                                Singleton.Instance.CurrentSection = 6;
                                SpawnMonstersForSection(Singleton.Instance.CurrentSection);
                            }
                        }
                    }
                }

                // Paremeters: Health, Ultimate
                _drawing.Healthbar.Update(Singleton.Instance.player.Health);
                _drawing.HealthbarAnimated.Update(Singleton.Instance.player.Health, gameTime);

                _drawing.Ultimatebar.Update(Singleton.Instance.player.Ultimate);
                _drawing.UltimatebarAnimated.Update(Singleton.Instance.player.Ultimate);

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
                }
                else if (IsButtonClicked(_drawing.Setting))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
                else if (IsButtonClicked(_drawing.SettingsButtonDOWN))
                {
                    float currentVolume = Singleton.Instance.AudioManager.GetCurrentVolume();
                    Singleton.Instance.AudioManager.SetVolume(MathHelper.Clamp(currentVolume - 0.25f, 0f, 2f));
                }
                else if (IsButtonClicked(_drawing.SettingsButtonUP))
                {
                    float newVolume = MathHelper.Clamp(Singleton.Instance.AudioManager.GetCurrentVolume() + 0.25f, 0f, 2f);
                    Singleton.Instance.AudioManager.SetVolume(newVolume);
                }
                else if (IsButtonClicked(_drawing.SettingsButtonMute))
                {
                    if (Singleton.Instance.AudioManager.IsMuted())
                    {
                        Singleton.Instance.AudioManager.UnmuteAll();
                    }
                    else
                    {
                        Singleton.Instance.AudioManager.MuteAll();
                    }
                }
                break;

            case Singleton.GameState.GameOver:
                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed && Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                {
                    MediaPlayer.Stop();
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
                    Reset();
                }
                break;
        }

        Console.WriteLine("Count Game Objects : " + _numOjects);
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
                    _drawing.DrawVolumeBar(_spriteBatch);
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
        MonsterProtype(); // Call Monster Object
        ResetObject(); // Call Object

        Singleton.Instance.AudioManager.PlayMusic("Bgm", 0.5f);

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
            // LastCheckpoint = new Vector2(720, 470),
            // LastCheckpoint = new Vector2(4097, 932),
            // LastCheckpoint = new Vector2(5490, 71),
            LastCheckpoint = new Vector2(3800, 3847),
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

    public void MonsterProtype()
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
        Texture2D skillmedusa = Content.Load<Texture2D>("Fire_ball");
        var _animationsBullet = AnimationUlitmate.LoadAnimations(skillmedusa);


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
            Score = 50,
            Viewport = new Rectangle(0, 0, 65, 77),
            Position = new Vector2(6740, 4049),
            bullet = new Bullet(_animationsBullet)
            {
                Name = "Skill_medusa",
                Viewport = new Rectangle(384, 65, 45, 3)
            }
        };

        monsterPrototypes["SKLT_AC"] = prototypeAC;
        monsterPrototypes["SKLT_SM"] = prototypeSM;
        monsterPrototypes["SKLT_WR"] = prototypeWR;
        monsterPrototypes["SL"] = prototypeSL;
        monsterPrototypes["MDS"] = prototypeMDS;


        // foreach (var pos in spawnPositionsMDS)
        // {
        //     var clone = prototypeMDS.Clone();
        //     clone.Position = pos;
        //     _gameObjects.Add(clone);
        // }
    }

    private void SpawnMonstersForSection(int section)
    {
        // ลบมอนสเตอร์ของ section ก่อนหน้าออก
        // _gameObjects.RemoveAll(obj =>
        //     obj is MonsterType monster &&
        //     monster.Section == Singleton.Instance.PreviousSection);

        // Spawn มอนสเตอร์ใหม่ของทุกประเภท
        foreach (var pair in monsterPrototypes)
        {
            string type = pair.Key;
            MonsterType prototype = pair.Value;

            Dictionary<int, List<Vector2>> spawnPositions = type switch
            {
                "SKLT_AC" => SKLT_AC.SpawnPositions,
                "SKLT_SM" => SKLT_SM.SpawnPositions,
                "SKLT_WR" => SKLT_WR.SpawnPositions,
                "SL" => SL.SpawnPositions,
                "MDS" => MDS.SpawnPositions,
                _ => null
            };

            if (spawnPositions != null && spawnPositions.TryGetValue(section, out var positions))
            {
                foreach (var pos in positions)
                {
                    MonsterType clone = prototype.Clone();
                    clone.Position = pos;
                    clone.Section = section; // กำหนด Section ให้ตัว clone
                    _gameObjects.Add(clone);
                }
            }
        }

        Singleton.Instance.PreviousSection = Singleton.Instance.CurrentSection;
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