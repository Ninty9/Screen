using Godot;
using Screen.Scripts;

namespace Screen;

public partial class CamPos : Node3D
{
    [Export] public CamPos Left;
    [Export] public CamPos Right;
    [Export] public CamPos Up;
    [Export] public CamPos Down;
    [Export] public bool isDoor;

    public void OnLook()
    {
        if (isDoor)
            GhostManager.lookAtDoor();
    }
}