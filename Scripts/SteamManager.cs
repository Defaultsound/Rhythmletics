using Godot;
using System;
using Steamworks;

public class SteamManager : Node
{

	public override void _Ready()
	{
		try
		{

			if (Steamworks.SteamClient.RestartAppIfNecessary((AppId)480))
			{
				GD.Print("restarting through steam...");
				GetTree().Quit();
			}

			Steamworks.SteamClient.Init(480, true);

			if (Steamworks.SteamClient.IsValid)
			{
				GD.Print(Steamworks.SteamClient.Name);
			}
			else
			{
				GD.Print("Failed to Initalize Steam. Please ensure that the Steam Client is Running");
			}

		}
		catch (System.DllNotFoundException e)
		{
			GD.Print(e);
		}

	}


}
