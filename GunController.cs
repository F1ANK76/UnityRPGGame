using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;

    // ���� ������ ��
    [SerializeField]
    private Gun currentGun;

    // ���� �ӵ� ���
    private float currentFireRate;

    // ���� ����
    private bool isReload = false;

    [HideInInspector]
    public bool isFineSightMode = false;

    // ���� ������ ��
    [SerializeField]
    private Vector3 originPos;

    // ȿ���� ���
    private AudioSource audioSource;

    // ������ �浹 ���� �޾ƿ�
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    // �ǰ� ����Ʈ
    [SerializeField]
    private GameObject hit_effect_prefab;

    private void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindObjectOfType<Crosshair>();

        //WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        //WeaponManager.currentWeaponAnim = currentGun.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFindSight();
        }
    }

    // ����ӵ� ����
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0) currentFireRate -= Time.deltaTime; // 60���� 1�� 60�� ����
    }

    // �߻� �õ�
    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && isReload == false)
        {
            Fire();
        }
    }

    // �߻� �� ���
    private void Fire()
    {
        if(isReload == false)
        {
            if (currentGun.currentBulletCount > 0) Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    // ������ �õ�
    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && isReload == false && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void CancleReload()
    {
        if(isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    // �߻� �� ���
    private void Shoot()
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // ���� �ӵ� ����
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();

        Hit();

        // �ѱ� �ݵ� �ڷ�ƾ ����
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    private void Hit()
    {
        // ī�޶� ���� ��ġ���� �������� ���µ� �ּҺ��� �ִ��� �������� ������
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
            Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy), 0)
            , out hitInfo, currentGun.range, layerMask))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    // ������
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("������ �Ѿ��� �����ϴ�.");
        }
    }

    // ������ �õ�
    private void TryFindSight()
    {
        if (Input.GetButtonDown("Fire2") && isReload == false)
        {
            FindSight();
        }
    }

    // ������ ���
    public void CancelFineSight()
    {
        if (isFineSightMode) FindSight();
    }

    // ������ ��ȯ
    private void FindSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineShigtActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineShigtDeactivateCoroutine());
        }
    }

    // ������ ����
    IEnumerator FineShigtActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    // ������ ����
    IEnumerator FineShigtDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    // �ѱ� �ݵ� ����
    IEnumerator RetroActionCoroutine()
    {
        // ������ �� �������� �ִ� �ݵ�
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);

        // ������ �������� �ִ� �ݵ�
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        // ������ ���°� �ƴ� ���
        if(isFineSightMode == false)
        {
            currentGun.transform.localPosition = originPos;

            // �ݵ� ����, �����ϴٰ� ���� ��ġ�����ٸ� ���� ����
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // ����ġ
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        // ������ ����
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // �ݵ� ����, �����ϴٰ� ���� ��ġ�����ٸ� ���� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // ����ġ
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    // ���� ���
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            // ���� �� ǥ�� ����
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        // �ٲٴ� ���Ⱑ ���� ���Ⱑ ��
        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
