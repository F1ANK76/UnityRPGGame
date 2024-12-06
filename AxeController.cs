using UnityEngine;
using System.Collections;

public class AxeController : CloseWeaponController
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;

    //private void Start()
    //{
    //    WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
    //    WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    //}

    // Update is called once per frame
    void Update()
    {
        if (isActivate) TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;

                // �浹����
                Debug.Log(hitInfo.transform.name);
            }

            // 1������ ���
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
