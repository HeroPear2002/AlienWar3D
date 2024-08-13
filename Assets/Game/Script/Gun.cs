using UnityEngine;
using UnityEngine.InputSystem.XR;
[System.Serializable]
public class Gun
{
    public int bulletnum = 100;
    public string gunName;
    public float damage;
    public float fireRate;
    public float bulletSpeed;
    private float nextFireTime;
    public Gun(string name, float dmg, float rate, float speed)
    {
        gunName = name;
        damage = dmg;
        fireRate = rate;
        bulletSpeed = speed;
    }

    public void Shoot(Transform barrelTransform, GameObject bulletPrefab, Transform bulletParent, float bulletHitMissDistance)
    {
        if (barrelTransform == null)
        {
            Debug.LogError("Barrel transform is null!");
            return;
        }
        if (Time.time >= nextFireTime)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            RaycastHit hit;
            if (Physics.Raycast(barrelTransform.position, barrelTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                bulletController.target = barrelTransform.position + barrelTransform.forward * bulletHitMissDistance;
                bulletController.hit = true;
            }
            if (bulletController != null)
            {
                bulletController.SetSpeed(bulletSpeed);
                bulletController.SetDamage(damage);
            }
            else
            {
                Debug.LogError("Bullet prefab does not contain a BulletController component!");
            }
            bulletnum -= 1;
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
    public void ShootShotGun(Transform barrelTransform, GameObject bulletPrefab,Transform bulletParent, float bulletHitMissDistance)
    {
        if (barrelTransform == null)
        {
            Debug.LogError("Barrel transform is null!");
            return;
        }
        if (Time.time < nextFireTime)
        {
            return;
        }
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.Normalize();
            randomDirection *= 0.2f;
            Vector3 newPosition = barrelTransform.position + new Vector3(randomDirection.x, randomDirection.y, randomDirection.z);
            GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            Vector3 targetPosition = newPosition + barrelTransform.forward * 10f ;
            bulletController.target = targetPosition;
            bulletController.hit = true;
            if (bulletController != null)
            {
                bulletController.SetSpeed(bulletSpeed);
                bulletController.SetDamage(damage);
            }
            else
            {
                Debug.LogError("Bullet prefab does not contain a BulletController component!");
            }
        }
        bulletnum -= 10;
        nextFireTime = Time.time + 1f / fireRate;
    }
}
