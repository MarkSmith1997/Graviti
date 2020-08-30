using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public MatchSettings matchSettings;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }
    }

    #region Player tracking

    public const string PlayerIDPrefix = "Player ";

    public static Dictionary<string, Player> Players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PlayerIDPrefix + netID;
        Players.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static void UnRegisterPlayer(string playerID)
    {
        Players.Remove(playerID);
    }

    public static Player GetPlayer(string playerID)
    {
        return Players[playerID];
    }

    #endregion

}
