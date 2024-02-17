namespace Screen.Commands;

public class DoorCommand : Command
{
    public DoorCommand(Console c) : base(c)
    {
    }

    public override string Name { get; set; } = "door";
    protected override Console Con { get; set; }
    
    public override void Run(string[] args)
    {
        //find door args[1]
        //args[2] is command, ie: open, close, lock
    }
}