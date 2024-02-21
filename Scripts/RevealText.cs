using Godot;
using System;

public partial class RevealText : RichTextLabel
{
    [Export] private float revealSpeed;
    private double revealCounter;
    private Node sound;
    public override void _Ready()
    {
        VisibleCharacters = 0;
        sound = FindChild("Sound");
    }

    public override void _Process(double delta)
    {
        revealCounter += delta;

        if (!(revealCounter > revealSpeed)) return;
        sound.Call("play");
        if (VisibleCharacters == Text.Length - 1)
        {
            VisibleCharacters = -1;
            ProcessMode = ProcessModeEnum.Disabled;
            return;
        }

        revealCounter = 0;
        VisibleCharacters++;
    }
}
