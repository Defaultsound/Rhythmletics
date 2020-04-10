using Godot;
using System;
using Steamworks;

public class Global : Node
{
    public Steamworks.Data.Lobby CurrentLobby;

    public Steamworks.SteamId ClientSteamId;
    public override void _Ready()
    {
        
    }
    public override void _Process(float delta) 
    {

    }
}
