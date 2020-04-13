using Godot;
using System;
using Steamworks;
using FlatBuffers;
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
            GD.Print(IncomingData);
            ByteBuffer Buffer = new ByteBuffer(0);
        }
    }

    public override void OnLobbyMemberJoined(Steamworks.Data.Lobby Lobby, Friend Friend) 
    {
        AddNewPlayer(Friend);
        RhythmleticsGlobal.CurrentLobby.SendChatString("EV_BEGIN");
    }

    public void OnP2PSessionRequest(Steamworks.SteamId Friend)
    {
        foreach (var member in  RhythmleticsGlobal.CurrentLobby.Members) 
        {
            if(member.Id == Friend) 
            {
                Steamworks.SteamNetworking.AcceptP2PSessionWithUser(Friend);
                GD.Print("You have accepted incoming connection from " + member.Name);
            }
        }
    }

    public void OnP2PConnectionFailed(Steamworks.SteamId Friend, Steamworks.P2PSessionError Error)
    {
        GD.Print("P2P session failed. Error code: " + Error);
    }


    public void AddNewPlayer(Friend Friend)
    {
        var ANewPlayer = ResourceLoader.Load("res://Scenes/Player.tscn") as PackedScene;
        var NewPlayer = ANewPlayer.Instance();
        NewPlayer.Name = Friend.Name;
        NewPlayer.GetNode("Viewport/Control/PlayerName").Set("text",Friend.Name);
        NewPlayer.Set("ControllerId", Friend.Id.ToString());
        AddChild(NewPlayer);
        NewPlayer.Set("translation",new Vector3(0,6,0));
    }

}
