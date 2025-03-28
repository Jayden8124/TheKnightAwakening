using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class MDS : MonsterType
    {
        public Bullet bullet;

        private float bulletSpawnTimer;
        private float weakDebuffTimer;
        public MDS(Texture2D texture) : base(texture)
        {
        }

        public MDS(Dictionary<string, Animation> animations) : base(animations)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            collidedWithHitblock = false;
            HandleHitblockCollision();

            bulletSpawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            weakDebuffTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (bulletSpawnTimer <= 0)
            {
                SpawnBullets(gameObjects);
                bulletSpawnTimer = 10f;
            }

            if (weakDebuffTimer <= 0)
            {

                if (Vector2.Distance(Singleton.Instance.player.Position, Position) <=  500)
                {
                    Singleton.Instance.player.TakeDebuff(Player.DebuffType.Weak, 5.0f, Position);
                    Singleton.Instance.player.TakeDebuff(Player.DebuffType.Stun, 1f, Position);
                    Console.WriteLine("Applied Weak Debuff to Player");
                }
                weakDebuffTimer = 5f;
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
            base.Update(gameTime, gameObjects);
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
                Singleton.Instance.player.TakeDebuff(Player.DebuffType.Poison, 5.0f, Position);
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
        private void SpawnBullets(List<GameObject> gameObjects)
        {
            Vector2[] directions = new Vector2[]
            {
                new Vector2(1, 0),   // Right
                new Vector2(-1, 0),  // Left
                new Vector2(0, 1),   // Down
                new Vector2(0, -1),  // Up
                new Vector2(1, 1),   // Bottom Right
                new Vector2(-1, 1),  // Bottom Left
                new Vector2(1, -1),  // Top Right
                new Vector2(-1, -1)  // Top Left
            };

            foreach (var dir in directions)
            {
                var newBullet = bullet.Clone() as Bullet;
                // กำหนดตำแหน่งเริ่มต้นของกระสุนให้รอบตัวบอส โดยปรับตำแหน่งออกไปจากแกนหลักเล็กน้อย
                newBullet.Position = new Vector2(Rectangle.Center.X + (dir.X * 10), Rectangle.Center.Y + (dir.Y * 10));
                newBullet.Velocity = Vector2.Normalize(dir) * 300;
                newBullet.Reset();
                gameObjects.Add(newBullet);
            }
            Console.WriteLine("Spawned bullets in all directions");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            Health = 5;
            Damage = 20;
            walkSpeed = 1f;
            runSpeed = 2f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = Animations["Attack"].FrameSpeed * Animations["Attack"].FrameCount / 2;
            bulletSpawnTimer = 10f;
            weakDebuffTimer = 5f;
            base.Reset();
        }

        public static List<Vector2> SpawnPositions = new List<Vector2>
        {
            new Vector2(100, 0) // Test
            // new Vector2(6740, 4049)
        };
    }
}