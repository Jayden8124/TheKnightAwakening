using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public static class AnimationUlitmate
    {
        public static Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                {"0", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(25, 6,  54, 65)
                    }, 0.0f, false)
                },
                { "Move", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(25, 6,  54, 65),
                        new Rectangle(108, 6, 54, 65),
                        new Rectangle(183, 6, 54, 65),
                        new Rectangle(255, 6, 54, 65),
                        new Rectangle(335, 6, 54, 65),
                        new Rectangle(440, 6, 54, 65),
                    }, 0.25f, true)
                },
                {
                    "Medusa", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(24, 24,  76, 76),
                        new Rectangle(152, 24, 76, 76),
                        new Rectangle(280, 28, 76, 76),
                        new Rectangle(408, 28, 76, 76),
                        new Rectangle(536, 28, 76, 76)
                    }, 0.25f, true)
                }
            };
        }
    }
}