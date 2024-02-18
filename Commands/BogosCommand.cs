namespace Screen.Commands;

public partial class BogosCommand : Command
{
    
    public override void Run(string[] args)
    {
        if(args.Length < 2)
        {
            Con.Print("bogos binted");
            return;
        }
        Con.Print(args[1] + " binted" + (args[1] == "bogos" ? "!" : "" ));
    }


}