using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Potion : GameObject
    {
        private int HealingAmount;

        public Potion(string name, int healingAmount)
        {
            HealingAmount = healingAmount;
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            // Logic to update potion state if needed
            foreach (var gameObject in _gameObjects)
            {
                if (GameObject.CheckAABBCollision(this.Rectangle, gameObject.Rectangle) && gameObject is Player)
                {
                    // Apply healing to player
                    (gameObject as Player).Health += HealingAmount;
                    IsActive = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Singleton.Instance._rect, Position, Color.Red);
        }
    }
}