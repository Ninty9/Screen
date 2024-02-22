namespace Screen.Commands;

public partial class BootCommand : Command
{
    public static bool boot;
    public override void Run(string[] args)
    {
        boot = true;
        Con.Print("Good luck.");
    }
}