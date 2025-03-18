using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Potion : GameObject
    {
        // Animation
        private AnimationManager AnimationManager;
        private Dictionary<string, Animation> Animations;
        
        public Potion(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Move"]);
            IsActive = true;
        }

        public void Collected()
        {
            Console.WriteLine("Potion Collected");
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
                Singleton.Instance.player.Health += 10;
            }
            AnimationManager.Play(Animations["Move"]);
            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }
    }
}