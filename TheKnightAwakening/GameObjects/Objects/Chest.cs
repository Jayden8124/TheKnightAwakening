using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Chest : GameObject
    {
        public bool IsOpen;
        public Keys openKey;

        // Animation
        private AnimationManager AnimationManager;
        private Dictionary<string, Animation> Animations;
        int numberOfItemsToSpawn;
        public enum ItemType
        {
            Potion,
            Coin,
            None // Represents no item
        }
        public Coin coin;
        public Potion potion;

        public Chest(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Closed"]);
            IsActive = true;
            IsOpen = false;

            numberOfItemsToSpawn = 2;
            Singleton.Instance.Random = new Random();
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
                if (Singleton.Instance.CurrentKey.IsKeyDown(openKey) && !IsOpen)
                {
                    IsOpen = true;
                    AnimationManager.Play(Animations["Opening"]);

                    SpawnRandomItems(_gameObjects, numberOfItemsToSpawn);
                }
            }

            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }
        public void SpawnRandomItems(List<GameObject> _gameObjects, int numberOfItems)
        {
            Dictionary<ItemType, double> itemProbabilities = new Dictionary<ItemType, double>
            {
                { ItemType.Potion, 0.4 },
                { ItemType.Coin, 0.5 },
                { ItemType.None, 0.1 }
            };

            for (int i = 0; i < numberOfItems; i++)
            {
                ItemType itemType = GetRandomItemTypeWithProbability(itemProbabilities);
                if (itemType == ItemType.None)
                {
                    Console.WriteLine("No item spawned.");
                    continue;
                }
                GameObject item = itemType switch
                {
                    ItemType.Potion => potion.Clone() as Potion,
                    ItemType.Coin => coin.Clone() as Coin,
                    _ => null
                };

                item.Position = GetRandomPosition();
                _gameObjects.Add(item);
                Console.WriteLine($"Item spawned: {itemType}");
            }
        }

        private Vector2 GetRandomPosition()
        {
            int randomX = Singleton.Instance.Random.Next(Rectangle.X - 100, Rectangle.X + 100);
            int randomY = this.Rectangle.Y;

            return new Vector2(randomX, randomY);
        }

        private ItemType GetRandomItemTypeWithProbability(Dictionary<ItemType, double> itemProbabilities)
        {
            double randomValue = Singleton.Instance.Random.NextDouble();
            double cumulativeProbability = 0.0;

            foreach (var item in itemProbabilities)
            {
                cumulativeProbability += item.Value;
                if (randomValue < cumulativeProbability)
                {
                    return item.Key;
                }
            }

            return ItemType.None; // Default fallback
        }

        public static List<Vector2> SpawnChestPosition = new List<Vector2>
        {
            new Vector2(11136, 197),
            new Vector2(125, 1013),
            new Vector2(3102, 1141),
            new Vector2(10016, 1220),
            new Vector2(3902, 1588),
            new Vector2(7599, 1941),
            new Vector2(11376, 1891),
            new Vector2(3997, 3380),
            new Vector2(6254, 3045),
            new Vector2(9327, 2917),
            new Vector2(61, 4101),
            new Vector2(5054, 3923),
            new Vector2(7023, 4100)
        };
    }
}