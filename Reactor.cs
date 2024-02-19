using Godot;

namespace Screen;

public partial class Reactor : SpotLight3D
{
    public static int Energy;
    [Export] private float gainPerSecond;
    private double secCount;
    private static bool reacting;
    [Export] private Node sound;
    public static Reactor React { get; private set; }

    public override void _Ready()
    {
        React = this;
    }

    public static void SetReactor(bool on)
    {
        if(on == reacting) return;
        reacting = on;
        if (on)
            React.LightsOn();
        else
            React.LightsOff();
    }

    private void LightsOn()
    {
        sound.Call("set", "parameter_on", 1);
        Tween tween = GetTree().CreateTween();
        tween.TweenInterval(0.25f);
        tween.TweenProperty(this, "light_energy", 16, 0.2f);
        tween.TweenProperty(this, "light_energy", 0, 0);
        tween.TweenProperty(this, "light_energy", 32, 1f);
    }
    
    private void LightsOff()
    {
        sound.Call("set", "parameter_on", 0);
        Tween tween = GetTree().CreateTween();
        tween.TweenInterval(0.25f);
        tween.TweenProperty(this, "light_energy", 16, 0.2f);
        tween.TweenProperty(this, "light_energy", 32, 0);
        tween.TweenInterval(0.2f);
        tween.TweenProperty(this, "light_energy", 0, 0.1f);
        tween.TweenProperty(this, "light_energy", 16, 0);
        tween.TweenInterval(0.1f);
        tween.TweenProperty(this, "light_energy", 0, 0f);
    }

    public override void _PhysicsProcess(double delta)
    {
        secCount += delta;
        if (reacting && secCount > 1f / gainPerSecond)
        {
            Energy += 1;
            secCount = 0;
        }
    
        Energy = Mathf.Clamp(Energy, 0, 100);
    }
}