using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Player : GameObject
    {
        // Bullet Object of Player
        public Bullet Bullet;
        // Animation
        public AnimationManager AnimationManager;
        public Dictionary<string, Animation> Animations;

        // Checkpoint
        public Vector2 LastCheckpoint { get; set; }

        // Properties
        public int Ultimate { get; private set; }
        private float speed;
        private bool isAttacking;
        private float attackTimer;
        private bool isInvincible;
        private float invincibleTimer;
        private float blinkTimer;
        private bool isDefending;
        private bool isVisible = true;
        private float time = 5f;

        // Movement
        public Keys Left, Right, Up, Down, Fire, Defend, Attack2, Attack3, UltimateAttack;

        // Debuff
        public enum DebuffType { Slow, Stun, Poison, Weak }
        public Dictionary<DebuffType, Texture2D> debuffIcons;

        public Dictionary<DebuffType, float> activeDebuffs = new();


        public Player(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Idle"]);
            IsActive = true;
            Reset();
        }

        public override void Reset()
        {
            MaxHealth = 200;
            Health = MaxHealth;
            Damage = 50;
            Ultimate = 0;
            isDead = false;
            Position = LastCheckpoint;
            Velocity = Vector2.Zero;
            activeDebuffs.Clear();
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.R))
            {
                Reset();
            }
            if (isDead)
            {
                AnimationManager.Play(Animations["Dead"]);
                AnimationManager.Update(gameTime);
                Singleton.Instance.AudioManager.PlayEffect("Death_SFX");
                return;
            }
            if (activeDebuffs.ContainsKey(DebuffType.Poison))
            {

                time -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                if (time < 0f)
                {
                    TakeDamage(1, Vector2.Zero);
                    time = 5f;
                    // Console.WriteLine("Poisoned! Health: " + Health);
                }
            }
            UpdateDebuffs(gameTime);
            HandleInvincibility(gameTime);
            HandleDefend();
            HandleMovement();
            HandleAttacks(gameTime, gameObjects);
            AnimationManager.Update(gameTime);

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
            base.Update(gameTime, gameObjects);
        }
        private void UpdateDebuffs(GameTime gameTime)
        {
            var keys = new List<DebuffType>(activeDebuffs.Keys);
            foreach (var debuff in keys)
            {
                activeDebuffs[debuff] -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (activeDebuffs[debuff] <= 0)
                    activeDebuffs.Remove(debuff);
            }
        }

        private void HandleInvincibility(GameTime gameTime)
        {
            if (!isInvincible) return;

            invincibleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            blinkTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            isVisible = blinkTimer <= 0f ? !isVisible : isVisible;
            blinkTimer = blinkTimer <= 0f ? 0.1f : blinkTimer;
            isInvincible = invincibleTimer > 0f;
        }

        private void HandleMovement()
        {
            var velocity = Vector2.Zero;
            bool isRunning = Singleton.Instance.CurrentKey.IsKeyDown(Keys.LeftShift);
            speed = isRunning ? 8f : 4f;

            if (activeDebuffs.ContainsKey(DebuffType.Slow))
                speed *= 0.5f;
            if (activeDebuffs.ContainsKey(DebuffType.Stun))
                return;
            if (isDefending)
                speed *= 0.5f;

            if (Singleton.Instance.CurrentKey.IsKeyDown(Left))
            {
                velocity.X -= speed;
                AnimationManager.FacingRight = false;
            }
            if (Singleton.Instance.CurrentKey.IsKeyDown(Right))
            {
                velocity.X += speed;
                AnimationManager.FacingRight = true;
            }

            if (OnGround && Singleton.Instance.CurrentKey.IsKeyDown(Up) &&
                !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
            {
                Velocity.Y = -11;
                OnGround = false;
                AnimationManager.Play(Animations["Jump"]);
            }

            if (!OnGround)
            {
                Velocity.Y += Gravity;
                Position.Y += Velocity.Y;
            }

            Position += new Vector2(velocity.X, 0);

            if (OnGround && !isAttacking && !isDefending)
            {
                if (velocity.X != 0)
                    AnimationManager.Play(isRunning ? Animations["Run"] : Animations["Walk"]);
                else
                    AnimationManager.Play(Animations["Idle"]);
            }
        }

        private void HandleDefend()
        {
            if (Singleton.Instance.CurrentKey.IsKeyDown(Defend) &&
            !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
            {
                AnimationManager.Play(Animations["Defend"]);
                isDefending = true;
            }
            else if (Singleton.Instance.CurrentKey.IsKeyUp(Defend) &&
            !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
            {
                isDefending = false;
            }
        }

        private void HandleAttacks(GameTime gameTime, List<GameObject> _gameObjects)
        {
            if (isAttacking)
            {
                attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (attackTimer >= Animations["Attack1"].FrameSpeed * Animations["Attack1"].FrameCount)
                    isAttacking = false;
                return;
            }

            if (Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
                !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
            {
                TriggerAttack("Attack1", _gameObjects, 0);
            }
            else if (Ultimate >= 1 && Singleton.Instance.CurrentKey.IsKeyDown(Attack2) &&
            !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
            {
                TriggerAttack("Attack2", _gameObjects, 20);
                Ultimate -= 1;
            }
            else if (Ultimate >= 5 && Singleton.Instance.CurrentKey.IsKeyDown(Attack3) &&
            !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
            {
                TriggerAttack("Attack3", _gameObjects, 50);
                var newBullet = Bullet.Clone() as Bullet;
                newBullet.Position = new Vector2(Rectangle.X + (AnimationManager.FacingRight ? Rectangle.Width : -newBullet.Rectangle.Width), Position.Y + 15);
                newBullet.Velocity = new Vector2(AnimationManager.FacingRight ? 300 : -300, 0);
                newBullet.Reset();
                _gameObjects.Add(newBullet);
                Ultimate = 0;
            }
        }

        private void TriggerAttack(string attack, List<GameObject> gameObjects, int Extradamage)
        {
            AnimationManager.Play(Animations[attack]);
            isAttacking = true;
            attackTimer = 0f;
            if (activeDebuffs.ContainsKey(DebuffType.Weak))
                this.Damage = 25;

            Singleton.Instance.AudioManager.PlayEffect("Sword_SFX"); 

            foreach (var gameObject in gameObjects)
            {
                if (gameObject is MonsterType monster)
                {
                    if (Player.CheckAABBCollision(this.Rectangle, monster.Rectangle))
                    {
                        monster.TakeDamage(this.Damage + Extradamage, this.Position);
                        Ultimate++; // Test Befor Edit
    
                    }
                }
            }
        }

        public void TakeDamage(int damage, Vector2 enemyPosition)
        {
            if (isInvincible) return;

            // เช็คว่าหันหน้าถูกด้านหรือไม่
            bool isFacingEnemy = (enemyPosition.X > Position.X && AnimationManager.FacingRight) ||
                                 (enemyPosition.X < Position.X && !AnimationManager.FacingRight);

            // ถ้ากันแต่หันผิดด้าน -> โดนดาเมจ
            damage = (isDefending && !isFacingEnemy) ? damage : (isDefending ? 0 : damage);
            if (damage > 0)
            {
                Health -= damage;
                if (Health <= 0)
                {
                    Health = 0;
                    isDead = true;
                    AnimationManager.Play(Animations["Dead"]);
                }
                else
                {
                    isInvincible = true;
                    invincibleTimer = 1f;
                }
            }
        }

        public void TakeDebuff(DebuffType debuffType, float duration, Vector2 enemyPosition)
        {
            bool isFacingEnemy = (enemyPosition.X > Position.X && AnimationManager.FacingRight) ||
                                 (enemyPosition.X < Position.X && !AnimationManager.FacingRight);

            if (!activeDebuffs.ContainsKey(debuffType))
            {
                if (!isDefending || (isDefending && !isFacingEnemy))
                {
                    activeDebuffs[debuffType] = duration;

                    if (debuffType == DebuffType.Stun)
                    {
                        AnimationManager.Play(Animations["Idle"]);
                        isAttacking = false;
                        isDefending = false;
                    }
                    else if (debuffType == DebuffType.Slow)
                    {
                        speed *= 0.5f;
                    }
                }
            }
            else
            {
                // Console.WriteLine("Debuff already applied");
            }
        }

        public void Heal(float heal)
        {
            this.Health += heal;
            if (this.Health > this.MaxHealth) this.Health = MaxHealth;
        }

        public void LoadDebufIcons(ContentManager Content)
        {
            debuffIcons = new Dictionary<DebuffType, Texture2D>{
                { DebuffType.Slow, Content.Load<Texture2D>("Slow") },
                { DebuffType.Stun, Content.Load<Texture2D>("Petrify") },
                { DebuffType.Poison, Content.Load<Texture2D>("poison") },
                { DebuffType.Weak, Content.Load<Texture2D>("Weakness") }
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color playerColor = Color.White;
            if (activeDebuffs.ContainsKey(DebuffType.Stun))
            {
                playerColor = Color.Gray;
            }
            else if (activeDebuffs.ContainsKey(DebuffType.Slow))
            {
                playerColor = Color.Red;
            }
            else if (activeDebuffs.ContainsKey(DebuffType.Poison))
            {
                playerColor = Color.Green;
            }
            if (isVisible)
            {
                AnimationManager.Position = Position;
                AnimationManager.Draw(spriteBatch, playerColor);
            }
        }
    }
}