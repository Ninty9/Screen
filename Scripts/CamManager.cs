using Godot;

namespace Screen;

public partial class CamManager : Node3D
{
    [Export] private Camera3D camera;
    [Export] private CamPos camPos;
    [Export] private float moveDuration;
    private Vector2 position;
    private bool moving;

    public override void _Ready()
    {
        camera.Position = camPos.Position;
        camera.Rotation = camPos.Rotation;
    }

    private void CamSwitch(Dirs dir)
    {
        if(moving) return;
        moving = true;
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
        camPos = pos;
        Tween tweenPos = GetTree().CreateTween();
        tweenPos.TweenProperty(camera, "position", camPos.Position, moveDuration).SetEase(ease: Tween.EaseType.Out).SetTrans(trans: Tween.TransitionType.Quad);
        tweenPos.TweenCallback(Callable.From(ResetMove));
        Tween tweenRot = GetTree().CreateTween();
        tweenRot.TweenProperty(camera, "rotation", camPos.Rotation, moveDuration).SetEase(ease: Tween.EaseType.InOut).SetTrans(trans: Tween.TransitionType.Expo);
    }

    private void ResetMove()
    {
        moving = false;
    }

    public override void _Input(InputEvent @event)
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;
        // Mouse in viewport coordinates.
        if (@event is not InputEventMouseMotion eventMouseMotion) return;
        position = eventMouseMotion.Position;
        if (position.X > screenSize.X - (screenSize.X / 15))
            CamSwitch(Dirs.Right);
        else if (position.X < screenSize.X / 15)
            CamSwitch(Dirs.Left);
        else if (position.Y > screenSize.Y - (screenSize.Y / 10))
            CamSwitch(Dirs.Down);
        else if (position.Y < screenSize.Y / 10)
            CamSwitch(Dirs.Up);
    }
    
    private enum Dirs
    {
        Up,
        Down,
        Left,
        Right,
    }
}