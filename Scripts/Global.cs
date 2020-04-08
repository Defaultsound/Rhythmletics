using Godot;
using System;

public class Global : Node
{
    public bool PlayingAsHost;
    public Steamworks.SteamId GlobalLobbyID;
    public Steamworks.SteamId PlayerOne;
    public Steamworks.SteamId PlayerTwo;
    public override void _Ready()
    {

    }

}
