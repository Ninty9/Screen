using System.Collections.Generic;
using Godot;
using Screen.Commands;

namespace Screen;

public partial class Console : LineEdit
{
    [Export] private PackedScene textPrefab;
    [Export] private LineEdit input;
    private static List<Command> CommandList = new ();

    public override void _Ready()
    {
        CommandList.Add(new BogosCommand(this));
    }

    public void _on_mouse_entered()
    {
        GrabFocus();
    }
    
    public void _on_mouse_exited()
    {
        ReleaseFocus();
    }
    
    public void _on_text_submitted(string text)
    {
        string[] args = text.ToLower().Split(" ");
        bool found = false;
        foreach (Command c in CommandList)
        {
            if (args[0] == c.Name)
            {
                c.Run(args);
                found = true;
            }
        }
        if(!found) Print("Command \"" + args[0] + "\" not recognised.");
        input.Clear();
    }

    public void Print(string text)
    {
        RichTextLabel label = textPrefab.Instantiate<RichTextLabel>();
        AddChild(label);
        label.Text = text;
        MoveChild(input.GetParent(), GetChildCount()-1);
    }
}