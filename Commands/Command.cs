using Godot;

namespace Screen.Commands;

public abstract partial class Command : Resource
{
    [Export] public string Name;
    [Export] public string Description;
    protected Console Con { get; private set; }
    public void Init(Console c)
    {
        Con = c;
    }
    
    public abstract void Run(string[] args);
    

}