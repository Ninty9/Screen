using Godot;
using System;
using Screen;
using Screen.Commands;

public partial class CamStatic : Node3D
{
    public override void _Process(double delta)
    {
        if (!BootCommand.boot)
        {
            Visible = true;
            return;
        }
        Visible = Reactor.Energy < 0;
    }
}
