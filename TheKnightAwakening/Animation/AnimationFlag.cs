using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public static class AnimationFlag
    {
        public static Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                {"0", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(41, 20,  63, 172)
                    }, 0.0f, false)
                },
                { "Raise", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(41, 20, 63, 172),
                        new Rectangle(160, 20, 64, 172),
                        new Rectangle(272, 20, 64, 172),
                        new Rectangle(384, 20, 62, 172),
                        new Rectangle(496, 20, 59, 172)
                    }, 0.25f, false)
                }
            };
        }
    }
}