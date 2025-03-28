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
            collidedWithHitblock = false;
            
            HandleHitblockCollision();

            if (gameTime.TotalGameTime.TotalSeconds > 1)
            {
                if (ShouldPerformMeleeAttack())
                {
                    HandleMeleeAttack(gameTime, _gameObjects);
                }
                else
                {
                    ResetAttackTimer();
                    HandleMovement();
                }
            }

            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }

        private void HandleHitblockCollision()
        {
            const int frontOffset = 5;
            Rectangle frontRect = AnimationManager.FacingRight ?
                new Rectangle(Rectangle.Right, Rectangle.Y, frontOffset, Rectangle.Height) :
                new Rectangle(Rectangle.X - frontOffset, Rectangle.Y, frontOffset, Rectangle.Height);

            if (Singleton.Instance.HitblockTiles != null)
            {
                foreach (var tile in Singleton.Instance.HitblockTiles)
                {
                    if (frontRect.Intersects(tile))
                    {
                        collidedWithHitblock = true;
                        break;
                    }
                }
            }

            if (collidedWithHitblock)
            {
                moveDirection *= -1;
                AnimationManager.FacingRight = moveDirection > 0;
            }
        }

        private bool ShouldPerformMeleeAttack()
        {
            const int meleeDistanceLimit = 2000;
            const int verticalTolerance = 5;

            return DistanceMoved <= meleeDistanceLimit && Math.Abs(Position.Y - Singleton.Instance.player.Position.Y) <= verticalTolerance;
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

        private void ResetAttackTimer()
        {
            attackTimer = 0f;
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
            Health = 100;
            Damage = 25;
            walkSpeed = 3f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = Animations["Attack"].FrameSpeed * Animations["Attack"].FrameCount / 2;
            base.Reset();
        }

        public static List<Vector2> SpawnPositions = new List<Vector2>
        {
            new Vector2(100, 0) // Test
            // new Vector2(8209, 464),
            // new Vector2(10405, 464),
            // new Vector2(1580, 1040),
            // new Vector2(2717, 1086),
            // new Vector2(4852, 1183),
            // new Vector2(5181, 830),
            // new Vector2(6796, 974),
            // new Vector2(10173, 1184),
            // new Vector2(10375, 927),
            // new Vector2(115, 1487),
            // new Vector2(4024, 1552),
            // new Vector2(6113, 1902),
            // new Vector2(11118, 1902),
            // new Vector2(761, 3343),
            // new Vector2(2431, 3440),
            // new Vector2(3280, 3134),
            // new Vector2(5269, 3343),
            // new Vector2(9015, 3343)
        };
    }
}