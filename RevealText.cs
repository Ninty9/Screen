using Godot;
using System;
using System.Threading.Tasks;

public partial class RevealText : RichTextLabel
{
    [Export] private float revealSpeed;
    public async Task PrintText(string text, float speed)
    {
        VisibleRatio = 0;
        revealSpeed = speed;
        Text = text;
        while (VisibleRatio >= 1)
        {
            await Task.Delay(25);
        }
    }

    public override void _Process(double delta)
    {
        if (VisibleRatio < 1) VisibleRatio += revealSpeed * (float)delta;
    }
}
