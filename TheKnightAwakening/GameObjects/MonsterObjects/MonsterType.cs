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
        protected float attackDelay; // Delay 3 วินาทีระหว่างการโจมตี
        protected bool collidedWithHitblock = false;

        public MonsterType(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            isHurt = false;
            // AnimationManager = new AnimationManager(Animations["Idle"]);  // จำเป็นไหม?
            IsActive = true;
        }

        public MonsterType(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            if (!OnGround)
            {
                Velocity.Y += Gravity;
                Position.Y += Velocity.Y;
            }

            if (Health <= 0)
            {
                Health = 0;
                isDead = true;
                Singleton.Instance.Score += Score;
                if (Singleton.Instance.player.Health <= 50)
                {
                    Singleton.Instance.player.Health += 10;
                }
                AnimationManager.Play(Animations["Dead"]);
                Console.WriteLine(Singleton.Instance.Score);
                IsActive = false;
            }

            DistanceMoved = Math.Abs(Position.X - Singleton.Instance.player.Position.X);

            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw Monster Animation
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public void TakeDamage(int damage, Vector2 enemyPosition)
        {
            // ถ้ากันแต่หันผิดด้าน -> โดนดาเมจ
            Health -= damage;
            Console.WriteLine("Monster Health: " + Health);

        }

        public new virtual MonsterType Clone()
        {
            MonsterType clone = (MonsterType)this.MemberwiseClone();
            clone.AnimationManager = new AnimationManager(this.Animations["Idle"]);
            return clone;
        }
    }
}

