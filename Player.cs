using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    
    public float maxHealth = 100f;
    [SyncVar]
    public float currentHealth;

    public Behaviour[] disableOnDeath;
    public GameObject[] graphics;
    public bool[] wasEnabled;

    public GameObject deathEffect;
    public GameObject spawnEffect;

    void Start()
    {
        wasEnabled = new bool[disableOnDeath.Length];
    }
    
    public void PlayerSetup()
    {
        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    public void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    public void RpcSetupPlayerOnAllClients()
    {
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    [ClientRpc]
    public void RpcTakeDamage(float  amount, string killer)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;
        Debug.Log(transform.name + " has " + currentHealth);

        if(currentHealth <= 0)
        {
            Die(killer);
        }
    }

    private void Die(string killer)
    {
        isDead = true;

        GameObject newDeathEffect = Instantiate(deathEffect, transform.position, transform.rotation * Quaternion.Euler(-90, 0, 0), null);
        Destroy(newDeathEffect, 2);

        GameObject playerUI = GetComponent<PlayerSetup>().playerUIInstance;
        if (playerUI != null)
        {
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        foreach(GameObject graphic in graphics)
        {
            graphic.SetActive(false);
        }

        Debug.Log(transform.name + " is dead!");

        StartCoroutine(Respawn());
        StartCoroutine(LookAtKiller(killer));
        
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        GameObject newSpawnEffect = Instantiate(spawnEffect, spawnPoint.position, spawnPoint.rotation * Quaternion.Euler(-90, 0, 0), null);
        Destroy(newSpawnEffect, 2);
        SetDefaults();
    }

    IEnumerator LookAtKiller(string killer)
    {
        float timePassed = 0;
        while (timePassed < GameManager.instance.matchSettings.respawnTime)
        {
            timePassed += Time.deltaTime;
            GetComponent<PlayerShoot>().cam.transform.rotation = Quaternion.LookRotation(GameManager.GetPlayer(killer).gameObject.transform.position - transform.position, (transform.position - GetComponent<PlayerMotor>().dominantBody.transform.position).normalized);
            yield return null;
        }
    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
        foreach (GameObject graphic in graphics)
        {
            graphic.SetActive(true);
        }
        GameObject playerUI = GetComponent<PlayerSetup>().playerUIInstance;

        if (playerUI != null)
        {
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }       
    }
}

