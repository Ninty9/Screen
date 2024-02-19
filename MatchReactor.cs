using Godot;
using System;
using Screen;

public partial class MatchReactor : OmniLight3D
{
    [Export] private float energyFactor = 4f;
    public override void _Process(double delta)
    {
        LightEnergy = Reactor.React.LightEnergy / energyFactor;
    }
}
