using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class AnimationMonster
    {
        public Dictionary<AnimationMonsterType, Dictionary<string, Animation>> AnimationMonsterList = new();

        public enum AnimationMonsterType
        {
            SKLT_WR,
            SKLT_SM,
            SKLT_AC,
            SL,
            MDS
        }

        public virtual Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>();
        }

        public void LoadAllAnimations(Dictionary<AnimationMonsterType, Texture2D> textures)
        {
            if (textures.ContainsKey(AnimationMonsterType.SKLT_WR))
                AnimationMonsterList[AnimationMonsterType.SKLT_WR] = new AnimationSKLT_WR().LoadAnimations(textures[AnimationMonsterType.SKLT_WR]);

            if (textures.ContainsKey(AnimationMonsterType.SKLT_SM))
                AnimationMonsterList[AnimationMonsterType.SKLT_SM] = new AnimationSKLT_SM().LoadAnimations(textures[AnimationMonsterType.SKLT_SM]);

            if (textures.ContainsKey(AnimationMonsterType.SKLT_AC))
                AnimationMonsterList[AnimationMonsterType.SKLT_AC] = new AnimationSKLT_AC().LoadAnimations(textures[AnimationMonsterType.SKLT_AC]);

            if (textures.ContainsKey(AnimationMonsterType.SL))
                AnimationMonsterList[AnimationMonsterType.SL] = new AnimationSlime().LoadAnimations(textures[AnimationMonsterType.SL]);
                
            if (textures.ContainsKey(AnimationMonsterType.MDS))
                AnimationMonsterList[AnimationMonsterType.MDS] = new AnimationMDS().LoadAnimations(textures[AnimationMonsterType.MDS]);    
        }

        public Dictionary<string, Animation> GetAnimations(AnimationMonsterType type)
        {
            return AnimationMonsterList.ContainsKey(type) ? AnimationMonsterList[type] : new Dictionary<string, Animation>();
        }
    }
    
    public class AnimationSKLT_WR : AnimationMonster
    {
        public override Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 15, 53, 70),
                        new Rectangle(112, 15, 53, 70)
                    }, 0.15f, true)
                },
                { "Projectile", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 435, 36, 64)
                    }, 0.10f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 111, 36, 70),
                        new Rectangle(96, 111, 36, 70),
                        new Rectangle(160, 111, 52, 70),
                        new Rectangle(256, 111, 59, 70),
                        new Rectangle(352, 111, 52, 70),
                        new Rectangle(443, 111, 36, 70),
                        new Rectangle(512, 111, 36, 70)
                    }, 0.3f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 353, 63, 66),
                        new Rectangle(112, 350, 59, 66),
                        new Rectangle(208, 349, 51, 66),
                        new Rectangle(304, 348, 53, 66),
                        new Rectangle(400, 348, 61, 66)
                    }, 0.25f, true)
                },
                { "Attack", new Animation(texture, new List<Rectangle> 
                    {
                        new Rectangle(13, 189, 47, 64),
                        new Rectangle(94, 189, 53, 64),
                        new Rectangle(189, 190, 42, 64),
                        new Rectangle(269, 190, 86, 64),
                        new Rectangle(397, 190, 48, 64)
                    }, 0.15f, false)
                },
                { "Dead", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(20, 534, 39, 64),
                        new Rectangle(115, 537, 39, 64),
                        new Rectangle(224, 555, 51, 64),
                        new Rectangle(321, 575, 64, 64)
                    }, 0.25f, false)
                }
            };
        }
    }
    
    public class AnimationSKLT_AC : AnimationMonster
    {
        public override Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(texture, new List<Rectangle> 
                    {
                        new Rectangle(17, 34, 37, 64),
                        new Rectangle(145, 34, 37, 64)
                    }, 0.3f, true)
                },
                { "Projectile", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(384, 65, 45, 3)
                    }, 0.10f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 123, 35, 68),
                        new Rectangle(145, 123, 35, 68),
                        new Rectangle(273, 123, 35, 68),
                        new Rectangle(400, 123, 35, 68),
                        new Rectangle(527, 123, 35, 68),
                        new Rectangle(654, 123, 35, 68),
                        new Rectangle(789, 123, 35, 68)
                    }, 0.30f, true)
                },
                { "Attack", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 220, 35, 74),
                        new Rectangle(144, 220, 35, 74),
                        new Rectangle(263, 220, 47, 74),
                        new Rectangle(387, 220, 70, 74),
                        new Rectangle(512, 220, 58, 74),
                        new Rectangle(15, 332, 39, 74),
                        new Rectangle(144, 332, 45, 74),
                        new Rectangle(272, 332, 47, 74),
                        new Rectangle(397, 332, 50, 74),
                        new Rectangle(520, 332, 55, 74),
                        new Rectangle(642, 332, 61, 74),
                        new Rectangle(780, 332, 51, 74),
                        new Rectangle(917, 332, 48, 74),
                        new Rectangle(1046, 332, 58, 74),
                        new Rectangle(1173, 332, 58, 74)
                    }, 0.2f, true)
                },
                { "Shot", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 410, 51, 74),
                        new Rectangle(143, 410, 51, 74),
                        new Rectangle(271, 410, 51, 74)
                    }, 0.15f, false)
                },
                { "Hurt", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(17, 549, 40, 59),
                        new Rectangle(145, 549, 39, 59)
                    }, 0.30f, false)
                },
                { "Dead", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 654, 46, 49),
                        new Rectangle(144, 662, 50, 41),
                        new Rectangle(272, 693, 68, 10)
                    }, 0.25f, false)
                }
            };
        }
    }

    public class AnimationSlime : AnimationMonster
    {
        public override Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 8, 47, 33),
                        new Rectangle(80, 8, 47, 33),
                        new Rectangle(144, 8, 47, 33),
                        new Rectangle(208, 8, 47, 33)
                    }, 0.35f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(15, 63, 54, 35),
                        new Rectangle(80, 63, 53, 35),
                        new Rectangle(144, 63, 52, 35),
                        new Rectangle(208, 63, 50, 35)
                    }, 0.35f, true)
                },
                { "Jump", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(13, 114, 38, 46),
                        new Rectangle(80, 112, 44, 42),
                        new Rectangle(144, 112, 48, 38),
                        new Rectangle(208, 112, 39, 43),
                        new Rectangle(272, 112, 34, 48),
                        new Rectangle(323, 112, 39, 41)
                    }, 0.10f, true)
                },
                { "Attack", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 163, 49, 33),
                        new Rectangle(80, 163, 50, 33),
                        new Rectangle(144, 168, 49, 33),
                        new Rectangle(208, 168, 61, 33)
                    }, 0.3f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 223, 52, 35),
                        new Rectangle(80, 223, 56, 35),
                        new Rectangle(144, 223, 59, 35),
                        new Rectangle(208, 223, 59, 35),
                        new Rectangle(272, 223, 56, 35),
                        new Rectangle(336, 223, 51, 35),
                        new Rectangle(400, 223, 48, 35)
                    }, 0.25f, true)
                },
                { "Hurt", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 273, 48, 31),
                        new Rectangle(80, 271, 50, 33),
                        new Rectangle(144, 268, 60, 35),
                        new Rectangle(224, 270, 77, 34),
                        new Rectangle(320, 273, 121, 31),
                        new Rectangle(458, 274, 50, 30)
                    }, 0.2f, false)
                },
                { "Dead", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 325, 46, 27),
                        new Rectangle(80, 333, 47, 19),
                        new Rectangle(144, 337, 52, 15)
                    }, 0.25f, false)
                }
            };
        }
    }

    public class AnimationSKLT_SM : AnimationMonster
    {
        public override Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 4, 30, 90),
                        new Rectangle(144, 4, 30, 90)
                    }, 0.3f, true)
                },
                { "Projectile", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(544, 27, 33, 69),
                        new Rectangle(672, 28, 29, 68)
                    }, 0.25f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 118, 42, 90),
                        new Rectangle(151, 118, 43, 90),
                        new Rectangle(275, 118, 50, 90),
                        new Rectangle(402, 118, 54, 90),
                        new Rectangle(533, 118, 48, 90),
                        new Rectangle(664, 118, 42, 90),
                        new Rectangle(785, 118, 42, 90)
                    }, 0.25f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 214, 80, 90),
                        new Rectangle(144, 214, 80, 90),
                        new Rectangle(272, 218, 80, 90),
                        new Rectangle(404, 213, 80, 90),
                        new Rectangle(532, 213, 80, 90),
                        new Rectangle(660, 215, 80, 90)
                    }, 0.23f, true)
                },
                { "Attack", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 302, 83, 90),
                        new Rectangle(143, 302, 83, 90),
                        new Rectangle(284, 303, 95, 90),
                        new Rectangle(412, 303, 96, 90)
                    }, 0.2f, true)
                },
                { "Hurt", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 446, 42, 82),
                        new Rectangle(151, 446, 35, 82),
                        new Rectangle(289, 446, 30, 82)
                    }, 0.25f, false)
                },
                { "Dead", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 574, 42, 82),
                        new Rectangle(154, 574, 36, 82),
                        new Rectangle(282, 583, 49, 73),
                        new Rectangle(410, 619, 62, 37),
                        new Rectangle(538, 642, 64, 14)
                    }, 0.20f, false)
                }
            };
        }
    }

    public class AnimationMDS : AnimationMonster // Not Ready
    {
        public override Dictionary<string, Animation> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(31, 407, 65, 90),
                        new Rectangle(159, 407, 65, 90),
                        new Rectangle(287, 407, 65, 90),
                        new Rectangle(415, 407, 65, 90),
                        new Rectangle(543, 407, 65, 90),
                        new Rectangle(671, 407, 65, 90),
                        new Rectangle(799, 407, 65, 90)
                    }, 0.35f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(42, 690, 73, 90),
                        new Rectangle(169, 690, 73, 90),
                        new Rectangle(294, 690, 73, 90),
                        new Rectangle(419, 690, 73, 90),
                        new Rectangle(545, 690, 73, 90),
                        new Rectangle(667, 690, 73, 90),
                        new Rectangle(791, 690, 73, 90),
                        new Rectangle(935, 690, 73, 90),
                        new Rectangle(1060, 690, 73, 90),
                        new Rectangle(1187, 690, 73, 90),
                        new Rectangle(1314, 690, 73, 90),
                        new Rectangle(1445, 690, 73, 90),
                        new Rectangle(1584, 690, 73, 90)
                    }, 0.25f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(7, 543, 116, 90),
                        new Rectangle(133, 543, 119, 90),
                        new Rectangle(258, 543, 123, 90),
                        new Rectangle(385, 543, 125, 90),
                        new Rectangle(513, 543, 123, 90),
                        new Rectangle(642, 543, 121, 90),
                        new Rectangle(773, 543, 117, 90)
                    }, 0.25f, true)
                },
                { "Attack", new Animation(texture, new List<Rectangle> 
                    {
                        new Rectangle(39, 40, 56, 90),
                        new Rectangle(168, 40, 56,90),
                        new Rectangle(301, 40, 51, 90),
                        new Rectangle(430, 40, 65, 90),
                        new Rectangle(556, 40, 82, 90),
                        new Rectangle(678, 40, 79, 90),
                        new Rectangle(803, 40, 49, 90)
                    }, 0.17f, false)
                },
                { "Hurt", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(40, 301, 56, 83),
                        new Rectangle(168, 292, 56, 92),
                        new Rectangle(293, 291, 59, 93)
                    }, 0.30f, false)
                },
                { "Dead", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(30, 166, 66, 90),
                        new Rectangle(158, 166, 66, 90),
                        new Rectangle(286, 230, 83, 26)
                    }, 0.25f, false)
                }
            };
        }
    }
}