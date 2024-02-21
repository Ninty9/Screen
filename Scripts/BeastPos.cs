using Godot;
using System;

public partial class BeastPos : Node3D
{
    [Export] public Screen.Scripts.BeastManager.Rooms room;
    public Node3D FR;
    public Node3D FL;
    public Node3D BR;
    public Node3D BL;
    public override void _Ready()
    {
        FR = GetChild<Node3D>(0);
        FL = GetChild<Node3D>(1);
        BR = GetChild<Node3D>(2);
        BL = GetChild<Node3D>(3);

    }
}
