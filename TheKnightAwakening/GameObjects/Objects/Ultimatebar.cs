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
        currentValue = 0; 
        currentForegroundSource = new Rectangle(Foreground.X, Foreground.Y, Foreground.Width, 0);
    }

public void Update(float value)
{
    currentValue = MathHelper.Clamp(value, 0, maxValue);
    int height = (int)(currentValue / maxValue * Foreground.Height);
    currentForegroundSource = new Rectangle(Foreground.X, Foreground.Y + Foreground.Height - height, Foreground.Width, height);
}

public virtual void Draw(SpriteBatch spriteBatch)
{
    spriteBatch.Draw(texture, new Vector2(300, 37), Background, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
    spriteBatch.Draw(texture, new Vector2(310.5f, 47 + (Foreground.Height - currentForegroundSource.Height) * 2f), currentForegroundSource, Color.White, 0f, Vector2.Zero, 2.35f, SpriteEffects.None, 0);
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
        _targetValue = 0;
        _animationSource = new Rectangle(Foreground.X + Foreground.Width, Foreground.Y, 0, Foreground.Height);
        _animationPosition = new Vector2(313, 50);
        _animationShade = Color.DarkGray;
    }

    public void Update(float value, GameTime gameTime)
    {
        if (value == currentValue)
        {
            _animationSource.Width = 0; 
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

        currentForegroundSource.Width = x;
        _animationSource.X = Foreground.X + x;
        _animationSource.Width = (int)(Math.Abs(currentValue - _targetValue) / maxValue * Foreground.Width);
        _animationPosition.Y = 47 + (Foreground.Height - currentForegroundSource.Height) * 2f;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        spriteBatch.Draw(texture, _animationPosition, _animationSource, _animationShade, 0f, Vector2.Zero, 2.05f, SpriteEffects.None, 0);
    }
}

}