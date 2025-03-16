using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace TheKnightAwakening
{
    public enum ChestType
    {
        GoldChest,
        SilverChest,
        BronzeChest,
        WoodChest
    }

    public static class AnimationChest
    {
        public static Dictionary<ChestType, Dictionary<string, Animation>> LoadAnimations(Texture2D texture)
        {
            return new Dictionary<ChestType, Dictionary<string, Animation>>
            {
                { ChestType.GoldChest, LoadChestAnimations(texture, "gold") },
                { ChestType.SilverChest, LoadChestAnimations(texture, "silver") },
                { ChestType.BronzeChest, LoadChestAnimations(texture, "bronze") },
                { ChestType.WoodChest, LoadChestAnimations(texture, "wood") }
            };
        }

        private static Dictionary<string, Animation> LoadChestAnimations(Texture2D texture, string chestName)
        {
            int yPosition = chestName switch
            {
                "gold" => 217,
                "silver" => 153,
                "bronze" => 89,
                "wood" => 24,
                _ => 0
            };

            var closedFrames = new List<Rectangle>
            {
                new Rectangle(31, yPosition, 34, 40),
                new Rectangle(95, yPosition, 34, 40),
                new Rectangle(159, yPosition, 34, 40),
                new Rectangle(223, yPosition, 34, 40),
                new Rectangle(287, yPosition, 34, 40),
                new Rectangle(351, yPosition, 34, 40)
            };

            var openFrame = new Rectangle(451, yPosition, 34, 40);

            return new Dictionary<string, Animation>
            {
                { "Closed", new Animation(texture, new List<Rectangle> { closedFrames[0] }, 0.15f, false) },
                { "Opening", new Animation(texture, closedFrames, 0.15f, false) },
                { "Open", new Animation(texture, new List<Rectangle> { openFrame }, 0.15f, false) },
                { "Closing", new Animation(texture, closedFrames.AsReadOnly().Reverse().ToList(), 0.15f, false) }
            };
        }
    }
}
