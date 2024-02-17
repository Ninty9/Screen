namespace Screen.Commands;

public class BogosCommand : Command
{
    public override string Name { get; set; } = "bogos";
    protected override Console Con { get; set; }
    public BogosCommand(Console c) : base(c)
    {
        
    }

    public override void Run(string[] args)
    {
        Con.Print("bogos binted");
    }


}