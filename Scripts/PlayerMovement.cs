using Godot;
using System;



public class PlayerMovement : KinematicBody
{

    Vector3 CurrentVelocity;
    Vector3 Gravity = Vector3.Down * 12;

    public override void _Ready()
    {
        
    }

 public override void _Process(float delta)
 {
     CurrentVelocity.x = 0;
     CurrentVelocity.z = 0;

     CurrentVelocity += Gravity * delta;

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


     CurrentVelocity = MoveAndSlide(CurrentVelocity, Vector3.Up,true);
 }
}
