using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public static class AnimationPlayer
    {
        public static Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(5,   0, 43, 64),
                        new Rectangle(72,  0, 43, 64),
                        new Rectangle(139, 0, 43, 64),
                        new Rectangle(206, 0, 43, 64)
                    }, 0.15f, true)
                },

                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(8,   80, 40, 64),
                        new Rectangle(74,  81, 43, 63),
                        new Rectangle(143, 81, 48, 63),
                        new Rectangle(217, 80, 47, 64),
                        new Rectangle(290, 80, 46, 64),
                        new Rectangle(342, 81, 43, 63),
                        new Rectangle(411, 80, 43, 64),
                        new Rectangle(480, 80, 44, 64)
                    }, 0.10f, true)
                },

                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(2,   161, 48, 64),
                        new Rectangle(69,  163, 54, 64),
                        new Rectangle(142, 160, 49, 64),
                        new Rectangle(210, 163, 51, 64),
                        new Rectangle(280, 162, 48, 64),
                        new Rectangle(347, 163, 54, 64),
                        new Rectangle(420, 160, 48, 64)
                    }, 0.10f, true)
                },

                { "Jump", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(2,   247, 49, 64),
                        new Rectangle(81,  240, 49, 64),
                        new Rectangle(153, 240, 46, 64),
                        new Rectangle(229, 239, 69, 64),
                        new Rectangle(305, 246, 64, 64),
                        new Rectangle(385, 247, 61, 64)
                    }, 0.12f, false)
                },

                { "Attack1", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(4,   320, 43, 64),
                        new Rectangle(70,  320, 59, 64),
                        new Rectangle(152, 320, 64, 64),
                        new Rectangle(251, 320, 33, 64),
                        new Rectangle(319, 310, 85, 64)
                    }, 0.10f, false)
                },

                { "Attack2", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(6,   417, 42, 75),
                        new Rectangle(101, 421, 43, 75),
                        new Rectangle(198, 412, 88, 75),
                        new Rectangle(299, 416, 66, 75)
                    }, 0.10f, false)
                },

                { "Attack3", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(9,   496, 48, 64),
                        new Rectangle(113, 496, 49, 64),
                        new Rectangle(217, 496, 85, 64),
                        new Rectangle(315, 496, 80, 64)
                    }, 0.10f, false)
                },


                { "Dead", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(5,   578, 43, 64),
                        new Rectangle(78,  578, 47, 64),
                        new Rectangle(160, 578, 45, 64),
                        new Rectangle(239, 578, 50, 64),
                        new Rectangle(316, 578, 54, 64),                            
                        new Rectangle(392, 578, 54, 64)
                    }, 0.10f, false)
                },


                { "Defend", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(7,   657, 43, 64),
                        new Rectangle(83,  657, 43, 64),
                        new Rectangle(159, 657, 43, 64),
                        new Rectangle(235, 657, 43, 64),
                        new Rectangle(311, 657, 43, 64)
                    }, 0.10f, false)
                }
            };
        }
    }
}
