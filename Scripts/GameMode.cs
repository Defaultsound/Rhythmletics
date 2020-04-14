using Godot;
using System;
using Steamworks;
using FlatBuffers;
using System.Linq;
public class GameMode : SteamLobby
{

    public override void _Ready()
    {
        base._Ready();

        Steamworks.SteamNetworking.OnP2PSessionRequest += OnP2PSessionRequest;
        Steamworks.SteamNetworking.OnP2PConnectionFailed += OnP2PConnectionFailed;


        foreach (var member in  RhythmleticsGlobal.CurrentLobby.Members) 
        {
            AddNewPlayer(member);
        }
    }

    public override void _Process(float delta)
    {
        while (Steamworks.SteamNetworking.IsP2PPacketAvailable(0)) 
        {
            var IncomingData = Steamworks.SteamNetworking.ReadP2PPacket(0);
            ByteBuffer Buffer = new ByteBuffer(IncomingData.Value.Data);
            var IncomingPacket = NetworkPacket.PlayerInformation.GetRootAsPlayerInformation(Buffer);

            foreach (KinematicBody player in GetTree().GetNodesInGroup("Players")) 
            {
                if(player.Name != RhythmleticsGlobal.ClientSteamId.ToString()) 
                {
    
                    if(player.Name == IncomingPacket.ID)
                    {
                        player.Translation = new Vector3(IncomingPacket.Position.Value.X,IncomingPacket.Position.Value.Y,IncomingPacket.Position.Value.Z);
                        player.RotationDegrees = new Vector3(IncomingPacket.Rotation.Value.X,IncomingPacket.Rotation.Value.Y,IncomingPacket.Rotation.Value.Z);
                    }
                }

            }
        }
    }

    public override void OnLobbyMemberJoined(Steamworks.Data.Lobby Lobby, Friend Friend) 
    {
        GD.Print(Friend + " Joined");
        Steamworks.SteamNetworking.CloseP2PSessionWithUser(Friend.Id);
        AddNewPlayer(Friend);
        RhythmleticsGlobal.CurrentLobby.SendChatString("EV_BEGIN");
    }

    public override void OnLobbyMemberLeave(Steamworks.Data.Lobby Lobby, Friend Friend)
    {
        Steamworks.SteamNetworking.CloseP2PSessionWithUser(Friend.Id);
        GD.Print("Player Left " + Friend.Id);
        RemovePlayer(Friend);

    }

    public void PacketMulticast()
    {

    }

    public void OnP2PSessionRequest(Steamworks.SteamId Friend)
    {
        GD.Print("Request from: " + Friend);
        var ConnectingFriend = (Friend?)RhythmleticsGlobal.CurrentLobby.Members.FirstOrDefault(m => m.Id == Friend);
        if (ConnectingFriend != null)
        {
            Steamworks.SteamNetworking.AcceptP2PSessionWithUser(Friend);
            GD.Print("You have accepted incoming connection from " + ConnectingFriend.Value.Name); 
        }
        
    }

    public void OnP2PConnectionFailed(Steamworks.SteamId Friend, Steamworks.P2PSessionError Error)
    {
        GD.Print("P2P session failed. Error code: " + Error);
    }

    public void RemovePlayer(Friend Friend)
    {
        foreach (Node player in GetTree().GetNodesInGroup("Players")) 
        {
            if(Friend.Id.ToString() == player.Name)
            {
                GD.Print("Removed: " + player.Name);
                player.QueueFree();
            }
        }
    }

    public void AddNewPlayer(Friend Friend)
    {
        var ANewPlayer = ResourceLoader.Load("res://Scenes/Player.tscn") as PackedScene;
        var NewPlayer = ANewPlayer.Instance();
        NewPlayer.Name = Friend.Id.ToString();
        NewPlayer.GetNode("Viewport/Control/PlayerName").Set("text",Friend.Name);
        NewPlayer.Set("ControllerId", Friend.Id.ToString());
        AddChild(NewPlayer);
        NewPlayer.Set("translation",new Vector3(0,6,0));
    }

}
