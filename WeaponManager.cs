using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

    public string weaponLayerName = "Weapon";

    public Transform weaponHolder;

    public PlayerWeapon primaryWeapon;
    public PlayerWeapon currentWeapon;
    public WeaponEffects currentEffects;


    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponEffects GetCurrentEffects()
    {
        return currentEffects;
    }

    void EquipWeapon(PlayerWeapon weapon)
    {
        currentWeapon = weapon;
        GameObject weaponInstance = Instantiate(weapon.graphics, weaponHolder.position, weaponHolder.rotation, weaponHolder);

        currentEffects = weaponInstance.GetComponent<WeaponEffects>();
        if (currentEffects == null)
        {
            Debug.Log("No WeaponEffects component on the weapon object: " + weaponInstance.name);
        }

        if (isLocalPlayer)
        {
            Util.SetPlayerRecursively(weaponInstance, LayerMask.NameToLayer(weaponLayerName));
        }

    }

}
