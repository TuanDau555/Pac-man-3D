using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shoot settings")]
    public int damage = 25;
    public float shootRange = 100f;
    public LayerMask enemyLayers;
    [Header("Gun settings")]
    public Transform weaponHolder;
    public GameObject currentWeapon;
    
    [Header("Ammo settings")]
    public int maxAmmo = 10;

    [HideInInspector]
    public int currentAmmo;
    public bool hasWeapon = false;
    private void Start()
    {
        currentAmmo = maxAmmo;
    }
    public void EquipWeapon(GameObject weaponPrefab, GameObject pickupObjectOnGround)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(weaponPrefab, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.Euler(0, 180, 0);
        hasWeapon = true;
        currentAmmo = maxAmmo;
        Debug.Log("Weapon equipped");
        Debug.Log($"Đã nhặt súng - Đạn: {currentAmmo}/{maxAmmo}");

        if (pickupObjectOnGround != null)
        {
            Debug.Log("Object destroying"+ pickupObjectOnGround.name);
            pickupObjectOnGround.SetActive(false);
            Destroy(pickupObjectOnGround, 0.1f);
            Debug.Log("Pickup object destroyed");
        }
    }
     void Update()
    {
     if (hasWeapon && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    void Shoot()
    {
     if (currentAmmo <= 0)
        {
            Debug.Log("Het Dan");
            DropOrDestroyCurrentWeapon();
            return;
        }
        currentAmmo--;
        Debug.Log($"Bắn! Còn {currentAmmo}/{maxAmmo} viên");

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, shootRange, enemyLayers))
        {
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
    private void DropOrDestroyCurrentWeapon()
    {
        if (currentWeapon != null) {
            Destroy(currentWeapon);
            currentWeapon = null;
            hasWeapon = false;
            Debug.Log("Weapon was destroyed because ammo =0");
    }
    }
}
