using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheKnightAwakening
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get; private set; }
        private Viewport _viewport;
        private float _speed = 3f; // Camera movement speed
        private const float FloorHeight = 720f; // Height of each floor (900px)
        private const int TotalFloors = 6; // Total number of floors

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
        }

        public void Follow(GameObject target)
        {
            // Calculate the floor number based on the player's Y position
            int floorNumber = (int)(target.Position.Y / 690);

            // Ensure that the floor number does not go beyond the maximum number of floors
            floorNumber = MathHelper.Clamp(floorNumber, 0, TotalFloors - 1);

            // Calculate the desired position for X and Y
            float targetX = target.Position.X - _viewport.Width / 2;

            // Calculate the Y position to keep the camera centered in the middle of each floor
            float targetY = (floorNumber * FloorHeight) + 360 - _viewport.Height / 2; // 360 is half of FloorHeight to center the camera

            // Proper clamping for Y to ensure camera doesn't move too far
            float clampedY = MathHelper.Clamp(targetY, 0, (TotalFloors * FloorHeight) - _viewport.Height);
            float clampedX = MathHelper.Clamp(targetX, 0, 12800);

            // Smooth movement with Lerp
            Position = Vector2.Lerp(Position, new Vector2(clampedX, clampedY), 0.03f * _speed);

            // Apply the transformation matrix
            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
        public bool IsVisible(Rectangle boundingBox)
        {
            Rectangle cameraView = new Rectangle(
                (int)this.Position.X - 100,
                (int)this.Position.Y - 100,
                Singleton.SCREENWIDTH + 200,
                Singleton.SCREENHEIGHT + 200
            );

            return cameraView.Intersects(boundingBox);
        }
    }
}
