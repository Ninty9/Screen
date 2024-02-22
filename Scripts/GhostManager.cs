using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Screen.Commands;

namespace Screen.Scripts;

public partial class GhostManager : Node
{
    
    [Export] private Node3D ghost;
    public static double moveInterval = 10;
    private List<GhostPos> posList = new ();
    private double tick;
    private Random rnd;
    private ToggleVis roomLazer;
    private ToggleVis hallwayTurret;
    private int blastCounter;
    private Node turretSound;
    public static Rooms currentRoom = Rooms.Away;
    private static GhostManager ghostManager;
    

    public override void _Ready()
    {
        foreach (Node n in GetChildren())
            if (n is GhostPos c) posList.Add(c);
        rnd = new Random();
        ghostManager = this;
        roomLazer = GetNode<ToggleVis>("/root/Main/room/RoomLazer");
        hallwayTurret = GetNode<ToggleVis>("/root/Main/screen/Turret");
        turretSound = FindChild("TurretSound");
        MoveToPos(currentRoom);
    }

    public static async void lookAtDoor()
    {
        if(currentRoom == Rooms.HallwayDoor)
            ghostManager.MoveToPos(Rooms.Away);
        await Task.Delay(500);
        ghostManager.ghost.GetChild(0).Call("stop");
    }

    public override void _Process(double delta)
    {
        if(!BootCommand.boot) return;
        tick += delta;

        if (tick > moveInterval)
        {
            tick = 0;
            Ai();

        }
    }

    private void Ai()
    {
        switch (currentRoom)
        {
            case Rooms.Away:
                switch (rnd.Next(0,4))
                {
                    case 0:
                        if(BeastManager.currentRoom is BeastManager.Rooms.HallwayBeforeTurret or BeastManager.Rooms.HallwayAfterTurret or BeastManager.Rooms.HallwayDoor) 
                            return;
                        MoveToPos(Rooms.HallwayBeforeTurret);
                        break;
                    case 1:

                        MoveToPos(SatCommand.rescue ? Rooms.HallwayBeforeTurret : Rooms.ServerApproach);
                        break;
                    case 2:
                        MoveToPos(Rooms.Reactor);
                        break;
                }
                
                break;
            case Rooms.HallwayBeforeTurret:
                if (!hallwayTurret.On)
                {
                    MoveToPos(Rooms.HallwayAfterTurret);
                    break;
                }
                if (!Reactor.modEnergy(-20))
                {
                    Console.console.Print("Not enough power to shoot turret");
                    MoveToPos(Rooms.HallwayAfterTurret);
                    
                    break;
                }
                GD.Print("bang");
                turretSound.Call("play");
                MoveToPos(Rooms.HallwayAfterTurret);
                break;
            case Rooms.HallwayAfterTurret:
                //todo: turn lights off?
                if (CamManager.CamMan.camPos.isDoor)
                {
                    MoveToPos(Rooms.Away);
                    return;
                }
                ghostManager.ghost.GetChild(0).Call("play");
                MoveToPos(Rooms.HallwayDoor);
                break;
            case Rooms.HallwayDoor:
                //todo: kill player
                GetTree().ChangeSceneToFile("res://death.tscn");
                break;
            case Rooms.ServerApproach:
                MoveToPos(SatCommand.Calculating ? Rooms.ServerEating : Rooms.Away);
                break;
            case Rooms.ServerEating:
                MoveToPos(Rooms.Away);
                break;
            case Rooms.Reactor:
                switch (rnd.Next(0,4))
                {
                    case 0:
                        MoveToPos(Rooms.HallwayBeforeTurret);
                        break;
                    case 1:
                        MoveToPos(Rooms.ServerApproach);
                        break;
                    case 2:
                        MoveToPos(Rooms.Away);
                        break;
                }
                break;
        }

        switch (currentRoom)
        {
            case Rooms.ServerApproach when SatCommand.Calculating:
                Console.console.Print("Unknown entity in server rooms.");
                break;
            case Rooms.ServerEating when SatCommand.Calculating:
                SatCommand.KillServers();
                break;
        }
    }

    public void MoveToPos(Rooms room)
    {
        GhostPos pos;
        try
        {
            pos = posList.First(GhostPos => GhostPos.room == room);
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
            return;
        }
        currentRoom = room;
        ghost.Position = pos.Position;
        ghost.Rotation = pos.Rotation;
    }

    public enum Rooms
    {
        Away,
        HallwayBeforeTurret,
        HallwayAfterTurret,
        HallwayDoor,
        ServerApproach,
        ServerEating,
        Reactor,
    }
}