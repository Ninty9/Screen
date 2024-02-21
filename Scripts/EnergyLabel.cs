using Godot;
using System;
using Screen;

public partial class EnergyLabel : Label3D
{
    public override void _Process(double delta)
    {
        Text = Reactor.Energy + "%";
    }
}
