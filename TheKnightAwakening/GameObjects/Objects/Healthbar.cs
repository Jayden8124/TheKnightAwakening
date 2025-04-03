using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheKnightAwakening
{
    public class Healthbar
    {
        // Properties
        protected Texture2D texture;
        protected Rectangle Background, Foreground;
        protected float maxHealth;
        protected float currentHealth;
        protected Rectangle currentHealthForeground; 

        // Animation Shade
        public HealthbarAnimated healthbarAnimated;

        public Healthbar(Texture2D texture, Rectangle Background, Rectangle Foreground, float maxHealth)
        {
            this.texture = texture;
            this.Background = Background;
            this.Foreground = Foreground;
            this.maxHealth = maxHealth;

            currentHealth = maxHealth;
            currentHealthForeground = new Rectangle(Foreground.X, Foreground.Y, Foreground.Width, Foreground.Height);
        }

        public void Update(float value)
        {
            currentHealthForeground.Width = (int)(value / maxHealth * Foreground.Width);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(50, 50), Background, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0); // Background
            spriteBatch.Draw(texture, new Vector2(76, 63), currentHealthForeground, Color.White, 0f, Vector2.Zero, 1.53f, SpriteEffects.None, 0); // Foreground
        }
    }

    public class HealthbarAnimated : Healthbar
    {
        private float _targetValue;
        private readonly float _animationSpeed = 10f;
        private Rectangle _animationSource;
        private Vector2 _animationPosition;
        private Color _animationShade;

        public HealthbarAnimated(Texture2D tex, Rectangle bgSource, Rectangle fgSource, float max) : base(tex, bgSource, fgSource, max)
        {
            _targetValue = max;
            _animationSource = new Rectangle(Foreground.X + Foreground.Width, Foreground.Y, 0, Foreground.Height); 
            _animationPosition = new Vector2(76, 63);
            _animationShade = Color.DarkGray;
        }

        public void Update(float value, GameTime gameTime)
        {
            if (value == currentHealth) 
            {
                _animationSource.Width = 0; 
                return;
            }

            _targetValue = value;
            int x;

            if (_targetValue < currentHealth)
            {
                currentHealth -= _animationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentHealth < _targetValue)
                    currentHealth = _targetValue;
                x = (int)(_targetValue / maxHealth * Foreground.Width);
                _animationShade = Color.Gray;
            }
            else
            {
                currentHealth += _animationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentHealth > _targetValue)
                    currentHealth = _targetValue;
                x = (int)(currentHealth / maxHealth * Foreground.Width);
                _animationShade = Color.DarkGray * 0.5f;
            }

            currentHealthForeground.Width = x; 
            _animationSource.X = Foreground.X + x; 
            _animationSource.Width = (int)(Math.Abs(currentHealth - _targetValue) / maxHealth * Foreground.Width);
            _animationPosition.X = 76 + currentHealthForeground.Width * 1.53f; 
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(texture, _animationPosition, _animationSource, _animationShade, 0f, Vector2.Zero, 1.53f, SpriteEffects.None, 0);
        }
    }
}