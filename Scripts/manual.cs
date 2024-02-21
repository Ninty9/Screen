using Godot;
using System;

public partial class manual : AnimatedSprite3D
{
    private bool mouse;
    public void _mouse_enter()
    {
        mouse = true;
        GD.Print(mouse);
    }
    
    public void _mouse_exit()
    {
        mouse = false;
    }
    public override void _Input(InputEvent ev)
    {
        if (!mouse || ev is not InputEventMouseButton button || button.Pressed == false) return;
        switch (button.ButtonIndex)
        {
            case MouseButton.Left:
                Frame++;
                break;
            case MouseButton.Right:
                Frame--;
                break;
            default:
                return;
        }

        Frame = Mathf.Clamp(Frame, 0, 2);
    }
}
