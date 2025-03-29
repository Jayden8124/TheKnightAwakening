using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheKnightAwakening
{
    public class GameObject : ICloneable
    {
        protected Texture2D _texture;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;
        public Vector2 Velocity;
        public string Name; // Check Collision
        public bool IsActive; // Check is show in screen
        public Rectangle Viewport; // View of the object
        protected float Gravity; 
        public bool isDead;
        public int Health;
        protected int Damage;
        protected bool isJumping;

        // Properties Other
        public bool OnGround { get; set; } = false;

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height); }
        }

        public GameObject()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            Gravity = 0.5f;
            isJumping = false;
            isDead = false;
            IsActive = true;
        }

        public GameObject(Texture2D texture)
        {
            _texture = texture;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            Gravity = 0.5f;
            isJumping = false;
            isDead = false;
            IsActive = true;
        }
        public virtual void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Reset()
        {

        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        public static bool CheckAABBCollision(Rectangle A, Rectangle B)
        {
            float d1x = B.Left- A.Right;
            float d1y = B.Top -A.Bottom;
            float d2x = A.Left- B.Right;
            float d2y = A.Top -B.Bottom;

            if(d1x > 0 || d1y > 0) return false;
            if(d2x > 0 || d2y > 0) return false;
            return true;
        }
    }
}