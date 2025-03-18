using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Coin : GameObject
    {
        // Animation
        private AnimationManager AnimationManager;
        private Dictionary<string, Animation> Animations;
        
        public Coin(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Move"]);
            IsActive = true;
        }

        public void Collected()
        {
            Console.WriteLine("Coin Collected");
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
            AnimationManager.Play(Animations["Move"]);
            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }
    }
}


// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using System;
// using System.Collections.Generic;

// namespace TheKnightAwakening
// {
//     public class Coin : GameObject
//     {
//         private string Name;
//         private int Score;

//         public Coin(string name, int score)
//         {
//             Name = name;
//             Score = score;
//         }

//         public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
//         {
//             // Logic to update potion state if needed
//             foreach (var gameObject in _gameObjects)
//             {
//                 if (GameObject.CheckAABBCollision(this.Rectangle, gameObject.Rectangle) && gameObject is Player)
//                 {
//                     Singleton.Instance.Score += Score;
//                     IsActive = false;
//                 }
//             }
//         }

//         public override void Draw(SpriteBatch spriteBatch)
//         {
//             spriteBatch.Draw(Singleton.Instance._rect, Position, Color.Gold);
//         }
//     }
// }

// Constructorx