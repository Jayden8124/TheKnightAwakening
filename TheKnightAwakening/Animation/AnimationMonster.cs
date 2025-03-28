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
                        new Rectangle(16, 22, 53, 70),
                        new Rectangle(112, 22, 53, 70)
                    }, 0.15f, true)
                },
                { "Projectile", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 435, 36, 64)
                    }, 0.10f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 118, 36, 70),
                        new Rectangle(96, 118, 36, 70),
                        new Rectangle(160, 118, 52, 70),
                        new Rectangle(256, 118, 59, 70),
                        new Rectangle(352, 118, 52, 70),
                        new Rectangle(443, 118, 36, 70),
                        new Rectangle(512, 118, 36, 70)
                    }, 0.3f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 363, 63, 64),
                        new Rectangle(112, 360, 59, 64),
                        new Rectangle(208, 358, 51, 64),
                        new Rectangle(304, 359, 53, 64),
                        new Rectangle(400, 359, 61, 64)
                    }, 0.25f, true)
                },
                { "Attack", new Animation(texture, new List<Rectangle> 
                    {
                        new Rectangle(13, 196, 47, 64),
                        new Rectangle(94, 196, 53, 64),
                        new Rectangle(189, 198, 42, 64),
                        new Rectangle(269, 198, 86, 64),
                        new Rectangle(397, 198, 48, 64)
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
                        new Rectangle(17, 32, 37, 64),
                        new Rectangle(145, 32, 37, 64)
                    }, 0.3f, true)
                },
                { "Projectile", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(384, 65, 45, 3)
                    }, 0.10f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 128, 35, 64),
                        new Rectangle(145, 128, 35, 64),
                        new Rectangle(273, 128, 35, 64),
                        new Rectangle(400, 128, 35, 64),
                        new Rectangle(527, 128, 35, 64),
                        new Rectangle(654, 128, 35, 64),
                        new Rectangle(789, 128, 35, 64)
                    }, 0.30f, true)
                },
                { "Attack", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 224, 35, 74),
                        new Rectangle(144, 222, 35, 74),
                        new Rectangle(263, 225, 47, 74),
                        new Rectangle(387, 220, 70, 74),
                        new Rectangle(512, 220, 58, 74),
                        new Rectangle(15, 325, 39, 74),
                        new Rectangle(144, 325, 45, 74),
                        new Rectangle(272, 327, 47, 74),
                        new Rectangle(397, 327, 50, 74),
                        new Rectangle(520, 327, 55, 74),
                        new Rectangle(642, 327, 61, 74),
                        new Rectangle(780, 327, 51, 74),
                        new Rectangle(917, 327, 48, 74),
                        new Rectangle(1046, 327, 58, 74),
                        new Rectangle(1173, 327, 58, 74)
                    }, 0.20f, true)
                },
                { "Shot", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 435, 51, 74),
                        new Rectangle(143, 435, 51, 74),
                        new Rectangle(271, 435, 51, 74)
                    }, 0.3f, false)
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
                        new Rectangle(16, 16, 47, 32),
                        new Rectangle(80, 16, 47, 32),
                        new Rectangle(144, 16, 47, 32),
                        new Rectangle(208, 16, 47, 32)
                    }, 0.35f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(15, 64, 54, 35),
                        new Rectangle(80, 64, 53, 35),
                        new Rectangle(144, 64, 52, 35),
                        new Rectangle(208, 64, 50, 35)
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
                        new Rectangle(16, 168, 49, 34),
                        new Rectangle(80, 168, 50, 34),
                        new Rectangle(144, 173, 49, 34),
                        new Rectangle(208, 173, 61, 33)
                    }, 0.3f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 211, 52, 34),
                        new Rectangle(80, 213, 56, 34),
                        new Rectangle(144, 218, 59, 32),
                        new Rectangle(208, 222, 59, 32),
                        new Rectangle(272, 218, 56, 32),
                        new Rectangle(336, 211, 51, 34),
                        new Rectangle(400, 211, 48, 34)
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
