using UnityEngine;

namespace HCID274._Weapons
{
    public class WeaponHandler : MonoBehaviour 
    {
        public static WeaponHandler instance;
        public Gun primaryGun;
        public Gun secondaryGun;

        private Gun currentGun; 
        private GameObject currentGunPrefab; 
        

        private void Awake()
        {
            if (instance == null)
            {
                instance = this; 
            }
            else if (instance != this)
            {
                Destroy(this); 
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchToPrimary();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchToSecondary();   
            }
        }

        public void PickUpGun(Gun gun)
        {
            if (primaryGun == null)
            {
                primaryGun = gun;
                SwitchToPrimary();
            }
            else if (secondaryGun == null)
            {
                secondaryGun = gun; 
                SwitchToSecondary(); 
            }
            else
            {
                if (currentGun == primaryGun)
                {
                    // 实例化掉落的枪支预制体
                    Instantiate(primaryGun.gunPickup, transform.position + transform.forward, Quaternion.identity);
                    primaryGun = gun;
                    SwitchToPrimary();
                }
                else
                {
                    Instantiate(secondaryGun.gunPickup, transform.position + transform.forward, Quaternion.identity);
                    secondaryGun = gun; 
                    SwitchToSecondary(); 
                }
            }
        }

        private void SwitchToPrimary()
        {
            if (primaryGun != null)
            {
                EquipGun(primaryGun); 
            }
        }

        private void SwitchToSecondary()
        {
            if (secondaryGun != null)
            {
                EquipGun(secondaryGun); 
            }
        }


        private void EquipGun(Gun gun)
        {
            currentGun = gun; 
            if (currentGunPrefab != null)
            {
                Destroy(currentGunPrefab); 
            }

            currentGunPrefab = Instantiate(gun.gameObject, transform);
            AmmunitionManager.instance.ammunitionUI.UpdateAmmunitionType(currentGun);
        }
    }
}
