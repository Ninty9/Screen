using Godot;

namespace Screen.Commands;

public abstract class Command
{
    public abstract string Name { get; set; }
    protected abstract Console Con { get; set; }
    protected Command(Console c)
    {
        Con = c;
    }
    
    public abstract void Run(string[] args);
    

}