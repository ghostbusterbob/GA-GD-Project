using System.Collections;
using System.Collections.Generic;
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

    public float snappiness, returnAmount;

    private Vector3 recoilPositionOffset;
    private Quaternion recoilRotationOffset;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    [SerializeField] bool isAutomatic = false;  
    [SerializeField] float fireRate = 0.1f;  
    private bool isShooting = false;

    private float originalRecoilX;
    private float originalRecoilY;
    private float originalRecoilZ;
    private float originalKickBackZ;

    private bool appliedRecoil = false;

    

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
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snappiness);
        recoilRotationOffset = Quaternion.Euler(currentRotation);

        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * snappiness);
        recoilPositionOffset = currentPosition;

        if (isAutomatic)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !isShooting)
            {
                StartCoroutine(AutomaticFire());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ApplyRecoil();
            }
        }

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
        while (Input.GetKey(KeyCode.Mouse0) && shooting.bulletCount >= 0)
        {
            shooting.weapon();

            ApplyRecoil();
            shooting.weapon();
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(fireRate);
            muzzleFlash.SetActive(false);

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

    public Vector3 GetRecoilOffset()
    {
        return recoilPositionOffset;
    }

    public Quaternion GetRecoilRotation()
    {
        return recoilRotationOffset;
    }
}