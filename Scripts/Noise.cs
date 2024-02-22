using Godot;
using System;

public partial class Noise : Sprite3D
{
    private NoiseTexture2D t;
    private Random rnd;
    public override void _Ready()
    {
        base._Ready();
        t = Texture as NoiseTexture2D;
        rnd = new Random();
    }

    public override void _Process(double delta)
    {
        t.Noise.Set("seed", rnd.Next(0,100));
    }
}
