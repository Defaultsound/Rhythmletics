using Godot;
using System;
using Steamworks;

public class GameMode : Node
{
    Global RhythmleticsGlobal;

    public override void _Ready()
    {
        RhythmleticsGlobal = GetNode("/root/RhythmleticsGlobal") as Global;

        foreach (var member in  RhythmleticsGlobal.CurrentLobby.Members) 
        {
            var ANewPlayer = ResourceLoader.Load("res://Scenes/Player.tscn") as PackedScene;
            var NewPlayer = ANewPlayer.Instance();
            NewPlayer.GetNode("Viewport/Control/PlayerName").Set("text",member.Name);
            AddChild(NewPlayer);
            NewPlayer.Set("translation",new Vector3(0,6,0));
        }
    }

}
