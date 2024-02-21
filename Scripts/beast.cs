using Godot;

// ReSharper disable InconsistentNaming

namespace Screen;

public partial class beast : Node3D
{
#pragma warning disable CS0618 // Type or member is obsolete
    public SkeletonIK3D FR;
    public SkeletonIK3D FL;
    public SkeletonIK3D BR;
    public SkeletonIK3D BL;

    public override void _Ready()
    {
        Node3D skel = GetChild(0).GetChild<Node3D>(0);
        FR = skel.GetChild<SkeletonIK3D>(1);
        FL = skel.GetChild<SkeletonIK3D>(2);
        BR = skel.GetChild<SkeletonIK3D>(3);
        BL = skel.GetChild<SkeletonIK3D>(4);
    }
}