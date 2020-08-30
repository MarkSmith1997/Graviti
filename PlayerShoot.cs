using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    private const string PlayerTag = "Player";

    public Camera cam;
    public LayerMask mask;

    public PlayerWeapon currentWeapon;
    public WeaponManager weaponManager;

    private void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        if (!GetComponent<PlayerController>().actMenu)
        {
            if (currentWeapon.fireRate <= 0f)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    GetComponent<PlayerController>().speed = GetComponent<PlayerController>().shootSpeed;
                    InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    GetComponent<PlayerController>().speed = GetComponent<PlayerController>().normalSpeed;
                    CancelInvoke("Shoot");
                }
            }
        }
        if (GetComponent<PlayerController>().actMenu)
        {
            CancelInvoke("Shoot");
        }
    }

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(weaponManager.GetCurrentEffects().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 0.2f);
    }

    [Command]
    void CmdOnShoot()
    {
        RpcDoGunEffect();
    }

    [ClientRpc]
    void RpcDoGunEffect()
    {
        weaponManager.GetCurrentEffects().muzzleFlash.Play();
    }

    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdOnShoot();

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            if (hit.collider.tag == PlayerTag)
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage, transform.name);
            }

            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]
    void CmdPlayerShot(string playerID, float damage, string shooterID)
    {
        Debug.Log(shooterID + " has shot " + playerID + " dealing " + damage.ToString());
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage, shooterID);
    }

    private void OnDisable()
    {
        GetComponent<PlayerController>().speed = GetComponent<PlayerController>().normalSpeed;
        CancelInvoke("Shoot");
    }
}