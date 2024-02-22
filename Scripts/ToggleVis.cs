using Godot;
using System;
using Screen;
using Console = Screen.Console;

public partial class ToggleVis : Node3D
{
    private bool on;
    [Export] public string name;
    [Export] public float drainPerSec;
    private double draincount;
    public override void _Process(double delta)
    {
        if(!On) return;
        draincount += delta;
        if (draincount > 1f / drainPerSec)
        {
            draincount = 0;
            if (!Reactor.modEnergy(-1))
            {
                On = false;
                                    Console.console.Print(name + " has turned off due to lack of power.");
            }
        }
    }

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
