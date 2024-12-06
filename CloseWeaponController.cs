using UnityEngine;
using System.Collections;

// �̿ϼ� Ŭ���� = �߻� Ŭ����
public abstract class CloseWeaponController : MonoBehaviour
{
    // ���� ������ Hand�� Ÿ�� ����
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // ������
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
                    // �ڷ�ƾ ����
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

        // ���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }

    // �̿ϼ����� ����ڴ� = �߻� �ڷ�ƾ
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        // �浹 ���� ��ȯ
        return Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask);
    }

    // ���� �Լ� = �ϼ� �Լ�������, �߰� ������ �Լ�
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            // ���� �� ǥ�� ����
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        // �ٲٴ� ���Ⱑ ���� ���Ⱑ ��
        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
        //isActivate = true;
    }
}
