using Godot;
using System;

public partial class ScrollDown : ScrollContainer
{
    public override void _Process(double delta)
    {
        ScrollVertical += 200;
    }
}
