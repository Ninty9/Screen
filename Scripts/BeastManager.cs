using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Screen.Scripts;

public partial class BeastManager : Node
{
    [Export] private beast beast;
    [Export] private double moveInterval = 5;
    private List<BeastPos> posList = new ();
    private double tick;
    private Random rnd;
    private ToggleVis roomLazer;
    private ToggleVis hallwayTurret;
    private int blastCounter;
    [Export] private Rooms currentRoom = Rooms.RoomSleep;


    public override void _Ready()
    {
        foreach (Node n in GetChildren())
            if (n is BeastPos c) posList.Add(c);
        rnd = new Random();
        
        roomLazer = GetNode<ToggleVis>("/root/Main/room/RoomLazer");
        hallwayTurret = GetNode<ToggleVis>("/root/Main/screen/Turret");
        MoveToPos(currentRoom);
    }

    public override void _Process(double delta)
    {
        tick += delta;

        if (tick > moveInterval)
        {
            Ai();
            tick = 0;
        }
    }

    private void Ai()
    {
        switch (currentRoom)
        {
            case Rooms.RoomSleep:
                if(rnd.Next(0,2) == 1)
                    MoveToPos(room: Rooms.RoomAtDoor);
                break;
            case Rooms.RoomAtDoor:
                if (rnd.Next(0, 2) != 1) return;
                
                MoveToPos(room: hallwayTurret.On ? Rooms.RoomSleep : Rooms.RoomOutside);

                break;
            case Rooms.RoomOutside:
                if (rnd.Next(0, 2) != 1) return;
                
                switch (rnd.Next(0, 3))
                {
                    case 0:
                        MoveToPos(room: Rooms.HallwayBeforeTurret);
                        break;
                    case 1:
                        MoveToPos(room: Rooms.ReactorApproach);
                        break;
                    case 2:
                        break;
                }
                break;
            case Rooms.HallwayBeforeTurret:
                if (rnd.Next(0, 2) != 1) return;
                if (true) //turret on
                {
                    //gunshot sound
                    MoveToPos(room: Rooms.RoomSleep);
                    return;
                }
                MoveToPos(Rooms.HallwayAfterTurret);
                break;
            case Rooms.HallwayAfterTurret:
                if (rnd.Next(0, 2) != 1) return;
                MoveToPos(room: Rooms.HallwayDoor);
                break;
            case Rooms.HallwayDoor:
                //kills u
                break;
            case Rooms.ReactorApproach:
                blastCounter = 0;
                Reactor.Energy -= 10;
                Reactor.Energy = Mathf.Clamp(Reactor.Energy, 0, 100);
                MoveToPos(room: Rooms.ReactorEating);
                break;
            case Rooms.ReactorEating:
                Reactor.Energy -= 20;
                Reactor.Energy = Mathf.Clamp(Reactor.Energy, 0, 100);
                if (Reactor.Reacting)
                {
                    blastCounter++;
                }
                
                if(blastCounter > 2) MoveToPos(room: Rooms.RoomSleep);
                break;
        }
    }

    public void MoveToPos(Rooms room)
    {
        BeastPos pos;
        try
        {
            pos = posList.First(beastPos => beastPos.room == room);
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
            return;
        }
        currentRoom = room;
        beast.Position = pos.Position;
        beast.Rotation = pos.Rotation;
        beast.FR.TargetNode = pos.FR.GetPath();
        beast.FL.TargetNode = pos.FL.GetPath();
        beast.BR.TargetNode = pos.BR.GetPath();
        beast.BL.TargetNode = pos.BL.GetPath();
    }

    public enum Rooms
    {
        RoomSleep,
        RoomAtDoor,
        RoomOutside,
        HallwayBeforeTurret,
        HallwayAfterTurret,
        HallwayDoor,
        ReactorApproach,
        ReactorEating,
    }
}