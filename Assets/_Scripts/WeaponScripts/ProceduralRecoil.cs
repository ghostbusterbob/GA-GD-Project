using System.Collections;
using UnityEngine;

public class Recoi : MonoBehaviour
{
    Vector3 currentRotation, targetRotation;
    Vector3 currentPosition, targetPosition;

    public Transform cam;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject ADSTarget;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Shooting shooting;

    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float kickBackZ;

    public float snappiness = 6f;
    public float returnAmount = 2f;

    private Vector3 recoilPositionOffset;
    private Quaternion recoilRotationOffset;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    [SerializeField] public bool isAutomatic = false;
    [SerializeField] private float fireRate = 0.1f;

    private bool isShooting = false;

    private float originalRecoilX;
    private float originalRecoilY;
    private float originalRecoilZ;
    private float originalKickBackZ;
        private Vector3 impact = Vector3.zero;
            [SerializeField] private CharacterController controller;



    private bool appliedRecoil = false;

    [SerializeField] private float fireCooldown = 0.5f; // cooldown for semi-auto
    private float lastFireTime = -Mathf.Infinity;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        originalRecoilX = recoilX;
        originalRecoilY = recoilY;
        originalRecoilZ = recoilZ;
        originalKickBackZ = kickBackZ;
    }

    void Update()
    {
        if (impact.magnitude > 0.2f)
        {
            controller.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, 5f * Time.deltaTime);
        }
        // Handle recoil smoothing
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snappiness);
        recoilRotationOffset = Quaternion.Euler(currentRotation);

        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * snappiness);
        recoilPositionOffset = currentPosition;

        // Firing
        if (isAutomatic)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !isShooting)
            {
                StartCoroutine(AutomaticFire());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= lastFireTime + fireCooldown)
            {
                if (shooting.bulletCount > 0)
                {
                    shooting.weapon();
                    ApplyRecoil();
                    Vector3 forceDirection = -transform.forward;
            impact += forceDirection * 40f;
                    StartCoroutine(DisableMuzzleFlash());
                    lastFireTime = Time.time;
                }
            }
        }

        // ADS (Aim Down Sights)
        if (Input.GetKey(KeyCode.Mouse1))
        {
            gun.transform.position = Vector3.Lerp(gun.transform.position, ADSTarget.transform.position, Time.deltaTime * 10f);

            recoilX = originalRecoilX / 2;
            recoilY = originalRecoilY / 2;
            recoilZ = originalRecoilZ / 2;
            kickBackZ = originalKickBackZ / 2;
        }
        else
        {
            gun.transform.position = Vector3.Lerp(gun.transform.position, transform.position, Time.deltaTime * 10f);
            recoilX = originalRecoilX;
            recoilY = originalRecoilY;
            recoilZ = originalRecoilZ;
            kickBackZ = originalKickBackZ;
        }
    }

    IEnumerator AutomaticFire()
    {
        isShooting = true;
        while (Input.GetKey(KeyCode.Mouse0) && shooting.bulletCount > 0)
        {
            shooting.weapon();
            ApplyRecoil();
            StartCoroutine(DisableMuzzleFlash());
            yield return new WaitForSeconds(fireRate);
        }
        isShooting = false;
    }

    public void ApplyRecoil()
    {
        if (!appliedRecoil)
        {
            muzzleFlash.SetActive(true);
            
            appliedRecoil = true;
        }

        targetPosition -= new Vector3(0, 0, kickBackZ);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    public Vector3 GetRecoilOffset() => recoilPositionOffset;
    public Quaternion GetRecoilRotation() => recoilRotationOffset;

    public IEnumerator DisableMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
        appliedRecoil = false;
    }
}
