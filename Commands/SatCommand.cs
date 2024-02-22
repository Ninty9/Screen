using System;
using System.Threading.Tasks;
using Godot;
using Screen.Scripts;

namespace Screen.Commands;

public partial class SatCommand : Command
{
    public static bool Calculating;
    private bool calibrated;
    private double calcTime;
    [Export] private float calculationLength;
    public static bool rescue;
    private double reTime;
    [Export] private float reLength;
    private int x;
    private int y;

    private Random rnd = new ();

    private static SatCommand satCommand;
    public override void _Ready()
    {
        satCommand = this;
    }

    public static void KillServers()
    {
        satCommand.KillS();
    }

    private async void KillS()
    {
        Calculating = false;
        calcTime = 0;
        Con.Print("Uh Oh! Looks like the server's crashed.");
        await Task.Delay(500);
        Con.Print("Rebooting.");
        await Task.Delay(1000);
        Con.Print("It looks like your calculations didn't save properly, restarting them now.");
    }
    public override void Run(string[] args)
    {
        if (!BootCommand.boot)
        {
            Con.Print("Enable system first.");
            return;
        }

        if (rescue)
        {
            Con.Print("A squad is already on its way.");
            return;
        }
        if (args.Length == 1)
        {
            Con.Print("Not enough arguments. usage: sat [calc/setup/send]");
            return;
        }

        switch (args[1])
        {
            case "calc":
                Calc(args);
                break;
            case "setup":
                Setup(args);
                break;
            case "send":
                Send(args);
                break;
            default:
                Con.Print(args[1] + " not recognised. use 'calc', 'setup', or 'send'");
                break;
        }
    }

    private void Calc(string[] args)
    {
        if (args.Length < 3)
        {
            Con.Print("Not enough arguments.");
            return;
        }
        switch (args[2])
        {
            case "start":
                if(calcTime > calculationLength)
                {
                    Con.Print("Calculations already finished.");
                    break;
                }
                if (Calculating)
                {
                    Con.Print("Already calculating.");
                    break;
                }
                if(calcTime > 0)
                {
                    Calculating = true;
                    Con.Print("Resuming calculations.");
                    break;
                }
                Calculating = true;
                Con.Print("Starting calculations.");
                break;
            case "pause":
                if (!Calculating)
                {
                    Con.Print("Calculations need to be running to pause them.");
                    break;
                }
                Calculating = false;
                Con.Print("Paused calculations.");
                break;
            default:
                Con.Print(args[1] + " not recognised. use 'start' or 'pause'");
                break;
        }
    }

    private void Setup(string[] args)
    {
        if (args.Length < 4)
        {
            Con.Print("Not enough arguments. Usage: sat setup [x] [y]");
        }
        if (args[2] == x.ToString() && args[3] == y.ToString())
        {
            Con.Print("Connection established!");
            calibrated = true;
        }
        else
        {
            Con.Print("Connection failed.");
            calibrated = false;
        }
    }
    
    private void Send(string[] args)
    {
        if (args.Length < 3)
        {
            Con.Print("Not enough arguments.");
            return;
        }
        if (!calibrated)
        {
            Con.Print("No connection to HQ.");
            return;
        }
        switch (args[2])
        {
            case "2-b":
                Con.Print("Understood. A rescue team will be on the way shortly.");
                //todo: good ending
                rescue = true;
                BeastManager.moveInterval = 5;
                GhostManager.moveInterval = 6;
                break;
            case "1-a":
                Con.Print("Sorry our coffee machine is broken. Good luck.");
                ChangeScene("res://badEnding.tscn");
                //todo: bad ending
                break;
            default:
                Con.Print(args[1] + " is not a recognised code.");
                ChangeScene("res://badEnding.tscn");
                break;
        }
    }

    private async void ChangeScene(string path)
    {
        await Task.Delay(4000);
        GetTree().ChangeSceneToFile(path);
    }
    
    private void Done()
    {
        Calculating = false;
        Con.Print("Calculations finished.");
        x = rnd.Next(1, 100);
        y = rnd.Next(1, 100);
        Con.Print("Coordinates are: " + x + ",  " + y + ".");
        Con.Print("Use 'sat setup [x] [y]' to calibrate, and 'sat send [code]' to send out status code.");
    }

    public override void _Process(double delta)
    {
        if (Calculating) calcTime += delta;
        if (rescue) reTime += delta;
        if(calcTime > calculationLength && Calculating) Done();
        if(reTime > reLength && rescue) ChangeScene("res://goodEnding.tscn");;
        if (Calculating && Reactor.Energy < 10)
        {
            Con.Print("Energy low, pausing calculations.");
            Calculating = false;
        }   
    }
}