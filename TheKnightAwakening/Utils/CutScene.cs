using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;

namespace TheKnightAwakening
{
    public class CutScene
    {
        private List<string> messages;
        private Dictionary<int, Texture2D> backgrounds;
        private int currentMessageIndex = 0;
        private string displayedText = "";
        private float textSpeed = 0.05f;
        private float textTimer = 0f;
        private int charIndex = 0;
        private bool isTextFullyDisplayed = false;
        private const int frameWidth = 851;
        private const int frameHeight = 148;
        private GraphicsDevice _graphicsDevice;
        private SpriteFont _font;
        private Texture2D _frame;
        public enum CutsceneType
        {
            StartGame,
            BossRoom,
            BossDefeated
        }
        private CutsceneType currentCutscene;

        public CutScene(GraphicsDevice graphicsDevice)
        {
            this._graphicsDevice = graphicsDevice;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            _font = Content.Load<SpriteFont>("game_font");
            _frame = Content.Load<Texture2D>("message_box");
            currentCutscene = CutsceneType.StartGame;
            LoadSceneData(Content);
        }

        private void LoadSceneData(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            messages = new List<string>();
            backgrounds = new Dictionary<int, Texture2D>();

            switch (currentCutscene)
            {
                case CutsceneType.StartGame:
                    messages.Add("King: My knight, Medusa's curse has spread across our kingdom...");
                    backgrounds.Add(0, Content.Load<Texture2D>("cutscene1"));

                    messages.Add("King: You must vanquish her and end this catastrophe!");
                    backgrounds.Add(1, Content.Load<Texture2D>("cutscene1"));

                    messages.Add("Knight: Your Majesty, I shall obey your command...");
                    backgrounds.Add(2, Content.Load<Texture2D>("cutscene2"));
                    break;

                case CutsceneType.BossRoom:
                    messages.Add("Medusa: Did he send you to 'free' the kingdom from my curse?");
                    backgrounds.Add(0, Content.Load<Texture2D>("cutscene1"));

                    messages.Add("Medusa: This shall be your end!");
                    backgrounds.Add(1, Content.Load<Texture2D>("cutscene1"));

                    messages.Add("Knight: Even if it costs me my life, I will stop you!");
                    backgrounds.Add(2, Content.Load<Texture2D>("cutscene1"));
                    break;

                case CutsceneType.BossDefeated:
                    messages.Add("Medusa: I... I only wanted him to feel my pain...");
                    backgrounds.Add(0, Content.Load<Texture2D>("cutscene1"));

                    messages.Add("Knight: Who do you mean?");
                    backgrounds.Add(1, Content.Load<Texture2D>("cutscene1"));

                    messages.Add("Medusa: Your king! He betrayed me...");
                    backgrounds.Add(2, Content.Load<Texture2D>("cutscene1"));
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
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
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
            }

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgrounds[currentMessageIndex], new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
            spriteBatch.Draw(_frame, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);
            spriteBatch.DrawString(_font, WrapText(displayedText, frameWidth), new Vector2(240, 570), Color.White);
            spriteBatch.End();
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
    }
}
