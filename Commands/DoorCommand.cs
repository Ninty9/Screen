    using Godot;

namespace Screen.Commands;

public partial class DoorCommand : Command
{
    
    //array of doors
    public override void Run(string[] args)
    {
        if (!BootCommand.boot)
        {
            Con.Print("Enable system first.");
            return;
        }
        if (args.Length == 1)
        {
            Con.Print("List of doors:");
            Con.Print("r: bug room door", "- ");
        }
        if (args.Length < 3)
        {
            Con.Print("Not enough args: door [doorID] [on/off]");
            return;
        }
        ToggleVis t;
        switch (args[1])
        {
            case "r":
                t = GetNode<ToggleVis>("/root/Main/room/RoomLazer");
                break;
            default:
                Con.Print("Door not found.");
                return;
        }
        
        switch (args[2])
        {
            case "on":
                t.On = true;
                Con.Print("Door on/closed.");
                break;
            case "off":
                t.On = false;
                Con.Print("Door off/open.");
                break;
            default:
                Con.Print(args[2] + " not recognised. use 'on' or 'off'");
                break;
        }
    }
}