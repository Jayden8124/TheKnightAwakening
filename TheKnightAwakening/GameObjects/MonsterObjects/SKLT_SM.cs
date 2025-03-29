using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class SKLT_SM : MonsterType
    {
        public SKLT_SM(Texture2D texture) : base(texture)
        {
            
        }

        public SKLT_SM(Dictionary<string, Animation> animations) : base(animations)
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
            Health = 150;
            Damage = 20;
            walkSpeed = 2.5f;
            runSpeed = 5.5f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = 3.0f;
            base.Reset();
        }

        public static List<Vector2> SpawnPositions = new List<Vector2>
        {
            new Vector2(100, 0) // Test
            // new Vector2(7540, 470),
            // new Vector2(11049, 448),
            // new Vector2(2717, 1086),
            // new Vector2(7443, 1167),
            // new Vector2(8737, 1167),
            // new Vector2(1228, 1888),
            // new Vector2(2250, 1793),
            // new Vector2(3208, 1870),
            // new Vector2(5081, 1758),
            // new Vector2(6998, 1680),
            // new Vector2(3856, 3118),
            // new Vector2(5734, 3327),
            // new Vector2(7068, 3327),
            // new Vector2(8579, 3327),
            // new Vector2(183, 4046),
            // new Vector2(1449, 3712),
            // new Vector2(3504, 4046)
        };
    }
}
