using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Bullet : GameObject
    {
        public float DistanceMoved;

        public Bullet(Texture2D texture) : base(texture)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = (Velocity.X > 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(_texture, Position, Viewport, Color.White, 0f, Vector2.Zero, 1f, spriteEffect, 0f);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            DistanceMoved = 0;
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            DistanceMoved += Math.Abs(Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond);
            Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            if (DistanceMoved > 2000)
            {
                IsActive = false;
            }

            if (DistanceMoved >= Singleton.SCREENHEIGHT)
            {
                IsActive = false;
            }

            foreach (GameObject s in _gameObjects)
            {
                // if (Name.Equals("BulletPlayer"))
                // {
                //     if (IsTouching(s) && s.Name.Equals("Enemy") || s.Name.Equals("BulletEnemy"))
                //     {
                //         s.IsActive = false;
                //         if (s is Enemy)
                //         {
                //             Singleton.Instance.Score += (s as Enemy).Score;
                //             Singleton.Instance.InvaderLeft--;
                //         }
                //         IsActive = false;
                //     }
                // }
                if (Name.Equals("BulletEnemy"))
                {
                    if (CheckAABBCollision(s.Rectangle, Rectangle) && s.Name.Equals("Player"))
                    {
                        IsActive = false;
                        Singleton.Instance.player.TakeDamage(10, Position);
                    }
                }
            }
            base.Update(gameTime, _gameObjects);
        }
    }
}