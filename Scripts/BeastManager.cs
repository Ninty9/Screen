using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Screen.Commands;

namespace Screen.Scripts;

public partial class BeastManager : Node
{
    [Export] private beast beast;
    public static double moveInterval = 7;
    private List<BeastPos> posList = new ();
    private double tick;
    private Random rnd;
    private ToggleVis roomLazer;
    private ToggleVis hallwayTurret;
    private int blastCounter;
    private Node turretSound;
    public static Rooms currentRoom = Rooms.RoomSleep;
    
    
    public override void _Ready()
    {
        foreach (Node n in GetChildren())
            if (n is BeastPos c) posList.Add(c);
        rnd = new Random();
        
        roomLazer = GetNode<ToggleVis>("/root/Main/room/RoomLazer");
        hallwayTurret = GetNode<ToggleVis>("/root/Main/screen/Turret");
        turretSound = FindChild("TurretSound");
        MoveToPos(currentRoom);
    }

    public override void _Process(double delta)
    {
        if(!BootCommand.boot) return;
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
                if(rnd.Next(0,5) == 0)
                    MoveToPos(room: Rooms.RoomAtDoor);
                break;
            case Rooms.RoomAtDoor:
                if (roomLazer.On)
                {
                    MoveToPos(room: Rooms.RoomSleep);
                    break;
                }
                
                MoveToPos(room: Rooms.RoomOutside);

                break;
            case Rooms.RoomOutside:
                if (rnd.Next(0, 2) != 1) break;
                
                switch (rnd.Next(0, 3))
                {
                    case 0:
                        if(GhostManager.currentRoom is GhostManager.Rooms.HallwayBeforeTurret or GhostManager.Rooms.HallwayAfterTurret or GhostManager.Rooms.HallwayDoor) 
                            return;
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
                if (rnd.Next(0, 2) != 1) break;
                if (!hallwayTurret.On)
                {
                    MoveToPos(Rooms.HallwayAfterTurret);
                    beast.GetChild(0).Call("play");
                    moveInterval = 2;
                    break;
                }
                if (!Reactor.modEnergy(-20))
                {
                    Console.console.Print("Not enough power to shoot turret");
                    MoveToPos(Rooms.HallwayAfterTurret);
                    beast.GetChild(0).Call("play");
                    moveInterval = 2;
                    break;
                }
                
                GD.Print("bang");
                turretSound.Call("play");
                MoveToPos(room: Rooms.RoomSleep);
                return;
            case Rooms.HallwayAfterTurret:
                if (rnd.Next(0, 2) != 1) return;
                MoveToPos(room: Rooms.HallwayDoor);
                break;
            case Rooms.HallwayDoor:
                GetTree().ChangeSceneToFile("res://death.tscn");
                break;
            case Rooms.ReactorApproach:
                blastCounter = 0;
                MoveToPos(room: Reactor.modEnergy(-10, true) ? Rooms.ReactorEating : Rooms.HallwayBeforeTurret);
                break;
            case Rooms.ReactorEating:
                if (!Reactor.modEnergy(-10, true))
                {
                    MoveToPos(room: Rooms.HallwayBeforeTurret);
                    break;
                }
                if (Reactor.Reacting)
                    blastCounter++;

                if (blastCounter > 1) MoveToPos(room: Rooms.RoomSleep);
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