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
            if (Health <= 0)
            {
                base.Update(gameTime, _gameObjects);
                return;
            }

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

            if (DistanceMoved <= closeRangeDistance && Math.Abs(this.Position.Y - Singleton.Instance.player.Position.Y) <= 35)
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
            MaxHealth = 100f;
            Health = MaxHealth;
            walkSpeed = 1.5f;
            runSpeed = 4f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = 3.0f;
            base.Reset();
        }

        public static Dictionary<int, List<Vector2>> SpawnPositions = new Dictionary<int, List<Vector2>>()
        {
            { 1, new List<Vector2> { 
                new Vector2(7144, 465),
                new Vector2(8304, 500),
                new Vector2(7698, 500),
                new Vector2(8913, 500),
                new Vector2(9699, 500),
            } },

            { 2, new List<Vector2> { 
                new Vector2(11343, 500), 
                new Vector2(12074, 1220),
                new Vector2(12074, 1250),
                new Vector2(9089, 1220),
                new Vector2(5584, 1220),
                new Vector2(4575, 1220),
            } },

            { 3, new List<Vector2> { 
                new Vector2(2027, 1145), 
                new Vector2(1479, 1950), 
                new Vector2(2561, 1850),
            } },

            { 4, new List<Vector2> { 
                new Vector2(5606, 1940), 
                new Vector2(1479, 1950), 
                new Vector2(7371, 1755),
                new Vector2(10445, 1940),
                new Vector2(9920, 2770),
                new Vector2(8705, 3395),
            } },

            { 5, new List<Vector2> { 
                new Vector2(6791, 3395), 
                new Vector2(6591, 3060),  
                new Vector2(4794, 3255),
                new Vector2(3307, 3475),
                new Vector2(2903, 4100),
            } },

            { 6, new List<Vector2> { 
                new Vector2(4879, 4078),
                new Vector2(6489, 4078),
            } },
        };
    }
}
