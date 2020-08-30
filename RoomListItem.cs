using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;


public class RoomListItem : MonoBehaviour {

    public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
    public JoinRoomDelegate joinRoomCallBack;

    public Text roomNameText;
    public Text roomSizeText;

    public MatchInfoSnapshot match;

    public void Setup(MatchInfoSnapshot newMatch, JoinRoomDelegate newJoinRoomCallBack)
    {
        match = newMatch;
        joinRoomCallBack = newJoinRoomCallBack;

        roomNameText.text = match.name;
        roomSizeText.text = "(" + match.currentSize + "/" + match.maxSize + ")";
    }

    public void JoinRoom()
    {
        joinRoomCallBack.Invoke(match);
    }

}
