using Godot;
using System;
using Steamworks;

public class SteamLobby : Node
{
    public Global RhythmleticsGlobal;


    public SteamLobby()
    {
        Steamworks.SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        Steamworks.SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        Steamworks.SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
        Steamworks.SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
        Steamworks.SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberDisconnected;
        Steamworks.SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
        Steamworks.SteamMatchmaking.OnChatMessage += OnChatMessage;
    }

    public override void _Ready() 
    {
        RhythmleticsGlobal = GetNode("/root/RhythmleticsGlobal") as Global;
    }

    public override void _ExitTree()
    {
        Steamworks.SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        Steamworks.SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        Steamworks.SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyMemberJoined;
        Steamworks.SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
        Steamworks.SteamMatchmaking.OnLobbyMemberDisconnected -= OnLobbyMemberDisconnected;
        Steamworks.SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;
    }

    //Main Delegate Implementations
    public virtual void OnLobbyCreated(Steamworks.Result Result, Steamworks.Data.Lobby Lobby)
    {
        RhythmleticsGlobal.CurrentLobby = Lobby;
        RhythmleticsGlobal.CurrentLobby.SetPublic();
        GD.Print("Lobby Set to Public");
    }

    public virtual void OnLobbyEntered(Steamworks.Data.Lobby Lobby)
    {
                
    }
    public virtual void OnGameLobbyJoinRequested(Steamworks.Data.Lobby Lobby, Steamworks.SteamId SteamID)
    {
        Steamworks.SteamMatchmaking.JoinLobbyAsync(Lobby.Id);
    }

    public virtual void OnLobbyMemberJoined(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend)
    {

    }

    public virtual void OnLobbyMemberLeave(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend)
    {

    }
    public virtual void OnLobbyMemberDisconnected(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend)
    {

    }

    public virtual void OnChatMessage(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend, String Message)
    {

    }

    public override void _Notification(int what) 
    {
        if (what == MainLoop.NotificationWmQuitRequest) 
        {
             RhythmleticsGlobal.CurrentLobby.Leave();
             RhythmleticsGlobal.CurrentLobby = default;
        }
    }
}
