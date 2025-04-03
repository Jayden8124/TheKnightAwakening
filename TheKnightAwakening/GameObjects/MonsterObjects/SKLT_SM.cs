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
                Singleton.Instance.player.TakeDamage(this.Damage + 200, this.Position);
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
            MaxHealth = 150f;
            Health = MaxHealth;
            Damage = 20;
            walkSpeed = 2.5f;
            runSpeed = 5.5f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = 3.0f;
            base.Reset();
        }

        public static Dictionary<int, List<Vector2>> SpawnPositions = new Dictionary<int, List<Vector2>>()
        {
            { 1, new List<Vector2> { 
                new Vector2(9332, 480), 

            } },

            { 2, new List<Vector2> { 
                new Vector2(8786, 1220), 
                new Vector2(10365, 1220),  
                new Vector2(11039, 1220),
                new Vector2(7389, 1205),
                new Vector2(6173, 1206),
            } },

            { 3, new List<Vector2> { 
               new Vector2(6173, 1206),
               new Vector2(3285, 1887),
            } },
        };
    }
}