using UnityEngine;
using System.Collections;

// 미완성 클래스 = 추상 클래스
public abstract class CloseWeaponController : MonoBehaviour
{
    // 현재 장착된 Hand형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격중
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    [SerializeField]
    protected LayerMask layerMask;

    protected void TryAttack()
    {
        if(Inventory.inventoryActivated == false)
        {
            if (Input.GetButton("Fire1"))
            {
                if (isAttack == false)
                {
                    // 코루틴 실행
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }

    // 미완성으로 남기겠다 = 추상 코루틴
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        // 충돌 여부 반환
        return Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask);
    }

    // 가상 함수 = 완성 함수이지만, 추가 편집한 함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            // 기존 총 표기 삭제
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        // 바꾸는 무기가 현재 무기가 됨
        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
        //isActivate = true;
    }
}
