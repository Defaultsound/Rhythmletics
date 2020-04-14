using Godot;
using System;
using Steamworks;
using FlatBuffers;
public class PlayerMovement : KinematicBody
{

    Global RhythmleticsGlobal;
    Vector3 CurrentVelocity;
    Vector3 Gravity = Vector3.Down * 12;
    
    public String ControllerId;

    bool sendPacketReady = true;

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
            transferPlayerMovement();

        }

    }

    public void MovePlayer() 
    {
        CurrentVelocity = MoveAndSlide(CurrentVelocity, Vector3.Up,true);
    }

    private void transferPlayerMovement() 
    {
        if (sendPacketReady && (RhythmleticsGlobal.LobbyHost != RhythmleticsGlobal.ClientSteamId)) 
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(8);
            var name = builder.CreateString(ControllerId);

            NetworkPacket.PlayerInformation.StartPlayerInformation(builder);

            NetworkPacket.PlayerInformation.AddID(builder,name);
            NetworkPacket.PlayerInformation.AddPosition(builder,NetworkPacket.Vec3.CreateVec3(builder, Transform.origin.x,Transform.origin.y,Transform.origin.z));
            NetworkPacket.PlayerInformation.AddRotation(builder,NetworkPacket.Vec3.CreateVec3(builder, RotationDegrees.x,RotationDegrees.y,RotationDegrees.z));
            
            var StopBuilding = NetworkPacket.PlayerInformation.EndPlayerInformation(builder);
            builder.Finish(StopBuilding.Value);

            byte[] packet = builder.SizedByteArray();

            SteamNetworking.SendP2PPacket(RhythmleticsGlobal.LobbyHost,packet,(int)packet.Length, 0, Steamworks.P2PSend.Unreliable);
        } 
    }
}


