using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheKnightAwakening
{
    public class Ultimatebar
{
    protected Texture2D texture;
    protected Rectangle Background, Foreground;
    protected float maxValue;
    protected float currentValue;
    protected Rectangle currentForegroundSource;
    public UltimatebarAnimated ultimatebarAnimated;

    public Ultimatebar(Texture2D tex, Rectangle bgSource, Rectangle fgSource, float max)
    {
        texture = tex;
        Background = bgSource;
        Foreground = fgSource;
        maxValue = max;
        currentValue = 0; // เริ่มต้น ultimate ที่ 0
        // กำหนด source rectangle สำหรับ foreground ให้เริ่มจาก 0 (ไม่แสดงเลย)
        currentForegroundSource = new Rectangle(Foreground.X, Foreground.Y, Foreground.Width, 0);
    }

    public void Update(float value)
    {
        // ปรับความสูงของ foreground ให้สัมพันธ์กับค่า ultimate
        currentForegroundSource.Height = (int)(value / maxValue * Foreground.Height);
        
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, new Vector2(300, 37), Background, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0); // Background
        spriteBatch.Draw(texture, new Vector2(313, 50), currentForegroundSource, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0); // Foreground
    }
}

public class UltimatebarAnimated : Ultimatebar
{
    private float _targetValue;
    private readonly float _animationSpeed = 10f;
    private Rectangle _animationSource;
    private Vector2 _animationPosition;
    private Color _animationShade;

    public UltimatebarAnimated(Texture2D tex, Rectangle bgSource, Rectangle fgSource, float max)
        : base(tex, bgSource, fgSource, max)
    {
        // เริ่มต้นค่า ultimate animation ให้สอดคล้องกับ ultimate bar ที่เริ่มที่ 0
        _targetValue = 0;
        // _animationSource เริ่มที่ตำแหน่งสุดท้ายของ Foreground โดยมีความกว้าง 0
        _animationSource = new Rectangle(Foreground.X + Foreground.Width, Foreground.Y, 0, Foreground.Height);
        _animationPosition = new Vector2(313, 50);
        _animationShade = Color.DarkGray;
    }

    public void Update(float value, GameTime gameTime)
    {
        // หากค่า value เท่ากับ currentValue ให้ล้างส่วนแอนิเมชันแล้ว return
        if (value == currentValue)
        {
            _animationSource.Width = 0; // ลบแถบแอนิเมชันออก
            return;
        }

        _targetValue = value;
        int x;

        if (_targetValue < currentValue)
        {
            currentValue -= _animationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentValue < _targetValue)
                currentValue = _targetValue;
            x = (int)(_targetValue / maxValue * Foreground.Width);
            _animationShade = Color.Gray;
        }
        else
        {
            currentValue += _animationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentValue > _targetValue)
                currentValue = _targetValue;
            x = (int)(currentValue / maxValue * Foreground.Width);
            _animationShade = Color.DarkGray * 0.5f;
        }

        // ปรับความกว้างของส่วน Foreground ตามค่า ultimate ที่อัปเดท
        currentForegroundSource.Width = x;
        // ปรับ _animationSource ให้เริ่มจากตำแหน่งที่ต่อจาก Foreground ที่แสดงปัจจุบัน
        _animationSource.X = Foreground.X + x;
        _animationSource.Width = (int)(Math.Abs(currentValue - _targetValue) / maxValue * Foreground.Width);
        // ปรับตำแหน่งวาดของแอนิเมชัน ให้สอดคล้องกับตำแหน่งสุดท้ายของ Foreground
        _animationPosition.X = 313 + currentForegroundSource.Width * 2f;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        spriteBatch.Draw(texture, _animationPosition, _animationSource, _animationShade, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
    }
}

}
