using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float secondPerRealTimeSecond; // 게임 세계의 100초 = 현실 세계의 1초

    private bool isNight = false;

    [SerializeField]
    private float fogDensityCalc; // 증감량 비율

    [SerializeField]
    private float nightFogDensity; // 밤 상태의 Fog 밀도

    private float dayFogDensity; // 낮 상태의 fog 밀도
    private float currentFogDensity; // 계산

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170) isNight = true;
        else if (transform.eulerAngles.x >= 340) isNight = false;

        if(isNight)
        {
            if(currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity <= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
