using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public static class AnimationPotion
    {
        public static Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "Move", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(0, 0,  25, 30),
                        new Rectangle(48, 0, 25, 30),
                        new Rectangle(0, 48, 25, 30),
                        new Rectangle(48, 48, 25, 30)
                    }, 1.5f, true)
                }
            };
        }
    }
}