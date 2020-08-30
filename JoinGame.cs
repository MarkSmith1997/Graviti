using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {
    
    [SerializeField]
    List<GameObject> roomList = new List<GameObject>();

    public GameObject roomListItemPrefab;
    public Transform roomListParent;
    public NetworkManager networkManager;
    public Text statusText;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        statusText.color = Color.white;
        ClearRoomList();
        roomList.Clear();
        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        statusText.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        statusText.text = "";
        if(matches == null)
        {
            statusText.text = "Couldn't get room list";
            return;
        }

        ClearRoomList();
        foreach (MatchInfoSnapshot match in matches)
        {
            GameObject roomListItemGO = Instantiate(roomListItemPrefab);
            roomListItemGO.transform.SetParent(roomListParent);

            RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem>();
            if(roomListItem != null)
            {
                roomListItem.Setup(match, JoinRoom);
            }

            roomList.Add(roomListItemGO);
        }

        if(roomList.Count == 0)
        {
            statusText.text = "No rooms at the moment";
        }
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
    }

    public void JoinRoom(MatchInfoSnapshot match)
    {
        networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        statusText.text = "Joining...";
        StartCoroutine(ConnectionTimeOut());
    }

    IEnumerator ConnectionTimeOut()
    {
        yield return new WaitForSeconds(12f);
        statusText.color = Color.red;
        statusText.text = "Error: Connection timed out, refresh and try again";
    }
}
