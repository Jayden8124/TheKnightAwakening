using Microsoft.Xna.Framework;
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
                // คำนวณพื้นที่ทับซ้อน
                Rectangle intersection = Rectangle.Intersect(rectA, rectB);

                // ตรวจสอบแนวแกนที่มีการทับซ้อนน้อยกว่า
                if (intersection.Width < intersection.Height)
                {
                    // เลื่อนในแกน X (แบ่งครึ่งให้ทั้งสองฝ่าย)
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
                    // เลื่อนในแกน Y (แบ่งครึ่งให้ทั้งสองฝ่าย)
                    float displacement = intersection.Height / 4f;
                    if (rectA.Center.Y < rectB.Center.Y)
                    {
                        a.Position = new Vector2(a.Position.X, a.Position.Y - displacement);
                        b.Position = new Vector2(b.Position.X, b.Position.Y + displacement);
                    }
                    else
                    {
                        a.Position = new Vector2(a.Position.X, a.Position.Y + displacement);
                        b.Position = new Vector2(b.Position.X, b.Position.Y - displacement);
                    }
                }
            }
        }

        // เมธอดสำหรับแก้ไขตำแหน่งเมื่อเกิดการชนกับ tile map
        public static void ResolveCollision(GameObject obj, List<Rectangle> collisionTiles)
        {
            Rectangle objRect = obj.Rectangle;
            foreach (var tile in collisionTiles)
            {
                if (objRect.Intersects(tile))
                {
                    // คำนวณพื้นที่ทับซ้อน
                    Rectangle intersection = Rectangle.Intersect(objRect, tile);

                    // แก้ไขการชนในแนวที่มีการทับซ้อนน้อยกว่า
                    if (intersection.Width < intersection.Height)
                    {
                        // แก้ไขในแนวแกน X
                        if (objRect.Center.X < tile.Center.X)
                            obj.Position = new Vector2(obj.Position.X - intersection.Width, obj.Position.Y);
                        else
                            obj.Position = new Vector2(obj.Position.X + intersection.Width, obj.Position.Y);
                    }
                    else
                    {
                        // แก้ไขในแนวแกน Y
                        if (objRect.Center.Y < tile.Center.Y)
                            obj.Position = new Vector2(obj.Position.X, obj.Position.Y - intersection.Height);
                        else
                            obj.Position = new Vector2(obj.Position.X, obj.Position.Y + intersection.Height);
                    }
                    // อัพเดท objRect หลังจากแก้ไขตำแหน่ง
                    objRect = obj.Rectangle;
                }
            }
        }

        // เมธอดสำหรับอัปเดทสถานะ OnGround
       public static void UpdateOnGround(GameObject obj, List<Rectangle> collisionTiles)
        {
            Rectangle objRect = obj.Rectangle;
            // สร้าง rectangle เล็ก ๆ ที่ด้านล่างของวัตถุ (ความสูง 5 พิกเซล)
            Rectangle footRect = new Rectangle(objRect.X, objRect.Bottom, objRect.Width, 5);
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
