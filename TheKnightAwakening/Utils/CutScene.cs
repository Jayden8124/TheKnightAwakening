
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Text;

namespace TheKnightAwakening
{
    public class CutScene
    {
        private List<string> messages;
        private Dictionary<int, Texture2D> backgrounds;
        private int currentMessageIndex;
        private string displayedText;
        private float textSpeed;
        private float textTimer;
        private int charIndex;
        private bool isTextFullyDisplayed;
        private const int frameWidth = 630;
        private GraphicsDevice _graphicsDevice;
        private SpriteFont _font;
        private Texture2D _frame;
        private Texture2D _scene1, _scene2, _scene3, _scene4, _scene5, _scene6;
        private Texture2D _btnYes, _btnNo;

        // สำหรับระบบ choice แบบปุ่ม
        Rectangle btnKill;
        Rectangle btnSpare;
        private bool isChoiceActive;

        // ตัวแปรสำหรับ End Credits (เลื่อนข้อความ)
        private float creditsYPosition = 600; // เริ่มต้นจากด้านล่างของหน้าจอ

        public void ResetGame()
        {
            LoadSceneData();
        }

        public CutScene(GraphicsDevice graphicsDevice)
        {
            this._graphicsDevice = graphicsDevice;
            Singleton.Instance.currentCutscene = Singleton.CutsceneType.StartGame; // ค่าเริ่มต้นสำหรับทดลอง (คุณสามารถเปลี่ยนได้)
            messages = new List<string>();
            backgrounds = new Dictionary<int, Texture2D>();

            currentMessageIndex = 0;
            textSpeed = 0.05f;
            charIndex = 0;
            isTextFullyDisplayed = false;
            displayedText = "";

            // กำหนดตำแหน่งและขนาดปุ่มสำหรับการเลือก (Execute และ Spare)
            btnKill = new Rectangle(425, 595, 202, 63);
            btnSpare = new Rectangle(700, 595, 202, 63);
            isChoiceActive = false;
        }

        public void LoadContent(ContentManager Content)
        {
            _font = Content.Load<SpriteFont>("game_font");
            _frame = Content.Load<Texture2D>("message_box");
            _scene1 = Content.Load<Texture2D>("Scene1");
            _scene2 = Content.Load<Texture2D>("Scene2");
            _scene3 = Content.Load<Texture2D>("Scene3");
            _scene4 = Content.Load<Texture2D>("Scene4");
            _scene5 = Content.Load<Texture2D>("Scene5");
            _scene6 = Content.Load<Texture2D>("Scene6");

            _btnYes = Content.Load<Texture2D>("Button Ui");
            _btnNo = Content.Load<Texture2D>("Button Ui");
        }

