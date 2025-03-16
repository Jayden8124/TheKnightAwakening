using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheKnightAwakening
{
    public class AnimationManager
    {
        private Animation _animation;
        private float _timer;        // Tracks time between frames
        private int _currentFrame;   // Current frame index
        public Vector2 Position { get; set; }
        public float Scale { get; set; } = 1f;
        public bool FacingRight { get; set; } = true; // Default: facing right

        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        public void Play(Animation animation)
        {
            // If we’re already playing this animation, no need to reset
            if (_animation == animation)
                return; // Same animation, do nothing

            _animation = animation;
            _currentFrame = 0;
            _timer = 0f;
        }

        // Stop the current animation and reset to frame 0.
        public void Stop()
        {
            _timer = 0f;
            _currentFrame = 0;
        }


        // Update the current frame based on elapsed time.
        public void Update(GameTime gameTime)
        {
            if (_animation == null)
                return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move to the next frame if enough time has passed
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _currentFrame++;

                // If we've exceeded the last frame
                if (_currentFrame >= _animation.FrameCount)
                {
                    _currentFrame = _animation.IsLooping
                        ? 0
                        : _animation.FrameCount - 1;
                }
            }
        }

        // Draw the current frame to the screen.
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Rectangle source = _animation.Frames[_currentFrame];
            spriteBatch.Draw(_animation.Texture, Position, source, Color.White, 0f,Vector2.Zero, Scale, spriteEffect, 0f);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            SpriteEffects spriteEffect = FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Rectangle source = _animation.Frames[_currentFrame];
            // Vector2 drawPosition = Position + _animation.Offset;
             
            spriteBatch.Draw(_animation.Texture, Position, source, color, 0f, Vector2.Zero, Scale, spriteEffect, 0f);
            // วาดกรอบ hitbox ของเฟรม (debug)
            Texture2D rectTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.Red });
            spriteBatch.Draw(rectTexture, new Rectangle((int)Position.X, (int)Position.Y, source.Width, source.Height), Color.Red * 0.5f);

        }
    }
}
