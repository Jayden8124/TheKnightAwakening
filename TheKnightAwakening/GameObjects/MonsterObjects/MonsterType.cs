using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class MonsterType : GameObject
    {
        // Animation
        public AnimationManager AnimationManager;
        public Dictionary<string, Animation> Animations;

        // Monster Status
        public int Score;
        protected float walkSpeed;
        protected float runSpeed;
        protected float DistanceMoved;
        protected int moveDirection; // 1: เคลื่อนที่ไปทางขวา, -1: เคลื่อนที่ไปทางซ้าย
        protected bool isHurt;
        protected float attackTimer;
        protected float attackDelay; 
        protected bool collidedWithHitblock = false;
        protected float deadDelay;
        public int Section { get; set; } = -1;


        public MonsterType(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            isHurt = false;
            IsActive = true;
        }

        public MonsterType(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            collidedWithHitblock = false;
            HandleHitblockCollision();
            if (!OnGround)
            {
                Velocity.Y += Gravity;
                Position.Y += Velocity.Y;
            }

            if (Health <= 0) // Debug IsActive Immediately
            {
                Health = 0;

                // Only add score and heal player once when the monster first dies
                if (!isDead)
                {
                    isDead = true;
                    Singleton.Instance.Score += Score;
                    // Console.WriteLine(Singleton.Instance.Score);

                    // Heal player if needed (only once)
                    if (Singleton.Instance.player.Health <= 50)
                    {
                        Singleton.Instance.player.Health += 10;
                    }

                    // Play death animation only once when the monster first dies
                    AnimationManager.Play(Animations["Dead"]);

                    // Play sound effect only once
                    switch (this)
                    {
                        case SL:
                            Singleton.Instance.AudioManager.PlayEffect("Slime_Die");
                            break;
                        case SKLT_AC:
                        case SKLT_SM:
                        case SKLT_WR:
                            Singleton.Instance.AudioManager.PlayEffect("Skeleton_Dead");
                            break;
                        case MDS:
                            Singleton.Instance.AudioManager.PlayEffect("Medusa_Dead");
                            break;
                    }
                }

                // Increment death delay timer
                deadDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (deadDelay > 3f)
                {
                    if (this is MDS)
                    {
                        Singleton.Instance.currentCutscene = Singleton.CutsceneType.BossDefeated;
                        Singleton.Instance.CurrentGameState = Singleton.GameState.Cutscene;
                    }
                    IsActive = false;
                }

                // Update animation but skip other logic
                AnimationManager.Update(gameTime);
                return;
            }

            DistanceMoved = Math.Abs(Position.X - Singleton.Instance.player.Position.X);

            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle healthBar = new Rectangle(Rectangle.X, Rectangle.Bottom + 10, (int)(Rectangle.Width * (Health / MaxHealth)), 5);
            spriteBatch.Draw(Singleton.Instance._rect, healthBar, Color.Red);
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            deadDelay = 0f;
            base.Reset();
        }

        public void TakeDamage(int damage, Vector2 enemyPosition)
        {
            bool isFacingEnemy = (enemyPosition.X > Position.X && AnimationManager.FacingRight) ||
                                (enemyPosition.X < Position.X && !AnimationManager.FacingRight);

            // if (isFacingEnemy && Math.Abs(Position.Y - Singleton.Instance.player.Position.Y) <= 40)
            {
                Health -= damage;
                // Console.WriteLine("Monster Health: " + Health);
            }
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

        public new virtual MonsterType Clone()
        {
            MonsterType clone = (MonsterType)this.MemberwiseClone();
            clone.AnimationManager = new AnimationManager(this.Animations["Idle"]);
            clone.Reset();
            return clone;
        }
    }
}