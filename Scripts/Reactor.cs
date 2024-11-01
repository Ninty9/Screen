using Godot;
using Screen.Commands;
using Screen.Scripts;

namespace Screen;

public partial class Reactor : SpotLight3D
{
    public static int Energy { get; private set; }
    [Export] private float gainPerSecond;
    private double secCount;
    public static bool Reacting;
    [Export] private Node sound;
    public static Reactor React { get; private set; }


    public static bool modEnergy(int i, bool drainPartial = false)
    {
        if (i < 0 && Energy < i * -1)
        {
            if(drainPartial) Energy = 0;
            return false;
        }
        
        Energy += i;
        Energy = Mathf.Clamp(Energy, 0, 100);
        return true;
    }
    public override void _Ready()
    {
        Energy = 30;
        React = this;
    }

    public static void SetReactor(bool on)
    {
        if(on == Reacting) return;
        Reacting = on;
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
        if(BeastManager.currentRoom == BeastManager.Rooms.ReactorEating) return;
        secCount += delta;
        if (Reacting && secCount > 1f / gainPerSecond)
        {
            Energy += 1;
            secCount = 0;
        }
    
        Energy = Mathf.Clamp(Energy, 0, 100);
    }
}