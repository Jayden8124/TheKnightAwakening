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
                        new Rectangle(16, 22, 53, 58),
                        new Rectangle(112, 22, 53, 58)
                    }, 0.3f, true)
                },
                { "Projectile", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 435, 36, 61)
                    }, 0.10f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 111, 36, 65),
                        new Rectangle(96, 111, 36, 65),
                        new Rectangle(160, 111, 52, 65),
                        new Rectangle(256, 111, 59, 65),
                        new Rectangle(352, 111, 52, 65),
                        new Rectangle(443, 111, 36, 65),
                        new Rectangle(512, 111, 36, 65)
                    }, 0.25f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 363, 63, 53),
                        new Rectangle(112, 360, 59, 56),
                        new Rectangle(208, 358, 51, 58),
                        new Rectangle(304, 359, 53, 57),
                        new Rectangle(400, 359, 61, 57)
                    }, 0.25f, true)
                },
                { "Attack", new Animation(texture, new List<Rectangle> 
                    {
                        new Rectangle(13, 194, 47, 62),
                        new Rectangle(94, 194, 53, 62),
                        new Rectangle(189, 197, 42, 59),
                        new Rectangle(269, 179, 86, 77),
                        new Rectangle(397, 202, 48, 54)
                    }, 0.25f, false)
                },
                { "Hurt", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 274, 47, 62),
                        new Rectangle(96, 275, 47, 61)
                    }, 0.30f, false)
                },
                { "Dead", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(20, 534, 39, 58),
                        new Rectangle(115, 537, 39, 55),
                        new Rectangle(224, 555, 51, 37),
                        new Rectangle(321, 575, 64, 17)
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
                        new Rectangle(16, 224, 35, 64),
                        new Rectangle(144, 224, 35, 64),
                        new Rectangle(263, 227, 47, 61),
                        new Rectangle(387, 222, 70, 66),
                        new Rectangle(512, 222, 58, 66),
                        new Rectangle(15, 334, 39, 66),
                        new Rectangle(144, 333, 45, 67),
                        new Rectangle(272, 326, 47, 74),
                        new Rectangle(397, 326, 50, 74),
                        new Rectangle(520, 326, 55, 74),
                        new Rectangle(642, 326, 61, 74),
                        new Rectangle(780, 326, 51, 74),
                        new Rectangle(917, 326, 48, 74),
                        new Rectangle(1046, 326, 58, 74),
                        new Rectangle(1173, 326, 58, 74)
                    }, 0.20f, true)
                },
                { "Shot", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 439, 51, 74),
                        new Rectangle(143, 439, 51, 74),
                        new Rectangle(271, 439, 51, 74)
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
                    }, 0.2f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(15, 72, 54, 24),
                        new Rectangle(80, 70, 53, 26),
                        new Rectangle(144, 66, 52, 30),
                        new Rectangle(208, 63, 50, 33)
                    }, 0.20f, true)
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
                        new Rectangle(16, 176, 49, 31),
                        new Rectangle(80, 177, 50, 31),
                        new Rectangle(144, 174, 49, 34),
                        new Rectangle(208, 175, 61, 33)
                    }, 0.3f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 225, 52, 31),
                        new Rectangle(80, 227, 56, 29),
                        new Rectangle(144, 229, 59, 27),
                        new Rectangle(208, 231, 59, 25),
                        new Rectangle(272, 226, 56, 30),
                        new Rectangle(336, 224, 51, 32),
                        new Rectangle(400, 222, 48, 34)
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
                    }, 5f, false)
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
                        new Rectangle(16, 16, 30, 80),
                        new Rectangle(144, 16, 30, 80)
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
                        new Rectangle(16, 121, 42, 87),
                        new Rectangle(151, 120, 43, 88),
                        new Rectangle(275, 120, 50, 88),
                        new Rectangle(402, 115, 54, 93),
                        new Rectangle(533, 118, 48, 90),
                        new Rectangle(664, 120, 42, 88),
                        new Rectangle(785, 122, 42, 86)
                    }, 0.25f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 244, 80, 60),
                        new Rectangle(144, 245, 80, 59),
                        new Rectangle(272, 248, 80, 56),
                        new Rectangle(404, 244, 80, 60),
                        new Rectangle(532, 244, 80, 60),
                        new Rectangle(660, 246, 80, 58)
                    }, 0.12f, false)
                },
                { "Attack", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(16, 347, 83, 53),
                        new Rectangle(143, 347, 83, 53),
                        new Rectangle(284, 348, 95, 52),
                        new Rectangle(412, 348, 96, 52)
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
                        new Rectangle(31, 419, 65, 77),
                        new Rectangle(159, 418, 65, 78),
                        new Rectangle(287, 416, 65, 80),
                        new Rectangle(415, 415, 65, 81),
                        new Rectangle(543, 413, 65, 83),
                        new Rectangle(671, 411, 65, 85),
                        new Rectangle(799, 415, 65, 81)
                    }, 0.3f, true)
                },
                { "Walk", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(42, 699, 53, 85),
                        new Rectangle(169, 701, 54, 83),
                        new Rectangle(294, 702, 57, 82),
                        new Rectangle(419, 703, 60, 81),
                        new Rectangle(545, 701, 62, 83),
                        new Rectangle(667, 698, 68, 86),
                        new Rectangle(791, 696, 73, 88),
                        new Rectangle(935, 696, 57, 88),
                        new Rectangle(1060, 695, 60, 89),
                        new Rectangle(1187, 697, 60, 87),
                        new Rectangle(1314, 699, 61, 85),
                        new Rectangle(1445, 700, 58, 84),
                        new Rectangle(1548, 699, 47, 85)
                    }, 0.25f, true)
                },
                { "Run", new Animation(texture, new List<Rectangle>
                    {
                        new Rectangle(7, 553, 116, 80),
                        new Rectangle(133, 552, 119, 81),
                        new Rectangle(258, 551, 123, 82),
                        new Rectangle(385, 550, 125, 83),
                        new Rectangle(513, 551, 123, 82),
                        new Rectangle(642, 552, 121, 81),
                        new Rectangle(773, 552, 117, 81)
                    }, 0.25f, true)
                },
                { "Attack", new Animation(texture, new List<Rectangle> 
                    {
                        new Rectangle(39, 45, 56, 83),
                        new Rectangle(168, 44, 56, 84),
                        new Rectangle(301, 49, 51, 79),
                        new Rectangle(430, 48, 65, 80),
                        new Rectangle(556, 49, 82, 79),
                        new Rectangle(678, 47, 79, 81),
                        new Rectangle(803, 42, 49, 86)
                    }, 0.25f, false)
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
