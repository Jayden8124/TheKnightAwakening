using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public static class CollisionManager
    {
        public static void ResolveCharacterCollision(GameObject a, GameObject b)
        {
            Rectangle rectA = a.Rectangle;
            Rectangle rectB = b.Rectangle;

            if (rectA.Intersects(rectB))
            {
                Rectangle intersection = Rectangle.Intersect(rectA, rectB);

                if (intersection.Width < intersection.Height)
                {
                    float displacement = intersection.Width / 2f;
                    if (rectA.Center.X < rectB.Center.X)
                    {
                        a.Position = new Vector2(a.Position.X - displacement, a.Position.Y);
                        b.Position = new Vector2(b.Position.X + displacement, b.Position.Y);
                    }
                    else
                    {
                        a.Position = new Vector2(a.Position.X + displacement, a.Position.Y);
                        b.Position = new Vector2(b.Position.X - displacement, b.Position.Y);
                    }
                }
                else
                {
                    if (rectA.Center.Y < rectB.Center.Y)
                    {
                        // A is above B
                        a.Position = new Vector2(a.Position.X, a.Position.Y - intersection.Height);
                        
                        // If A is falling down, stop its velocity and mark as on ground
                        if (a.Velocity.Y > 0)
                        {
                            a.OnGround = true; // Mark as on ground when landing on another character
                        }
                    }
                    else
                    {
                        // B is above A
                        b.Position = new Vector2(b.Position.X, b.Position.Y - intersection.Height);
                        
                        // If B is falling down, stop its velocity and mark as on ground
                        if (b.Velocity.Y > 0)
                        {
                            b.OnGround = true; // Mark as on ground when landing on another character
                        }
                    }
                }
            }
        }

        public static void ResolveCollision(GameObject obj, List<Rectangle> collisionTiles) // Check Tile Map
        {
            Rectangle objRect = obj.Rectangle;
            foreach (var tile in collisionTiles)
            {
                if (objRect.Intersects(tile))
                {
                    Rectangle intersection = Rectangle.Intersect(objRect, tile);

                    if (intersection.Width < intersection.Height)
                    {
                        if (objRect.Center.X < tile.Center.X)
                            obj.Position = new Vector2(obj.Position.X - intersection.Width, obj.Position.Y);
                        else
                            obj.Position = new Vector2(obj.Position.X + intersection.Width, obj.Position.Y);
                    }
                    else
                    {
                        if (objRect.Center.Y < tile.Center.Y)
                            obj.Position = new Vector2(obj.Position.X, obj.Position.Y - intersection.Height);
                        else
                            obj.Position = new Vector2(obj.Position.X, obj.Position.Y + intersection.Height);
                    }
                    objRect = obj.Rectangle;
                }
            }
        }

       public static void UpdateOnGround(GameObject obj, List<Rectangle> collisionTiles)
        {
            Rectangle objRect = obj.Rectangle;
            Rectangle footRect = new Rectangle(objRect.X, objRect.Bottom, objRect.Width, 1);
            bool onGround = false;
            foreach (var tile in collisionTiles)
            {
                if (footRect.Intersects(tile))
                {
                    onGround = true;
                    break;
                }
            }

            if (obj.Velocity.Y >= 0)
                obj.OnGround = onGround;
        }
    }
}