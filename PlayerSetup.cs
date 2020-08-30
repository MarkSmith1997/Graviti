using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
    
    [SerializeField]
    Behaviour[] compoenentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    public GameObject playerUIPrefab;
    public GameObject playerUIInstance;

    Camera sceneCamera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisabledComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
            GetComponent<Player>().PlayerSetup();
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netID, player);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisabledComponents()
    {
        for (int i = 0; i < compoenentsToDisable.Length; i++)
        {
            compoenentsToDisable[i].enabled = false;
        }
    }


    void OnDisable()
    {

        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }

}
