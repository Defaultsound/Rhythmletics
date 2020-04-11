using Godot;
using System;

public class LobbyGUI : SteamLobby
{
    Button HostButton;
    Button ContinueButton;
    Label StatusLabel;
    Label LobbyLabel;
    VBoxContainer PlayerList;
    
    public override void _Ready()
    {
        base._Ready();
        HostButton = GetParent().GetNode("Host/HostBtn") as Button;
        ContinueButton = GetParent().GetNode("Host/ContinueBtn") as Button;
        StatusLabel = GetParent().GetNode("StatusLabel") as Label;
        LobbyLabel = GetParent().GetNode("LobbyLabel") as Label;
        PlayerList = GetParent().GetNode("PlayerListScrollContainer/PlayerList") as VBoxContainer;
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
        TravelToWorld()
    }

    //Main Delegate Implementations

     public override void OnLobbyEntered(Steamworks.Data.Lobby Lobby)
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

    public override void OnLobbyMemberJoined(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend)
    {
       SetLobbyDetails();
    }

    public override void OnLobbyMemberLeave(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend)
    {
        RemovePlayerFromList(Friend);
    }
    public override void OnLobbyMemberDisconnected(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend)
    {
        RemovePlayerFromList(Friend);
    }

    public override void OnChatMessage(Steamworks.Data.Lobby Lobby, Steamworks.Friend Friend, string Message)
    {
        if(Message.BeginsWith("EV_")) 
        {
            if (Message.Equals("EV_BEGIN")) 
            {
                GetTree().ChangeScene("Scenes/World.tscn");
            }
        }
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

    public void TravelToWorld() 
    {
        RhythmleticsGlobal.CurrentLobby.SendChatString("EV_BEGIN");
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

}
