using Godot;
using System;
using Steamworks;

public class GameMode : SteamLobby
{

    public override void _Ready()
    {
        base._Ready();
        
        foreach (var member in  RhythmleticsGlobal.CurrentLobby.Members) 
        {
            AddNewPlayer(member);
        }
    }

    public override void OnLobbyMemberJoined(Steamworks.Data.Lobby Lobby, Friend Friend) 
    {
        AddNewPlayer(Friend);
        RhythmleticsGlobal.CurrentLobby.SendChatString("EV_BEGIN");
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
