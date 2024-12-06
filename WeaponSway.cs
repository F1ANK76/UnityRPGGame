using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    // 기존 위치
    private Vector3 originPos;

    // 현재 위치
    private Vector3 currentPos;

    // sway 한계
    [SerializeField]
    private Vector3 limitPos;

    // 정조준 sway 한계
    [SerializeField]
    private Vector3 fineSightLimitPos;

    // 부드러운 움직임 정도
    [SerializeField]
    private Vector3 smoothSway;

    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 현재 자기자신의 위치 대입
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.canPlayerMove) TrySway();
    }

    // 마우스 움직였을때 실행
    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) Swaying();
        else BackToOriginPos();
    }

    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        // 정조준 상태가 아닌 상태
        if(theGunController.isFineSightMode == false)
        {
            // 화면 밖으로 벗어나지 않게 Clamp 처리
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
            Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y),
            originPos.z);
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
            Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
            originPos.z);
        }

        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
