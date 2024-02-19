using System.Linq;
using Godot;

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

        string s = args.Skip(1).ToArray().Join(" ");
        Con.Print(s + " binted" + (s == "bogos" ? "!" : "" ));
    }


}