using Godot;

namespace Screen;

public partial class CamManager : Node3D
{
    [Export] private Camera3D camera;
    [Export] public CamPos camPos;
    [Export] private float moveDuration;
    [Export] private LineEdit input;
    private bool moving;

    [Export] private Label left;
    [Export] private Label right;
    [Export] private Label up;
    [Export] private Label down;
    [Export] private Label tutorial;

    public static CamManager CamMan;

    public override void _Ready()
    {
        CamMan = this;
        CamMove(camPos);
    }

    private void CamSwitch(Dirs dir)
    {
        tutorial.Visible = false;
        if(moving) return;
        switch (dir)
        {
            case Dirs.Up:
                CamMove(camPos.Up);
                break;
            case Dirs.Down:
                CamMove(camPos.Down);
                break;
            case Dirs.Left:
                CamMove(camPos.Left);
                break;
            case Dirs.Right:
                CamMove(camPos.Right);
                break;
        }
    }

    private void CamMove(CamPos pos)
    {
        if (pos == null)
            return;
        moving = true;
        camPos = pos;
        camPos.OnLook();
        Tween tweenPos = GetTree().CreateTween();
        tweenPos.TweenProperty(camera, "position", camPos.Position, moveDuration).SetEase(ease: Tween.EaseType.Out).SetTrans(trans: Tween.TransitionType.Quad);
        tweenPos.TweenCallback(Callable.From(ResetMove));
        Tween tweenRot = GetTree().CreateTween();
        tweenRot.TweenProperty(camera, "rotation", camPos.Rotation, moveDuration).SetEase(ease: Tween.EaseType.InOut).SetTrans(trans: Tween.TransitionType.Expo);
        left.Visible = camPos.Left != null;
        right.Visible = camPos.Right != null;
        up.Visible = camPos.Up != null;
        down.Visible = camPos.Down != null;
    }

    private void ResetMove()
    {
        moving = false;
    }

    public override void _Process(double delta)
    {
        if(input.HasFocus()) return;
        if(Input.IsActionPressed("up"))
            CamSwitch(Dirs.Up);
        if(Input.IsActionPressed("down"))
            CamSwitch(Dirs.Down);
        if(Input.IsActionPressed("left"))
            CamSwitch(Dirs.Left);
        if(Input.IsActionPressed("right"))
            CamSwitch(Dirs.Right);
        
    }

    public void _on_line_edit_mouse_exited()
    {
        input.ReleaseFocus();
    }

    // public override void _Input(InputEvent @event)
    // {
    //     if(@event is not InputEventKey key) return;
    //     switch (key.KeyLabel)
    //     {
    //         case Key.W:
    //             CamSwitch(Dirs.Up);
    //             break;
    //         case Key.A:
    //             CamSwitch(Dirs.Left);
    //             break;
    //         case Key.S:
    //             CamSwitch(Dirs.Down);
    //             break;
    //         case Key.D:
    //             CamSwitch(Dirs.Right);
    //             break;
    //     }
        // Vector2 screenSize = GetViewport().GetVisibleRect().Size;
        // // Mouse in viewport coordinates.
        // if (@event is not InputEventMouseMotion eventMouseMotion) return;
        // position = eventMouseMotion.Position;
        // if (position.X > screenSize.X - (screenSize.X / 15))
        //     CamSwitch(Dirs.Right);
        // else if (position.X < screenSize.X / 15)
        //     CamSwitch(Dirs.Left);
        // else if (position.Y > screenSize.Y - (screenSize.Y / 10))
        //     CamSwitch(Dirs.Down);
        // else if (position.Y < screenSize.Y / 10)
        //     CamSwitch(Dirs.Up);
    // }
    
    private enum Dirs
    {
        Up,
        Down,
        Left,
        Right,
    }
}