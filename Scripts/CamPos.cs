using Godot;

namespace Screen;

public partial class CamPos : Node3D
{
    [Export] public CamPos Left;
    [Export] public CamPos Right;
    [Export] public CamPos Up;
    [Export] public CamPos Down;
}