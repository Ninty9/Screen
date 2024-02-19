using Godot;

namespace Screen;

public partial class MatchEnergy : SpotLight3D
{
    [Export] private float energyFactor = 4f;
    public override void _Process(double delta)
    {
        LightEnergy = ((float)Reactor.Energy / 100) * energyFactor;
    }
}