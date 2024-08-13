using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(HeadController))]
public class PlayerMoverment : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 3f;
    [SerializeField] private float speedUp = 7f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private PlayerInput playerInput;
    [SerializeField] private float rotationSpeed = 15f;  // Tốc độ quay
    public Vector3 transformPlayer { get; set; }
    public bool trans = false;
    private Transform mainCamera;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction shiftAction;
    private InputAction changeGunAction;
    private InputAction aimAction;
    [SerializeField] GameObject bulletPrefabs;
    [SerializeField] private float bulletHitMissDistance = 25f;
    [SerializeField] Transform bulletParent;
    [SerializeField] private GameObject[] guns;
    [SerializeField] private Transform[] bulletOrigins;
    [SerializeField] private Rig[] RigGuns;
    private float currentSpeed = 3;
    public int currentGunIndex = 0;
    private Dictionary<GameObject, Transform> gunBulletOriginsMap;
    private Dictionary<int, Action<Transform>> shootActions = new Dictionary<int, Action<Transform>>();
    public Dictionary<int, Gun> gunDictionary = new Dictionary<int, Gun>();
    private bool isShooting = false;
    private GunsMenu buyGuns;
    Coroutine shootingCoroutine;
    [SerializeField] private TextMeshProUGUI bulletnum;
    private PlayerAudio pa;
    private void Awake()
    {
        pa = FindObjectOfType<PlayerAudio>();
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        buyGuns = FindObjectOfType<GunsMenu>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shooting"];
        aimAction = playerInput.actions["Aim"];
        changeGunAction = playerInput.actions["ChangeGun"];
        shiftAction = playerInput.actions["Shift"];
        mainCamera = Camera.main.transform;
        for (int i = 1; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
        }
        gunBulletOriginsMap = new Dictionary<GameObject, Transform>();
        if (guns.Length == bulletOrigins.Length)
        {
            for (int i = 0; i < guns.Length; i++)
            {
                gunBulletOriginsMap[guns[i]] = bulletOrigins[i];
            }
        }
        shootActions.Add(0, ShootType0);
        shootActions.Add(1, ShootType1);
        shootActions.Add(2, ShootType2);
        shootActions.Add(3, ShootType3);
        shootActions.Add(4, ShootType4);
        gunDictionary.Add(0, new Gun("Pistol", 100, 3f, 50));
        gunDictionary.Add(1, new Gun("Asset", 150, 10f, 60));
        gunDictionary.Add(2, new Gun("Shotgun", 60, 1f, 60));
        gunDictionary.Add(3, new Gun("SMG", 100, 15f, 60));
        gunDictionary.Add(4, new Gun("Sniper", 1000, 0.5f, 70));
        
    }
    private void OnEnable()
    {
        InputSystem.EnableDevice(InputSystem.GetDevice<Keyboard>());
        changeGunAction.performed += _ => ChangeGun();
        shootAction.performed += _ => StartShooting();
        shootAction.canceled += _ => StopShooting();
        shiftAction.performed += _ => SpeedUp();
        shiftAction.canceled += _ => SpeedDown();
    }
    private void OnDisable()
    {
        InputSystem.DisableDevice(InputSystem.GetDevice<Keyboard>());
        changeGunAction.performed -= _ => ChangeGun();
        shootAction.performed += _ => StartShooting();
        shootAction.canceled += _ => StopShooting();
        shiftAction.performed -= _ => SpeedUp();
        shiftAction.canceled -= _ => SpeedDown();
    }
    private void ShootGun()
    {
        if (isShooting && aimAction.IsPressed())
        {
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootContinuously());
            }
        }
        else
        {
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }
    }
    private void ChangeGun()
    {
        do
        {
            currentGunIndex++;
            if (currentGunIndex >= guns.Length)
            {
                currentGunIndex = 0;
            }
        } while (!buyGuns.IsGunBought(currentGunIndex));
        foreach (var gun in guns)
        {
            gun.SetActive(false);
        }
        foreach (var rig in RigGuns)
        {
            rig.weight = 0f;
        }
        guns[currentGunIndex].SetActive(true);
        RigGuns[currentGunIndex].weight = 1f;
        pa.PlayFootstepWalkSound();
    }
    void FixedUpdate()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(input.x, 0, input.y);
        movement = movement.x * mainCamera.right.normalized + movement.z * mainCamera.forward.normalized;
        movement.y = 0f;
        controller.Move(movement * Time.deltaTime * currentSpeed);
        if (movement.magnitude > 0f)
        {
            pa.PlayFootstepSound();
        }
        else
        {
            pa.StopFootstepSound();
        }
        RotateCharacter();

        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        bulletnum.text = ": " + gunDictionary[currentGunIndex].bulletnum;
        if (gameObject != null && trans)
        {
            transform.position = transformPlayer;
            trans = false;
        }
    }
    private void StartShooting()
    {
        isShooting = true;
        ShootGun();
    }
    private void StopShooting()
    {
        isShooting = false;
    }
    private void SpeedUp()
    {
        if (!aimAction.IsPressed())
        {
            currentSpeed = speedUp;
            pa.PlayFootstepRunSound();

        }
    }
    private void SpeedDown()
    {
        currentSpeed = playerSpeed;
        pa.PlayFootstepWalkSound();
    }
    void RotateCharacter()
    {
        Quaternion targetRotion = Quaternion.Euler(0, mainCamera.eulerAngles.y,0);
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotion,rotationSpeed * Time.deltaTime);
    }
    private void ShootType0(Transform barrelTransform)
    {
        if (currentGunIndex < 0 || currentGunIndex >= guns.Length)
        {
            Debug.LogError("Invalid current gun index!");
            return;
        }
        pa.PlayGunPitol();
        gunDictionary[currentGunIndex].Shoot(barrelTransform, bulletPrefabs, bulletParent, bulletHitMissDistance);
    }
    private void ShootType1(Transform barrelTransform)
    {
        if (currentGunIndex < 0 || currentGunIndex >= guns.Length)
        {
            Debug.LogError("Invalid current gun index!");
            return;
        }
        pa.PlayGunAsset();
        gunDictionary[currentGunIndex].Shoot(barrelTransform, bulletPrefabs, bulletParent, bulletHitMissDistance);
    }
    private void ShootType2(Transform barrelTransform)
    {
        if (currentGunIndex < 0 || currentGunIndex >= guns.Length)
        {
            Debug.LogError("Invalid current gun index!");
            return;
        }
        pa.PlayGunShotgun();
        gunDictionary[currentGunIndex].ShootShotGun(barrelTransform, bulletPrefabs, bulletParent, bulletHitMissDistance);
    }
    private void ShootType3(Transform barrelTransform)
    {
        if (currentGunIndex < 0 || currentGunIndex >= guns.Length)
        {
            Debug.LogError("Invalid current gun index!");
            return;
        }
        pa.PlayGunSMR();
        gunDictionary[currentGunIndex].Shoot(barrelTransform, bulletPrefabs, bulletParent, bulletHitMissDistance);
    }
    private void ShootType4(Transform barrelTransform)
    {
        if (currentGunIndex < 0 || currentGunIndex >= guns.Length)
        {
            Debug.LogError("Invalid current gun index!");
            return;
        }
        pa.PlayGunSniper();
        gunDictionary[currentGunIndex].Shoot(barrelTransform, bulletPrefabs, bulletParent, bulletHitMissDistance);
    }
    IEnumerator ShootContinuously()
    {
        while (true)
        {
            if (gunDictionary[currentGunIndex].bulletnum > 0 && Time.timeScale != 0 && isShooting)
            {
                Transform bulletOrigins;
                if (gunBulletOriginsMap.TryGetValue(guns[currentGunIndex], out bulletOrigins))
                {
                    Transform barrelTransform = bulletOrigins;

                    if (shootActions.ContainsKey(currentGunIndex))
                    {
                        pa.PlayGunshotSound();
                        shootActions[currentGunIndex](barrelTransform);
                    }
                    else
                    {
                        Debug.LogError("No shoot action defined for current gun index: " + currentGunIndex);
                    }
                }
                else
                {
                    Debug.LogError("Bullet origins for current gun not found!");
                }
            }
            yield return null;
        }
    }
    public float GetMoveSpeed()
    {
        return playerSpeed;
    }
    public void SetMoveSpeed(float newmspeed)
    {
        playerSpeed = newmspeed;

    }
    public Vector3 GetTranform()
    {
        return transform.position;
    }
    public void SetTranformPlayer(Vector3 tranformPlayer)
    {
        transformPlayer = tranformPlayer;
        trans = true;
    }
    public int GetBullet(int cur)
    {
        return gunDictionary[cur].bulletnum;
    }
    public void SetBulletGun(int cur, int bulletplus)
    {
        gunDictionary[cur].bulletnum += bulletplus;
    }
}
