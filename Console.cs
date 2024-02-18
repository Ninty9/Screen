using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Screen.Commands;

namespace Screen;

public partial class Console : VBoxContainer
{
    [Export] private PackedScene textPrefab;
    [Export] private LineEdit input;
    [Export] public Command[] commandList;

    public override void _Ready()
    {
        foreach (Command c in commandList)
        {
            c.Init(this);
        }
    }
    
    public void _on_line_edit_text_submitted(string text)
    {
        if(text == "") return;
        string[] args = text.ToLower().Split(" ");
        bool found = false;
        foreach (Command c in commandList)
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