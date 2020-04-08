using Godot;
using System;

public class Lobby : Node
{

    Button HostButton;
    Label StatusLabel;
    Label LobbyLabel;
    VBoxContainer PlayerList;
    Steamworks.Data.Lobby CurrentLobby;
    public override void _Ready()
    {
        
        HostButton = GetParent().GetNode("Host/HostBtn") as Button;
        StatusLabel = GetParent().GetNode("StatusLabel") as Label;
        LobbyLabel = GetParent().GetNode("LobbyLabel") as Label;
        PlayerList = GetParent().GetNode("PlayerListScrollContainer/PlayerList") as VBoxContainer;

        //Steamworks Event Bindings
        Steamworks.SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        Steamworks.SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        Steamworks.SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
        Steamworks.SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberLeave;
        Steamworks.SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;

    }

    private void _on_HostBtn_pressed()
    {
        var lobby = Steamworks.SteamMatchmaking.CreateLobbyAsync(4);
    }

    public void OnLobbyCreated(Steamworks.Result Result, Steamworks.Data.Lobby Lobby)
    {
        CurrentLobby = Lobby;
        Lobby.SetPublic();
        SetLobbyDetails();
        StatusLabel.Set("text", "Status: Hosting, Waiting For Players");
    }

    public void OnLobbyEntered(Steamworks.Data.Lobby Lobby)
    {
        CurrentLobby = Lobby;
        SetLobbyDetails();
    }

    public void OnLobbyMemberJoined(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend)
    {
       SetLobbyDetails();
    }

    public void OnGameLobbyJoinRequested(Steamworks.Data.Lobby Lobby, Steamworks.SteamId SteamID)
    {
        Steamworks.SteamMatchmaking.JoinLobbyAsync(Lobby.Id);
    }
    public void OnLobbyMemberLeave(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend)
    {
        GD.Print(Friend.Name);
        var PlayerToRemove = PlayerList.GetNode("GUI/PlayerListScrollContainer/PlayerList/" + Friend.Name);
        PlayerToRemove.QueueFree();
    }

    public void SetLobbyDetails()
    {
        LobbyLabel.Set("text", "Lobby ID: " + CurrentLobby.Id.ToString());
        foreach (var member in CurrentLobby.Members) 
        {
            AddToPlayerList(member.Name);
        }

    } 
    public void AddToPlayerList(String FriendName) 
    {
        var NewPlayer = new Label();
        NewPlayer.Name = FriendName;
        NewPlayer.Set("text",FriendName);
        PlayerList.AddChild(NewPlayer,false);
    }

    public override void _Notification(int what) 
    {
        if (what == MainLoop.NotificationWmQuitRequest) 
        {
            CurrentLobby.Leave();
        }
    }
}