        public void LoadSceneData()
        {
            backgrounds.Clear();
            messages.Clear();

            currentMessageIndex = 0;
            displayedText = "";
            charIndex = 0;
            isTextFullyDisplayed = false;
            isChoiceActive = false;

            // ถ้ากำลังโหลด EndCredits ให้รีเซ็ตตำแหน่งเครดิตใหม่
            if (Singleton.Instance.currentCutscene == Singleton.CutsceneType.EndCredits)
            {
                creditsYPosition = 600;
            }

            switch (Singleton.Instance.currentCutscene)
            {
                case Singleton.CutsceneType.StartGame:
                    messages.Add("King: As you can see, Her curse now coils around our kingdom like a venomous serpent. The plague spreads unchecked, and our people perish by the day. I can no longer remain idle.");
                    backgrounds.Add(0, _scene1);

                    messages.Add("King: I bid thee, brave knight, venture forth to the Cursed Cavern where the foul creature hides. Medusa, the architect of this dark sorcery... end her reign of terror.");
                    backgrounds.Add(1, _scene1);

                    messages.Add("Knight: By your command, Your Majesty. I shall ride into the heart of darkness, and I shall not return until the curse is broken and Medusa is no more!");
                    backgrounds.Add(2, _scene2);
                    break;

                case Singleton.CutsceneType.BossRoom:
                    Singleton.Instance.AudioManager.PlayMusic("Boss_Bgm", 0.3f);
                    messages.Add("Medusa: So... the king sends a pawn to sever the chains of my curse.");
                    backgrounds.Add(0, _scene4);

                    messages.Add("Medusa: And yet.. you have made it this far. Impressive. Few ever do.");
                    backgrounds.Add(1, _scene4);

                    messages.Add("Medusa: But this... is where your tale ends. None escape the web of fate I have spun!");
                    backgrounds.Add(2, _scene4);

                    messages.Add("Knight: Even if it costs me my life, I will strike you down. No power you wield shall sway me. I will not allow your blight to claim another soul! There is no place in this realm for a heartless fiend like you!");
                    backgrounds.Add(3, _scene4);

                    messages.Add("Medusa: A fiend, you say? Hah... I have worn that title like a crown. I am the bringer of this calamity, yes but know this: every curse is born of sorrow, every monster made, not born.");
                    backgrounds.Add(4, _scene4);

                    messages.Add("Medusa: Come then, hero. Let fate judge us both!");
                    backgrounds.Add(5, _scene4);
                    break;

                case Singleton.CutsceneType.BossDefeated:
                    messages.Add("Medusa: I... I only wanted him to feel my pain.");
                    backgrounds.Add(0, _scene5);

                    messages.Add("Knight: Him? Who do you speak of?");
                    backgrounds.Add(1, _scene5);

                    messages.Add("Medusa: Your king! He betrayed me... he deceived me.");
                    backgrounds.Add(2, _scene5);

                    messages.Add("Medusa: I was once but a simple girl... a loyal handmaid devoted to this very kingdom. But it was your king who laid his eyes upon me, who crossed boundaries best left untouched! And the queen... she cast me aside, offered no hand, no mercy only a curse.");
                    backgrounds.Add(3, _scene5);

                    messages.Add("Medusa: She turned me into this monster.");
                    backgrounds.Add(4, _scene5);

                    messages.Add("Medusa: I became what they feared most. Hunted. Exiled. Cast into shadows like a plague.");
                    backgrounds.Add(5, _scene5);

                    messages.Add("Medusa: So let them feel it let them taste the dread they sowed! Let fear consume them as it consumed me!");
                    backgrounds.Add(6, _scene5);

                    messages.Add("Medusa: But in the end... I was undone. Brought low by the very blade that sought to end me. Your blade.");
                    backgrounds.Add(7, _scene5);

                    messages.Add("Knight: My king is not the man you speak of. He is a noble ruler a beacon of strength and honor. He would never commit such vile acts.");
                    backgrounds.Add(8, _scene5);

                    messages.Add("Medusa: And yet... do you truly believe I would curse the world without cause?");
                    backgrounds.Add(9, _scene5);

                    messages.Add("           Do you want to kill or spare Medusa?");
                    backgrounds.Add(10, _scene5);
                    break;

                case Singleton.CutsceneType.BossKilled:
                    messages.Add("Knight: No matter the truth... you are still the source of this calamity.");
                    backgrounds.Add(0, _scene3);

                    messages.Add("Knight: What you have done cannot be forgiven.");
                    backgrounds.Add(1, _scene3);

                    messages.Add("Medusa: Hmph... Then go on. Be their blade, their obedient tool.");
                    backgrounds.Add(2, _scene3);

                    messages.Add("Medusa: Strike me down, O knight of the kingdom you so proudly serve!");
                    backgrounds.Add(3, _scene3);

                    messages.Add("At last, Medusa falls... and with her final breath, the curse is lifted. But there is no triumph in the heart of knight is no joy in the silence that follows. She stands alone amidst the stillness... a single question echoing in her soul.");
                    backgrounds.Add(4, _scene3);

                    messages.Add("Was this truly the right thing to do...?");
                    backgrounds.Add(5, _scene3);
                    break;

                case Singleton.CutsceneType.BossSpared:
                    messages.Add("Knight: I do not see a monster before me... only a woman who has suffered under a cruel curse.");
                    backgrounds.Add(0, _scene6);

                    messages.Add("Knight: And I choose... not to strike you down.");
                    backgrounds.Add(1, _scene6);

                    messages.Add("Medusa: You are a strange knight indeed...");
                    backgrounds.Add(2, _scene6);

                    messages.Add("Medusa: Do you truly believe the curse will fade so easily... simply because you spared me?");
                    backgrounds.Add(3, _scene6);

                    messages.Add("Knight: It is not because I spared you...");
                    backgrounds.Add(4, _scene6);

                    messages.Add("Knight: It is because I believe you still have a choice. You are not bound by pain forever. You can decide what your life becomes from this moment onward.");
                    backgrounds.Add(5, _scene6);

                    messages.Add("Knight: I will not judge you. I only hope... that you find a way to release yourself from this sorrow.");
                    backgrounds.Add(6, _scene6);

                    messages.Add("Medusa: ...Thank you. For still believing... that I am human.");
                    backgrounds.Add(7, _scene6);

                    messages.Add("Medusa: For still believing... that I am human.");
                    backgrounds.Add(8, _scene6);
                    break;

                case Singleton.CutsceneType.EndCredits:
                    messages.Add("Thank you for playing 'The Knight Awakening'");
                    messages.Add("A game developed by PDream Group.");
                    messages.Add("Lead Programmers: Supanut Petsuparada & Pakkhapol Saekow");
                    messages.Add("Story & Narrative: Phatthanan Fungfuengworachai");
                    messages.Add("Map Creator: Phatthanan Fungfuengworachai");
                    messages.Add("Art Director & Game Design: Visanu Ittihisathainkun");
                    messages.Add("Music & Sound: Chanuwat Bumraung");
                    messages.Add("Special Thanks: Dr. Witchaya Towongpaichayont");
                    messages.Add("This game was created using MonoGame.");

                    messages.Add("");
                    messages.Add("=== References ===");

                    messages.Add("");
                    messages.Add("Sound Effects & Background Music");
                    messages.Add("Victory Sound: YouTube (https://youtu.be/OyTDjr5gpE0)");
                    messages.Add("Medusa Scream: YouTube (https://youtu.be/Sce0rw3Fpic)");
                    messages.Add("Background Music (BGM): YouTube (https://youtu.be/L_z7TBLzfEM)");
                    messages.Add("Boss Theme: YouTube (https://youtu.be/qSYKXBjl6Ro?t=75s)");
                    messages.Add("Sword SFX: YouTube (https://youtu.be/wURp8_kO2m8)");
                    messages.Add("Slime Death SFX: YouTube (https://youtu.be/ov7wyBksD_4)");
                    messages.Add("Player Death SFX: YouTube (https://youtu.be/6EoDjM5C0OA)");
                    messages.Add("Skeleton Death SFX: YouTube (https://youtu.be/Uim--YrmGyM)");
                    messages.Add("Item Pickup: YouTube (https://youtu.be/SoZhpnTuQBo)");
                    messages.Add("Coin Pickup: YouTube (https://youtu.be/mQSmVZU5EL4)");
                    messages.Add("Medusa Skill: YouTube (https://youtu.be/A6QWVGgWAmc)");

                    messages.Add("");
                    messages.Add("Sprite Art & Visual Assets:");
                    messages.Add("Cave Background: Terraria Wiki (https://terraria.fandom.com/wiki/Cavern)");
                    messages.Add("Moon Night Background: CraftPix (https://craftpix.net/freebies/free-moon-pixel-game-backgrounds/)");
                    messages.Add("Characters & Monsters: CraftPix (https://craftpix.net/categorys/monster-sprites/)");
                    messages.Add("Village Tileset: Unity Asset Store (https://assetstore.unity.com/packages/2d/environments)");
                    messages.Add("Cave Tileset: Itch.io (https://silvdragoon-d.itch.io/cave-tileset)");
                    messages.Add("Icons & Skills: Dribbble (https://dribbble.com)");
                    messages.Add("Background: VectorStock (https://www.vectorstock.com/royalty-free-vectors/night-sky-pixel-vectors)");
                    messages.Add("Cutscene Assets: ChatGPT (https://chatgpt.com)");

                    messages.Add("");
                    messages.Add("We hope you enjoyed the adventure.");
                    messages.Add("Press Space to return to the main menu.");

                    backgrounds.Add(0, _scene1);
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (backgrounds.Count == 0)
            {
                LoadSceneData();
                return;
            }

            // หากกำลังแสดงตัวเลือก (choice) ให้ตรวจสอบการคลิกปุ่ม
            if (isChoiceActive)
            {
                if (IsButtonClicked(btnKill))
                {
                    Singleton.Instance.currentCutscene = Singleton.CutsceneType.BossKilled;
                    LoadSceneData();
                    return;
                }
                else if (IsButtonClicked(btnSpare))
                {
                    Singleton.Instance.currentCutscene = Singleton.CutsceneType.BossSpared;
                    LoadSceneData();
                    return;
                }
            }

            switch (Singleton.Instance.currentCutscene)
            {
                case Singleton.CutsceneType.EndCredits:
                    UpdateCredits(gameTime);

                    // เมื่อข้อความเลื่อนขึ้นจนหมดหน้าจอแล้ว เปลี่ยนสถานะเกมกลับไปที่เมนูหลัก
                    if (creditsYPosition < -messages.Count * 40)
                    {
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
                        Singleton.Instance.currentCutscene = Singleton.CutsceneType.StartGame;

                        // รีเซ็ตเกมเพื่อให้เริ่มใหม่ได้
                        ResetGame();

                        backgrounds.Clear();
                        messages.Clear();
                        LoadSceneData();
                    }

                    // หรือกดปุ่ม Space เพื่อข้าม credits
                    if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    {
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GameStart;
                        Singleton.Instance.currentCutscene = Singleton.CutsceneType.StartGame;

                        // รีเซ็ตเกมเพื่อให้เริ่มใหม่ได้
                        ResetGame();

                        backgrounds.Clear();
                        messages.Clear();
                        LoadSceneData();
                    }
                    break;
                case Singleton.CutsceneType.StartGame:
                    // ในกรณีของ cutscene เริ่มเกมให้ปรับสถานะว่าได้เล่น cutscene แล้ว
                    UpdateRegularCutscene(gameTime);
                    break;

                default:
                    UpdateRegularCutscene(gameTime);
                    break;
            }

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
        }

        // แยกโค้ดการอัปเดต cutscene ทั่วไปออกมาเป็นเมทอดใหม่
        private void UpdateRegularCutscene(GameTime gameTime)
        {
            // Update การแสดงข้อความแบบตัวอักษรทีละตัว
            if (!isTextFullyDisplayed)
            {
                textTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textTimer >= textSpeed && charIndex < messages[currentMessageIndex].Length)
                {
                    displayedText += messages[currentMessageIndex][charIndex];
                    charIndex++;
                    textTimer = 0f;
                }

                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    displayedText = messages[currentMessageIndex];
                    charIndex = messages[currentMessageIndex].Length;
                    isTextFullyDisplayed = true;
                }
            }
            else if (isTextFullyDisplayed && Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
            {
                if (currentMessageIndex < messages.Count - 1)
                {
                    currentMessageIndex++;
                    displayedText = "";
                    charIndex = 0;
                    isTextFullyDisplayed = false;
                }
                else
                {
                    // จบ cutscene - ตรวจสอบประเภทของ cutscene ปัจจุบันและดำเนินการต่อ
                    switch (Singleton.Instance.currentCutscene)
                    {
                        case Singleton.CutsceneType.BossDefeated:
                            isChoiceActive = true;
                            break;

                        case Singleton.CutsceneType.BossSpared:
                        case Singleton.CutsceneType.BossKilled:
                            Singleton.Instance.currentCutscene = Singleton.CutsceneType.EndCredits;
                            backgrounds.Clear();
                            messages.Clear();
                            LoadSceneData();

                            break;

                        case Singleton.CutsceneType.StartGame:
                            backgrounds.Clear();
                            messages.Clear();
                            Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                            break;

                        default:
                            backgrounds.Clear();
                            messages.Clear();
                            Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                            break;
                    }
                }
            }

            // ตรวจสอบตัวอักษรที่แสดง (สำหรับ Debug)
            foreach (char c in displayedText)
            {
                if (!_font.Characters.Contains(c))
                {
                    Console.WriteLine($"Missing char: {c} ({(int)c})");
                }
            }
        }

