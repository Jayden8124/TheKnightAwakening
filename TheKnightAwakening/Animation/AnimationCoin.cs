using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public static class AnimationCoin
    {
        public static Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "Move", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(0, 0,  27, 27),
                        new Rectangle(32, 0, 26, 27),
                        new Rectangle(66, 1, 23, 25),
                        new Rectangle(100, 2, 17, 25),
                        new Rectangle(134, 2, 13, 24),
                        new Rectangle(170, 3, 6, 24),
                        new Rectangle(229, 2, 18, 25),
                        new Rectangle(258, 2, 23, 25),
                        new Rectangle(289, 1, 26, 27),
                        new Rectangle(320, 1, 27, 27)
                    }, 0.25f, true)
                }
            };
        }
    }
}