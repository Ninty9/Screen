using Godot;
using System;

public partial class RevealText : RichTextLabel
{
    [Export] private float revealSpeed;
    public override void _Ready()
    {
        VisibleRatio = 0;
    }

    public override void _Process(double delta)
    {
        if (VisibleRatio < 1) VisibleRatio += revealSpeed * (float)delta;
        else
            ProcessMode = ProcessModeEnum.Disabled;
    }
}
