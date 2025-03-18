using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheKnightAwakening
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get; private set; }
        private Viewport _viewport;
        private float _speed = 5f; // Camera movement speed

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
        }

        public void Follow(GameObject target)
        {
            // Calculate the desired position
            float targetX = target.Position.X - _viewport.Width / 2;
            float targetY = target.Position.Y - _viewport.Height / 2 - 150; // Fix Position

            // Proper clamping
            float clampedX = MathHelper.Clamp(targetX, 0, Singleton.SCREENWIDTH + 200  - _viewport.Width);
            float clampedY = MathHelper.Clamp(targetY, 0, Singleton.SCREENHEIGHT - _viewport.Height);

            // Smooth movement with Lerp
            Position = Vector2.Lerp(Position, new Vector2(targetX, targetY), 0.1f * _speed);
            // Position = Vector2.Lerp(Position, new Vector2(clampedX, clampedY), 0.1f * _speed);

            // Apply the transformation matrix
            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }
}
