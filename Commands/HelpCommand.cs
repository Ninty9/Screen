using System;

namespace Screen.Commands;

public partial class HelpCommand : Command
{
    public override void Run(string[] args)
    {
        foreach (Command c in Con.CommandList)
        {
            Con.Print(c.Name);
            Con.Print(c.Description, "- ");
        }
    }
}