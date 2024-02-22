using Godot;
using Screen.Commands;

namespace Screen;

public partial class Lever : Node3D
{
    [Export] private float upSpeed;
    [Export] private float downSpeed;
    [Export] private Tween.EaseType upEase;
    [Export] private Tween.EaseType downEase;
    private Node3D up;
    private Node3D down;
    private bool held;
    private Tween tween;

    public override void _Ready()
    {
        up = GetParent().FindChild("Up") as Node3D;
        down =  GetParent().FindChild("Down") as Node3D;
        Position = up.Position;
    }

    public void _on_mouse_entered()
    {
        held = true;
    }
    
    public void _on_mouse_exited()
    {
        held = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(held)
        {
                
            float speed = downSpeed * (float)delta;
            if (Position.DistanceTo(up.Position) > down.Position.DistanceTo(up.Position) / 3f && !BootCommand.boot) return;
            if (Position.DistanceTo(down.Position) > speed)
                Position += (down.Position - Position).Normalized() * upSpeed * (float)delta;
            else if(Position.DistanceTo(down.Position) != 0)
                Position = down.Position;

            if (Position.DistanceTo(down.Position) < down.Position.DistanceTo(up.Position)/2)
                Reactor.SetReactor(true);
        }
        else
        {
            float speed = upSpeed * (float)delta;
        
            if (Position.DistanceTo(up.Position) > speed)
                Position += (up.Position - Position).Normalized() * upSpeed * (float)delta;
            else if(Position.DistanceTo(up.Position) != 0)
            {
                Position = up.Position;
                Reactor.SetReactor(false);
            }

        }
        
    }

    private void StopGen()
    {
        GD.Print("stop");
    }
}