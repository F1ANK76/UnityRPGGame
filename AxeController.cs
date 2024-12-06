using UnityEngine;
using System.Collections;

public class AxeController : CloseWeaponController
{
    // 활성화 여부
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

                // 충돌했음
                Debug.Log(hitInfo.transform.name);
            }

            // 1프레임 대기
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
