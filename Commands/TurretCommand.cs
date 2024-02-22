using Godot;

namespace Screen.Commands;

public partial class TurretCommand : Command
{
    
    //array of doors
    public override void Run(string[] args)
    {
        if (!BootCommand.boot)
        {
            Con.Print("Enable system first.");
            return;
        }
        if (args.Length < 2)
        {
            Con.Print("Not enough args: turret [on/off].");
            return;
        }
        ToggleVis hallwayTurret = GetNode<ToggleVis>("/root/Main/screen/Turret");;
        
        switch (args[1])
        {
            case "on":
                hallwayTurret.On = true;
                Con.Print("Turret on.");
                break;
            case "off":
                hallwayTurret.On = false;
                Con.Print("Turret off.");
                break;
            default:
                Con.Print(args[2] + " not recognised. use 'on' or 'off'");
                break;
        }
    }
}