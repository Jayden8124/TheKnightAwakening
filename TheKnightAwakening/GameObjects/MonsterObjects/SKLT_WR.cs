using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class SKLT_WR : MonsterType
    {
        public SKLT_WR(Dictionary<string, Animation> animations) : base(animations)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            collidedWithHitblock = false;
            HandleHitblockCollision();

            if (gameTime.TotalGameTime.TotalSeconds > 1)
            {
                if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, this.Rectangle))
                {
                    HandleMeleeAttack(gameTime);
                }
                else
                {
                    attackTimer = 0f;
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
                new Rectangle(this.Rectangle.Right, this.Rectangle.Y, frontOffset, this.Rectangle.Height) :
                new Rectangle(this.Rectangle.X - frontOffset, this.Rectangle.Y, frontOffset, this.Rectangle.Height);

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

        private void HandleMeleeAttack(GameTime gameTime)
        {
            bool isFacingEnemy = (Singleton.Instance.player.Position.X < Position.X && AnimationManager.FacingRight) ||
                                 (Singleton.Instance.player.Position.X > Position.X && !AnimationManager.FacingRight);
            if (isFacingEnemy)
            {
                AnimationManager.FacingRight = !AnimationManager.FacingRight;
            }
            float attackAnimDuration = Animations["Attack"].FrameSpeed * Animations["Attack"].FrameCount;
            attackTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (attackTimer <= -0.4f)
            {
                AnimationManager.Play(Animations["Attack"]);
                Singleton.Instance.player.TakeDamage(this.Damage, this.Position);
                attackTimer = attackDelay;
            }
            else
            {
                if (attackTimer > attackDelay - attackAnimDuration)
                {
                    AnimationManager.Play(Animations["Attack"]);
                }
                else
                {
                    AnimationManager.Play(Animations["Idle"]);
                }
            }
        }

        private void HandleMovement()
        {
            const int chaseDistance = 150;

            if (DistanceMoved <= chaseDistance)
            {
                if (Singleton.Instance.player.Position.X < Position.X)
                {
                    Position = new Vector2(Position.X - runSpeed, Position.Y);
                    moveDirection = -1;
                    AnimationManager.FacingRight = false;
                }
                else
                {
                    Position = new Vector2(Position.X + runSpeed, Position.Y);
                    moveDirection = 1;
                    AnimationManager.FacingRight = true;
                }
                AnimationManager.Play(Animations["Run"]);
            }
            else
            {
                // หากระยะห่าง > 150 ให้เดินตามทิศทางที่กำหนด
                Position = new Vector2(Position.X + walkSpeed * moveDirection, Position.Y);
                AnimationManager.FacingRight = moveDirection > 0;
                AnimationManager.Play(Animations["Walk"]);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            Health = 200;
            Damage = 15;
            walkSpeed = 2f;
            runSpeed = 5f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = 4.0f;
            base.Reset();
        }

        public static List<Vector2> SpawnPositions = new List<Vector2>
        {
            new Vector2(100, 0) // Test
            // new Vector2(6780, 470),
            // new Vector2(9303, 470),
            // new Vector2(1190, 1108),
            // new Vector2(3504, 1108),
            // new Vector2(5871, 1189),
            // new Vector2(7900, 1189),
            // new Vector2(10969, 1190),
            // new Vector2(11834, 1190),
            // new Vector2(4016, 1893),
            // new Vector2(8540, 1908),
            // new Vector2(10745, 1909),
            // new Vector2(1573, 3525),
            // new Vector2(1870, 3140),
            // new Vector2(2833, 3446),
            // new Vector2(6557, 3012),
            // new Vector2(2661, 4068)
        };
    }
}
