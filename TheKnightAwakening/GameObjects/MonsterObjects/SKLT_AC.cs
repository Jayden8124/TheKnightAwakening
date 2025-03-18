using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class SKLT_AC : MonsterType
    {
        public Bullet Bullet;

        public SKLT_AC(Texture2D texture) : base(texture)
        {

        }

        public SKLT_AC(Dictionary<string, Animation> animations) : base(animations)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            // รีเซ็ต flag ของ hitblock ในแต่ละเฟรม
            collidedWithHitblock = false;

            // สร้าง front rectangle เพื่อตรวจสอบว่ากำแพง (hitblock) อยู่ด้านหน้าหรือไม่
            int offset = 5; // ระยะที่ใช้ตรวจสอบด้านหน้า
            Rectangle frontRect;
            if (AnimationManager.FacingRight)
            {
                frontRect = new Rectangle(this.Rectangle.Right, this.Rectangle.Y, offset, this.Rectangle.Height);
            }
            else
            {
                frontRect = new Rectangle(this.Rectangle.X - offset, this.Rectangle.Y, offset, this.Rectangle.Height);
            }

            // ตรวจสอบ collision กับ hitblock เฉพาะส่วนด้านหน้า (เพื่อไม่ให้ตรวจจับพื้น)
            if (Singleton.Instance.HitblockTiles != null)
            {
                foreach (var tile in Singleton.Instance.HitblockTiles)
                {
                    if (frontRect.Intersects(tile))
                    {
                        collidedWithHitblock = true;
                        break;
                    }
                }
            }

            // ถ้าชนกับกำแพงที่ด้านหน้า ให้เปลี่ยนทิศทาง
            if (collidedWithHitblock)
            {
                moveDirection *= -1;
                AnimationManager.FacingRight = moveDirection > 0;
            }

            // ส่วนการเคลื่อนที่และโจมตี
            if (gameTime.TotalGameTime.TotalSeconds > 1)
            {
                if (DistanceMoved <= 2000 && this.Position.Y == Singleton.Instance.player.Position.Y)
                {
                    bool isFacingEnemy = (Singleton.Instance.player.Position.X < Position.X && AnimationManager.FacingRight) ||
                                 (Singleton.Instance.player.Position.X > Position.X && !AnimationManager.FacingRight);

                    if (isFacingEnemy)
                    {
                        AnimationManager.FacingRight = !AnimationManager.FacingRight;
                    }

                    attackTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds; // ลด attackTimer ทุกเฟรม

                    if (attackTimer < -attackDelay) // ตรวจสอบว่าเวลาถึงการโจมตีหรือยัง
                    {
                        attackTimer = 0;
                        
                        var newBullet = Bullet.Clone() as Bullet;
                        newBullet.Position = new Vector2(Rectangle.X + (AnimationManager.FacingRight ? Rectangle.Width : -newBullet.Rectangle.Width), Position.Y + 15);
                        newBullet.Velocity = new Vector2(AnimationManager.FacingRight ? 300 : -300, 0);
                        newBullet.Reset();
                        _gameObjects.Add(newBullet);
                    }
                    else
                    {
                        AnimationManager.Play(Animations["Attack"]);
                    }
                }
                else
                {
                    attackTimer = 0f;
                    // เมื่อ Player ใกล้ (≤150) และอยู่ในแนวเดียวกัน ให้วิ่งเข้าหา
                    if (Singleton.Instance.player.Position.X < Position.X)
                    {
                        Position = new Vector2(Position.X - runSpeed, Position.Y);
                        moveDirection = -1;
                        AnimationManager.FacingRight = false;
                    }
                    else
                    {
                        Position = new Vector2(Position.X + runSpeed, Position.Y);
                        moveDirection = 1;
                        AnimationManager.FacingRight = true;
                    }
                    AnimationManager.Play(Animations["Walk"]);
                }
            }

            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            Health = 100;
            Damage = 25;
            walkSpeed = 3f;
            moveDirection = -1;
            attackTimer = 0f;
            attackDelay = Animations["Attack"].FrameSpeed * Animations["Attack"].FrameCount / 2;
            base.Reset();
        }

        public static List<Vector2> SpawnPositions = new List<Vector2>
        {
            new Vector2(8209, 464),
            new Vector2(10405, 464),
            new Vector2(1580, 1040),
            new Vector2(2717, 1086),
            new Vector2(4852, 1183),
            new Vector2(5181, 830),
            new Vector2(6796, 974),
            new Vector2(10173, 1184),
            new Vector2(10375, 927),
            new Vector2(115, 1487),
            new Vector2(4024, 1552),
            new Vector2(6113, 1902),
            new Vector2(11118, 1902),
            new Vector2(761, 3343),
            new Vector2(2431, 3440),
            new Vector2(3280, 3134),
            new Vector2(5269, 3343),
            new Vector2(9015, 3343)
        };
    }
}

// Fix Distance !isAttack