        // อัพเดทการแสดงเครดิต
        private void UpdateCredits(GameTime gameTime)
        {
            // ลดค่า creditsYPosition ให้ข้อความเลื่อนขึ้น
            creditsYPosition -= (float)gameTime.ElapsedGameTime.TotalSeconds * 50;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            // ถ้าไม่ใช่ EndCredits ให้วาด cutscene ปกติ
            if (Singleton.Instance.currentCutscene != Singleton.CutsceneType.EndCredits)
            {
                if (backgrounds.Count == 0)
                {
                    return;
                }
                _spriteBatch.Begin();
                _spriteBatch.Draw(backgrounds[currentMessageIndex], new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
                _spriteBatch.Draw(_frame, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
                _spriteBatch.DrawString(_font, WrapText(displayedText, frameWidth), new Vector2(320, 540), Color.White);

                if (isChoiceActive)
                {
                    _spriteBatch.Draw(_btnYes, btnKill, new Rectangle(683, 332, 594, 281), Color.White);
                    _spriteBatch.Draw(_btnNo, btnSpare, new Rectangle(36, 645, 595, 287), Color.White);
                }
                _spriteBatch.End();
            }
            else
            {
                // ถ้าเป็น EndCredits ให้วาดเครดิตที่เลื่อนขึ้น
                DrawCredits(_spriteBatch);
            }
        }
        // เมธอดสำหรับ Draw End Credits
        // public void DrawCredits(SpriteBatch _spriteBatch)
        // {
        //     _spriteBatch.GraphicsDevice.Clear(Color.Black);
        //     _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //     for (int i = 0; i < messages.Count; i++)
        //     {
        //         // วาดแต่ละบรรทัด โดยตำแหน่ง Y คำนวณจาก creditsYPosition + i * 40
        //         _spriteBatch.DrawString(_font, messages[i], new Vector2(320, creditsYPosition + i * 40), Color.White);
        //     }
        //     _spriteBatch.End();
        // }

        public void DrawCredits(SpriteBatch _spriteBatch)
        {
            _spriteBatch.GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            int padding = 20; // Additional vertical padding between lines
            int lineHeight = (int)_font.MeasureString("A").Y + padding;

            for (int i = 0; i < messages.Count; i++)
            {
                string line = messages[i];
                Vector2 size = _font.MeasureString(line);

                // Center horizontally
                float x = (Singleton.SCREENWIDTH - size.X) / 2;
                float y = creditsYPosition + i * lineHeight;

                _spriteBatch.DrawString(_font, line, new Vector2(x, y), Color.White);
            }

            _spriteBatch.End();
        }
        private string WrapText(string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder wrappedText = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = _font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = _font.MeasureString(word);
                if (lineWidth + size.X < maxLineWidth)
                {
                    wrappedText.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    wrappedText.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
            return wrappedText.ToString();
        }

        private bool IsButtonClicked(Rectangle buttonRect)
        {
            return Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                   Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released &&
                   buttonRect.Contains(Singleton.Instance.CurrentMouse.Position);
        }
    }
}