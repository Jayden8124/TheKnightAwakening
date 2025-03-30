using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Bullet : GameObject
    {
        public float DistanceMoved;
        private AnimationManager AnimationManager;
        private Dictionary<string, Animation> Animations;

        public Bullet(Texture2D texture) : base(texture)
        {
            IsActive = true;
        }

        public Bullet(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            if (Name is "Skill_medusa")
            {
                AnimationManager = new AnimationManager(Animations["Medusa"]);
            }
            if (Name is "BulletEnemy")
            {
                AnimationManager = new AnimationManager(Animations["Move"]);
            }
            AnimationManager = new AnimationManager(Animations["Move"]);
            IsActive = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                AnimationManager.Scale = 0.7f;
                AnimationManager.FacingRight = Velocity.X > 0;
                AnimationManager.Position = Position;
                AnimationManager.Draw(spriteBatch);
            }
            else
            {
                SpriteEffects spriteEffect = (Velocity.X > 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                spriteBatch.Draw(_texture, Position, Viewport, Color.White, Rotation, Vector2.Zero, 1f, spriteEffect, 0f);

                base.Draw(spriteBatch);
            }
        }

        public override void Reset()
        {
            DistanceMoved = 0;
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            float time = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            DistanceMoved += Math.Abs(Velocity.X * deltaTime);
            Position += Velocity * deltaTime;

            if (DistanceMoved > 2000 || DistanceMoved >= Singleton.SCREENHEIGHT || time > 15f)
            {
                time = 0f;
                IsActive = false;
                return;
            }

            foreach (GameObject s in _gameObjects)
            {
                if (Name == "BulletPlayer")
                {
                    if (CheckAABBCollision(s.Rectangle, Rectangle) && (s.Name.Equals("SKLT_WR") || s.Name.Equals("SL") || s.Name.Equals("SKLT_SM") || s.Name.Equals("SKLT_AC") || s.Name.Equals("MDS")))
                    {
                        s.IsActive = false;
                        if (s is MonsterType monster)
                        {
                            Singleton.Instance.Score += monster.Score;
                        }
                        IsActive = false;
                    }

                }
                else if (Name == "BulletEnemy")
                {
                    if (CheckAABBCollision(s.Rectangle, Rectangle) && s.Name == "Player")
                    {
                        IsActive = false;
                        Singleton.Instance.player.TakeDamage(10, Position);
                    }
                }
                else if (Name == "Skill_medusa")
                {
                    if (CheckAABBCollision(s.Rectangle, Rectangle) && s.Name == "Player")
                    {
                        IsActive = false;
                        Singleton.Instance.player.TakeDamage(10, Position);
                    }
                    if (gameTime.ElapsedGameTime.TotalSeconds > 5)
                    {
                        IsActive = false;
                    }
                }
            }

            if (_texture == null)
            {
                if (Name is "Skill_medusa")
                {
                    AnimationManager.Play(Animations["Medusa"]);
                }
                if (Name is "BulletEnemy")
                {
                    AnimationManager.Play(Animations["Move"]);
                }
                AnimationManager.Update(gameTime);
            }

            base.Update(gameTime, _gameObjects);
        }
    }
}
