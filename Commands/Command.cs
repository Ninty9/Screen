using Godot;

namespace Screen.Commands;

public abstract partial class Command : Node
{
    [Export] public string Description;
    [Export] public int PowerCost;
    protected Console Con { get; private set; }
    public void Init(Console c)
    {
        Con = c;
    }
    
    public abstract void Run(string[] args);
    

}