using Godot;
using System;

public class Lobby : Node
{
    Global RhythmleticsGlobal;
    Button HostButton;
    Button ContinueButton;
    Label StatusLabel;
    Label LobbyLabel;
    VBoxContainer PlayerList;

    public override void _Ready()
    {
        
        RhythmleticsGlobal = GetNode("/root/RhythmleticsGlobal") as Global;
        HostButton = GetParent().GetNode("Host/HostBtn") as Button;
        ContinueButton = GetParent().GetNode("Host/ContinueBtn") as Button;
        StatusLabel = GetParent().GetNode("StatusLabel") as Label;
        LobbyLabel = GetParent().GetNode("LobbyLabel") as Label;
        PlayerList = GetParent().GetNode("PlayerListScrollContainer/PlayerList") as VBoxContainer;

        //Steamworks Event Bindings
        Steamworks.SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        Steamworks.SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        Steamworks.SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
        Steamworks.SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
        Steamworks.SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberDisconnected;
        Steamworks.SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;

    }

    private void _on_HostBtn_pressed()
    {
        var HostLabelText = HostButton.GetNode("HostLabel").Get("text") as String;
        if (HostLabelText == "Host") 
        {
            var lobby = Steamworks.SteamMatchmaking.CreateLobbyAsync(4);
            ContinueButton.Visible = true;
        }
        else 
        {
            LeaveLobby();
            HostButton.GetNode("HostLabel").Set("text","Host");
            ContinueButton.Visible = false;
        }
    }

    public void _on_ContinueBtn_pressed()
    {
        GetTree().ChangeScene("Scenes/World.tscn");
    }

    //Main Delegate Implementations
    public void OnLobbyCreated(Steamworks.Result Result, Steamworks.Data.Lobby Lobby)
    {
        Lobby.SetPublic();
    }

    public void OnLobbyEntered(Steamworks.Data.Lobby Lobby)
    {
        RhythmleticsGlobal.CurrentLobby = Lobby;
        SetLobbyDetails();
        if (Lobby.IsOwnedBy(Steamworks.SteamClient.SteamId)) 
        {
            StatusLabel.Set("text", "Status: Hosting, Waiting For Players");
            HostButton.GetNode("HostLabel").Set("text","Cancel");
        }
        else 
        {
            StatusLabel.Set("text", "Status: Waiting in Lobby");
            HostButton.GetNode("HostLabel").Set("text","Leave");
        }

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
        RemovePlayerFromList(Friend);
        GD.Print("Left");
    }
    public void OnLobbyMemberDisconnected(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend)
    {
        RemovePlayerFromList(Friend);
    }

    //Functions for Handling UI Updates
    public void SetLobbyDetails()
    {
        LobbyLabel.Set("text", "Lobby ID: " +  RhythmleticsGlobal.CurrentLobby.Id.ToString());
        foreach (var member in  RhythmleticsGlobal.CurrentLobby.Members) 
        {
            AddToPlayerList(member.Name);
        }

    } 

    public void AddToPlayerList(String FriendName) 
    {
        if (!PlayerList.HasNode(FriendName))
        {
            var NewPlayer = new Label();
            NewPlayer.Name = FriendName;
            NewPlayer.Set("text",FriendName);
            PlayerList.AddChild(NewPlayer,false);
        }

    }

    public void RemovePlayerFromList(Steamworks.Friend Friend) 
    {
        var PlayerToRemove = PlayerList.GetNode(Friend.Name);
        PlayerToRemove.QueueFree();
    }

    public void LeaveLobby()
    {
        RhythmleticsGlobal.CurrentLobby.Leave();
        RhythmleticsGlobal.CurrentLobby = default;
        LobbyLabel.Set("text", "Lobby ID: ");
        StatusLabel.Set("text", "Status: Idle...");
        foreach (Node Child in PlayerList.GetChildren()) 
        {
            Child.QueueFree();
        }
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
