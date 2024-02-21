using Godot;

namespace Screen.Commands;

public partial class DoorCommand : Command
{
    
    //array of doors
    public override void Run(string[] args)
    {
        if (args.Length < 3)
            return;
        ToggleVis l;
        switch (args[1])
        {
            case "r":
                l = GetNode<ToggleVis>("/root/Main/room/RoomLazer");
                break;
            default:
                Con.Print("Door not found.");
                return;
        }
        
        switch (args[1])
        {
            case "on":
                l.On = true;
                break;
            case "off":
                l.On = false;
                break;
            default:
                Con.Print(args[2] + " not recognised. use 'on'/'off'");
                break;
        }
    }
}