// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;
// using System;
// using System.Collections.Generic;

// namespace TheKnightAwakening
// {
//     public class Chest : GameObject
//     {
//         public bool IsOpen;
//         public Keys openKey;

//         // Animation
//         private AnimationManager AnimationManager;
//         private Dictionary<string, Animation> Animations;
//         int numberOfItemsToSpawn;
//         public enum ItemType
//         {
//             Potion,
//             Coin,
//             None // Represents no item
//         }

//         public Chest(Dictionary<string, Animation> animations)
//         {
//             Animations = animations;
//             AnimationManager = new AnimationManager(Animations["Closed"]);
//             IsActive = true;
//             IsOpen = false;

//             numberOfItemsToSpawn = 20;
//             Singleton.Instance.Random = new Random();
//         }

//         public override void Draw(SpriteBatch spriteBatch)
//         {
//             AnimationManager.Position = Position;
//             AnimationManager.Draw(spriteBatch);
//         }

//         public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
//         {
//             if (CheckAABBCollision(Rectangle, Singleton.Instance.player.Rectangle))
//             {
//                 if (Singleton.Instance.CurrentKey.IsKeyDown(openKey) && !IsOpen)
//                 {
//                     IsOpen = true;
//                     AnimationManager.Play(Animations["Opening"]);

//                     SpawnRandomItems(_gameObjects, numberOfItemsToSpawn);
//                 }
//             }

//             AnimationManager.Update(gameTime);
//             base.Update(gameTime, _gameObjects);
//         }
//         public void SpawnRandomItems(List<GameObject> _gameObjects, int numberOfItems)
//         {
//             Dictionary<ItemType, double> itemProbabilities = new Dictionary<ItemType, double>
//             {
//                 { ItemType.Potion, 0.4 },
//                 { ItemType.Coin, 0.5 },
//                 { ItemType.None, 0.1 }
//             };

//             for (int i = 0; i < numberOfItems; i++)
//             {
//                 ItemType itemType = GetRandomItemTypeWithProbability(itemProbabilities);
//                 if (itemType == ItemType.None)
//                 {
//                     Console.WriteLine("No item spawned.");
//                     continue;
//                 }

//                 GameObject item = itemType switch
//                 {
//                     ItemType.Potion => new Potion("potion", 10).Clone() as Potion,
//                     ItemType.Coin => new Coin("Coin", 10).Clone() as Coin,
//                     _ => null
//                 };

//                 item.Position = GetRandomPosition();
//                 _gameObjects.Add(item);
//                 Console.WriteLine($"Item spawned: {itemType}");
//             }
//         }

//         private Vector2 GetRandomPosition()
//         {
//             int randomX = Singleton.Instance.Random.Next(Rectangle.X - 100, Rectangle.X + 100);
//             int randomY = this.Rectangle.Y;

//             return new Vector2(randomX, randomY);
//         }

//         private ItemType GetRandomItemTypeWithProbability(Dictionary<ItemType, double> itemProbabilities)
//         {
//             double randomValue = Singleton.Instance.Random.NextDouble();
//             double cumulativeProbability = 0.0;

//             foreach (var item in itemProbabilities)
//             {
//                 cumulativeProbability += item.Value;
//                 if (randomValue < cumulativeProbability)
//                 {
//                     return item.Key;
//                 }
//             }

//             return ItemType.None; // Default fallback
//         }
//     }
// }


// Fix Coin