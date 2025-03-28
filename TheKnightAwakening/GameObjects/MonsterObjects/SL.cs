using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class SL : MonsterType
    {
        public SL(Texture2D texture) : base(texture)
        {
        }

        public SL(Dictionary<string, Animation> animations) : base(animations)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            // รีเซ็ต flag ของ hitblock ในแต่ละเฟรม
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
            float attackAnimDuration = Animations["Attack"].FrameSpeed * Animations["Attack"].FrameCount;
            attackTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (attackTimer <= -0.4f)
            {
                AnimationManager.Play(Animations["Attack"]);
                Singleton.Instance.player.TakeDamage(this.Damage, this.Position);
                // Singleton.Instance.player.TakeDebuff(Player.DebuffType.Stun, 5.0f, this.Position);
                Singleton.Instance.player.TakeDebuff(Player.DebuffType.Slow, 5.0f, this.Position);
                Singleton.Instance.player.TakeDebuff(Player.DebuffType.Poison, 15.0f, this.Position);
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
            const int closeRangeDistance = 150;

            if (DistanceMoved <= closeRangeDistance)
            {
                // เมื่อ Player ใกล้ (≤150) ให้วิ่งเข้าหา
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
            Health = 100;
            walkSpeed = 1.5f;
            runSpeed = 4f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = 3.0f;
            base.Reset();
        }

        public static List<Vector2> SpawnPositions = new List<Vector2>
        {
            new Vector2(100, 0), // Test
            // new Vector2(6636, 496),
            // new Vector2(6945, 496),
            // new Vector2(7698, 496),
            // new Vector2(8304, 496),
            // new Vector2(8913, 496),
            // new Vector2(9699, 496),
            // new Vector2(11343, 496),
            // new Vector2(864, 1136),
            // new Vector2(1286, 1040),
            // new Vector2(2027, 1136),
            // new Vector2(3261, 1136),
            // new Vector2(4209, 1071),
            // new Vector2(4575, 1215),
            // new Vector2(4955, 865),
            // new Vector2(5584, 1215),
            // new Vector2(7059, 1215),
            // new Vector2(7633, 959),
            // new Vector2(9089, 1215),
            // new Vector2(11343, 1247),
            // new Vector2(12074, 1215),
            // new Vector2(1479, 1936),
            // new Vector2(2561, 1840),
            // new Vector2(3565, 1919),
            // new Vector2(4563, 1919),
            // new Vector2(5606, 1937),
            // new Vector2(5718, 1615),
            // new Vector2(7025, 1950),
            // new Vector2(7741, 1728),
            // new Vector2(7875, 1934),
            // new Vector2(9066, 1935),
            // new Vector2(10445, 1935),
            // new Vector2(9674, 2207),
            // new Vector2(9805, 2766),
            // new Vector2(979, 3375),
            // new Vector2(2513, 3056),
            // new Vector2(2092, 3472),
            // new Vector2(1423, 3328),
            // new Vector2(3771, 3375),
            // new Vector2(3307, 3472),
            // new Vector2(3432, 3166),
            // new Vector2(4075, 3166),
            // new Vector2(4816, 3278),
            // new Vector2(5578, 3166),
            // new Vector2(6353, 3038),
            // new Vector2(6048, 3375),
            // new Vector2(6791, 3375),
            // new Vector2(7524, 3375),
            // new Vector2(8368, 3375),
            // new Vector2(8719, 3375),
            // new Vector2(9362, 3375),
            // new Vector2(9539, 3056),
            // new Vector2(405, 4094),
            // new Vector2(1085, 3760),
            // new Vector2(1936, 3760),
            // new Vector2(2499, 4094),
            // new Vector2(2903, 4094),
            // new Vector2(3176, 4094)
        };
    }
}