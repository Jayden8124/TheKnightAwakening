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
            AnimationManager = new AnimationManager(Animations["0"]);
            Checked = false;
            IsActive = true;
        }

        public void Collected()
        {
            Console.WriteLine("Flag Collected");
            Singleton.Instance.player.LastCheckpoint = Position;
            Checked = true;
            AnimationManager.Play(Animations["Raise"]);
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
                if(!Checked)
                Collected();
            }

            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }
    }
}