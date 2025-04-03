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
            if (Health <= 0)
            {
                base.Update(gameTime, _gameObjects);
                return;
            }

            if (gameTime.TotalGameTime.TotalSeconds > 1)
            {
                if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, this.Rectangle) && Math.Abs(Position.Y - Singleton.Instance.player.Position.Y) <= 5)
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

            if (DistanceMoved <= chaseDistance && Math.Abs(Position.Y - Singleton.Instance.player.Position.Y) <= 35)
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
            MaxHealth = 200f;
            Health = MaxHealth;
            Damage = 15;
            walkSpeed = 2f;
            runSpeed = 5f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = 4.0f;
            base.Reset();
        }

        public static Dictionary<int, List<Vector2>> SpawnPositions = new Dictionary<int, List<Vector2>>()
        {
            { 1, new List<Vector2> { 
            } },

            { 2, new List<Vector2> { 
                new Vector2(1084, 235), 
                new Vector2(11924, 1215),  
                new Vector2(8000, 1205),
                new Vector2(5050, 1210),
                new Vector2(2270, 1135),
            } },

             { 3, new List<Vector2> { 
                new Vector2(170, 1371), 
                new Vector2(861, 1930),
                new Vector2(1736, 1795),
                
            } },
            
            
             { 4, new List<Vector2> { 
                new Vector2(3565, 1918), 
                new Vector2(4966, 1775),
                new Vector2(5605, 1937),
                new Vector2(7025, 1950),
                new Vector2(8000, 1940),
                new Vector2(9318, 2320),
            } },

            
             { 5, new List<Vector2> { 
                new Vector2(7187, 3359), 
                new Vector2(5885, 3182),
                new Vector2(2617, 3432),
                new Vector2(806, 3359),
                 new Vector2(2665, 4062),
            } },

             { 6, new List<Vector2> { 
                new Vector2(5975, 4062), 
            } },
        };
    }
}
