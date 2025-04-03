using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Flag : GameObject
    {
        public bool Checked;
        // Animation
        private AnimationManager AnimationManager;
        private Dictionary<string, Animation> Animations;

        public Flag(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["0"]);
            Checked = false;
            IsActive = true;
        }

        public void Collected()
        {
            Console.WriteLine("Flag Collected");
            Singleton.Instance.player.LastCheckpoint = Position;
            Singleton.Instance.AudioManager.PlayEffect("Save_SFX");
            Checked = true;
            AnimationManager.Play(Animations["Raise"]);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }

        public override object Clone()
        {
            Flag clone = (Flag)base.Clone();
            clone.AnimationManager = new AnimationManager(this.Animations["0"]);
            return clone;
        }

        public static List<Vector2> SpawnFlagPosition = new List<Vector2>
        {
            new Vector2(5940, 71),
            new Vector2(10400, 164),
            new Vector2(4097, 932),
            new Vector2(3070, 1509),
            new Vector2(7971, 3013),
            new Vector2(4000, 3847)
        };
    }
}