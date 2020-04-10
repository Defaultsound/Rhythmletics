using Godot;
using System;
using Steamworks;

public class Global : Node
{
    public Steamworks.Data.Lobby CurrentLobby;
    public override void _Ready()
    {
        
    }
    public override void _Process(float delta) 
    {
        GD.Print(CurrentLobby.Id);
    }
}
