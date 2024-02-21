using Godot;
using System;

public partial class ToggleVis : Node3D
{
    private bool on;
    [Export] public string name;

    public bool On
    {
        get => on;
        set
        {
            on = value;
            Visible = value;
        }
    }
}
