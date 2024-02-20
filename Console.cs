using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Screen.Commands;

namespace Screen;

public partial class Console : VBoxContainer
{
    [Export] private PackedScene textPrefab;
    [Export] private LineEdit input;
    public readonly List<Command> CommandList = new ();
    private Node sound;

    public override void _Ready()
    {
        sound = FindChild("Sound");
        foreach (Node n in GetChildren())
            if (n is Command c) CommandList.Add(c);
        
        foreach (Command c in CommandList)
            c.Init(this);
        
        Print("SecurOS V1.6.2");
        Print("Remember to read the manual.");
    }

    public void _on_text_changed(string _)
    {
        sound.Call("stop");
        sound.Call("play");
        GD.Print("test");
    }
    
    public void _on_line_edit_text_submitted(string text)
    {
        if(text == "") return;
        string[] args = text.ToLower().Split(" ");
        bool found = false;
        foreach (Command c in CommandList)
        {
            if (args[0] == c.Name)
            {
                c.Run(args);
                Reactor.Energy -= c.PowerCost;
                found = true;
            }
        }
        if(!found) Print("Command \"" + args[0] + "\" not recognised.");
        input.Clear();
    }

    public void Print(string text, string prefix = "")
    {
        string[] texts = text.Split("|");
        foreach (var t in texts)
        {
            RichTextLabel label = textPrefab.Instantiate<RichTextLabel>();
            AddChild(label);
            label.Text = prefix + t;
        }
    }
}