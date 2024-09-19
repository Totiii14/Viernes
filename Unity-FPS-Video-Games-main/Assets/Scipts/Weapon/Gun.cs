using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public string gunName; 
    public GameObject gunPickup;

    [Header("Stats")]
    public AmmunitionTypes ammunitionType;
    public int minmumDamage;
    public int maxmumDamage; 
    public float maxmumRange;
    public float recoilAmount = 1.0f;
    public float recoilRecoveryFactor = 1.0f; 
    public AudioClip shootAudioClip;
    public float fireRate;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;
    public float bulletSpeed = 20f;


    protected float timeOfLastShot;

    private Transform cameraTransform;
    private PlayerCameraController playerCameraController;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        playerCameraController = cameraTransform.GetComponent<PlayerCameraController>();
    }

    private Vector3 GetAimDirection()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            return (hit.point - firePoint.position).normalized;
        }
        else
        {
            return cameraTransform.forward;
        }
    }

    protected void Fire()
    {
        if (AmmunitionManager.instance.ConsumeAmmunition(ammunitionType))
        {
            AudioSource.PlayClipAtPoint(shootAudioClip, transform.position);
            Vector3 aimDirection = GetAimDirection();
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(aimDirection));

            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.minimumDamage = minmumDamage;
            bulletController.maximumDamage = maxmumDamage;
            bulletController.maximumRange = maxmumRange;
            bulletController.speed = bulletSpeed;
            bulletController.gun = this; 

            Destroy(bullet, 10f); 
        }

        //float verticalRecoil = Random.Range(recoilAmount * 0.02f, recoilAmount * 0.03f);
        //float horizontalRecoil = Random.Range(-recoilAmount * 0.0125f, recoilAmount * 0.0125f);

        //Vector2 initialLookingPos = playerCameraController.GetCurrentLookingPos();

        //Vector2 recoil = new Vector2(horizontalRecoil, verticalRecoil);
        //playerCameraController.ApplyRecoilWithRecovery(recoil, recoil.magnitude * recoilRecoveryFactor, initialLookingPos);
    }
}