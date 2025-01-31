using UnityEngine;

[System.Serializable] // 클래스 직렬화
public class Sound
{
    public string name; // 곡의 이름
    public AudioClip clip; // 곡
}

public class SoundManager : MonoBehaviour
{
    // 1개 유지를 위한 싱글턴 디자인 패턴화 필요

    static public SoundManager instance;

    void Awake() // 객체 생성시 최초 실행
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // 동시 재생을 위한 배열 생성
    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if(audioSourceEffects[j].isPlaying == false)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }

                Debug.Log("모든 가용 AudioSource가 사용중입니다");
                return;
            }
        }

        Debug.Log(_name + " 사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if(playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }

        Debug.Log("재생중인 " + _name + " 사운드가 없습니다.");
    }
}
