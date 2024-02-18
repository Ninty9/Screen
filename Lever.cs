using Godot;
using System;

public partial class Lever : Node3D
{
    [Export] private float upSpeed;
    [Export] private float downDuration;
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
        tween = GetTree().CreateTween();
        tween.SetEase(downEase);
        tween.TweenProperty(this, "position", down.Position, downDuration);
        held = true;
        GD.Print("start");
    }
    
    public void _on_mouse_exited()
    {
        held = false;
        tween.Stop();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(held) return;
        float speed = upSpeed * (float)delta;
        
        if (Position.DistanceTo(up.Position) > speed)
            Position += (up.Position - Position).Normalized() * upSpeed * (float)delta;
        else if(Position.DistanceTo(up.Position) != 0)
        {
            Position = up.Position;
            GD.Print("stop");
        }
    }

    private void StopGen()
    {
        GD.Print("stop");
    }
}
