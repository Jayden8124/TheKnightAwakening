using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class SKLT_AC : MonsterType
    {
        public Bullet bullet;

        public SKLT_AC(Texture2D texture) : base(texture)
        {
        }

        public SKLT_AC(Dictionary<string, Animation> animations) : base(animations)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            if (Health <= 0)
            {
                base.Update(gameTime, _gameObjects);
                return;
            }

            if (gameTime.TotalGameTime.TotalSeconds > 1)
            {
                if (ShouldPerformMeleeAttack())
                {
                    HandleMeleeAttack(gameTime, _gameObjects);
                }
                else
                {
                    attackTimer = 0f;
                    // HandleMovement();
                    AnimationManager.Play(Animations["Idle"]);
                }
            }

            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }
        private bool ShouldPerformMeleeAttack()
        {
            const int meleeDistanceLimit = 500;
            const int verticalTolerance = 5;

            return DistanceMoved <= meleeDistanceLimit && Math.Abs(this.Position.Y - Singleton.Instance.player.Position.Y) <= verticalTolerance;
        }

        private void HandleMeleeAttack(GameTime gameTime, List<GameObject> _gameObjects)
        {
            bool isFacingEnemy = (Singleton.Instance.player.Position.X < Position.X && AnimationManager.FacingRight) ||
                                 (Singleton.Instance.player.Position.X > Position.X && !AnimationManager.FacingRight);
            if (isFacingEnemy)
            {
                AnimationManager.FacingRight = !AnimationManager.FacingRight;
            }

            attackTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (attackTimer < -attackDelay)
            {
                attackTimer = 0;
                // สร้างกระสุนจากตำแหน่ง monster
                var newBullet = bullet.Clone() as Bullet;
                int bulletX = AnimationManager.FacingRight ? Rectangle.Width : -newBullet.Rectangle.Width;
                newBullet.Position = new Vector2(Rectangle.X + bulletX, Position.Y + 15);
                newBullet.Velocity = new Vector2(AnimationManager.FacingRight ? 300 : -300, 0);
                newBullet.Reset();
                _gameObjects.Add(newBullet);
            }
            else
            {
                AnimationManager.Play(Animations["Attack"]);
            }
        }

        private void HandleMovement()
        {
            if (Singleton.Instance.player.Position.X < Position.X)
            {
                Position = new Vector2(Position.X - walkSpeed, Position.Y);
                moveDirection = -1;
                AnimationManager.FacingRight = false;
                Console.WriteLine("Left");
            }
            else
            {
                Position = new Vector2(Position.X + walkSpeed, Position.Y);
                moveDirection = 1;
                AnimationManager.FacingRight = true;
            }
            AnimationManager.Play(Animations["Walk"]);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            MaxHealth = 200f;
            Health = MaxHealth;
            Damage = 20;
            walkSpeed = 2f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = Animations["Attack"].FrameSpeed * Animations["Attack"].FrameCount / 2; // or 3.5f
            base.Reset();
        }

        public static Dictionary<int, List<Vector2>> SpawnPositions = new Dictionary<int, List<Vector2>>()
        {
            { 1, new List<Vector2> { 
                new Vector2(8304, 496),
            } },

            { 2, new List<Vector2> { 
                new Vector2(864, 1136), 
                new Vector2(1286, 1040),  
                new Vector2(10335, 928),
                new Vector2(6962, 1010),  
                new Vector2(4784, 850),
            } },

            { 3, new List<Vector2> { 
                new Vector2(1286, 1040),
            } },

            { 4, new List<Vector2> { 
                new Vector2(3974, 1755),
                new Vector2(6400, 1600),
                new Vector2(8634, 1925),
            } },

            { 5, new List<Vector2> { 
                new Vector2(6171, 3050),
                new Vector2(5460, 3170),
                new Vector2(3274, 3135),
                new Vector2(168, 4085),
                new Vector2(3516, 4085),
            } },

            { 6, new List<Vector2> { 
                new Vector2(5460, 3950),
            } },
        };

    }
}
