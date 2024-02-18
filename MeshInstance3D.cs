using Godot;
using System;

public partial class MeshInstance3D : Godot.MeshInstance3D
{
    public override void _Ready()
    {
        Tween tweenPos = GetTree().CreateTween().SetLoops();
        tweenPos.TweenProperty(this, "position", Position + Vector3.Up, 2f);
        tweenPos.TweenProperty(this, "position", Position - Vector3.Up, 2f);
    }
}
