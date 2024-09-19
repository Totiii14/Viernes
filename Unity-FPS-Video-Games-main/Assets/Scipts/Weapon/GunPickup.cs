using HCID274._Weapons;
using UnityEngine;


public class GunPickup : MonoBehaviour, ILootable
{
    [SerializeField] private Gun gun;


    public void OnStartLook()
    {

    }

    public void OnInteract()
    {
        WeaponHandler.instance.PickUpGun(gun);
        Destroy(gameObject); 
    }


    public void OnEndLook()
    {
 
    }
}
