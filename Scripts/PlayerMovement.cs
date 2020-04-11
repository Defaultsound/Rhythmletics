using Godot;
using System;
using Steamworks;

public class PlayerMovement : KinematicBody
{

    Global RhythmleticsGlobal;
    Vector3 CurrentVelocity;
    Vector3 Gravity = Vector3.Down * 12;
    
    public String ControllerId;


    public override void _Ready()
    {
        RhythmleticsGlobal = GetNode("/root/RhythmleticsGlobal") as Global;
        var PlayerCamera = GetNode("Camera") as Camera;

        if (ControllerId == RhythmleticsGlobal.ClientSteamId.ToString()) 
        {
            PlayerCamera.MakeCurrent();
        }
    }

    public override void _Process(float delta)
    {
        CurrentVelocity.x = 0;
        CurrentVelocity.z = 0;

        CurrentVelocity += Gravity * delta;

        if (ControllerId == RhythmleticsGlobal.ClientSteamId.ToString()) 
        {
        
            if (Input.IsActionPressed("move_forward"))
            {  
                CurrentVelocity += Transform.basis.x * 3;
            }

            if (Input.IsActionPressed("turn_left"))
            {  
                RotationDegrees = new Vector3(0,RotationDegrees.y + 5,0);
            }

            if (Input.IsActionPressed("turn_right"))
            {  
                RotationDegrees = new Vector3(0,RotationDegrees.y - 5,0);
            }

            MovePlayer();

        }
        else 
        {
            MovePlayer();
        }

    }

    
    public void MovePlayer() 
    {
        CurrentVelocity = MoveAndSlide(CurrentVelocity, Vector3.Up,true);
    }
}


