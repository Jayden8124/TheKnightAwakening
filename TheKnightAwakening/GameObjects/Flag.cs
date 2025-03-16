using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Flag : GameObject
    {
        private bool Checked;
        // Animation
        private AnimationManager AnimationManager;
        private Dictionary<string, Animation> Animations;

        public Flag(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Move"]);
            Checked = false;
            IsActive = true;
        }

        public void Collected()
        {
                IsActive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            if (CheckAABBCollision(Rectangle, Singleton.Instance.player.Rectangle))
            {
                Collected();
                Singleton.Instance.Score++;
            }

            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }
    }
}