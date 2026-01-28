using UnityEngine;

// 게임 매니저라는 갓 클래스 (모든 데이터가 있다.)
public class GameManager : MonoBehaviour
{
    // 1. 게임 매니저 이름이 추상젓이다
    //  -> 책임이 많은 갓 클래스 냄새가 난다.(Bad Smell)
    // 2. 재화가 많아지면? 공격 방식이 많아지면?? 성장도 여기에 넣어야 하나?
    //  -> 코드량이 비대해지고 점점 복잡해 질 것이다.
    // 클릭(인게임) -> 재화 -> 성장 / 업그레이드 -> 로그인 -> DB -> 퀘스트

    public static GameManager Instance;

    public int ManualDamage = 10;
    public int AutoDamage = 3;
    public int Gold;
    public int ClickCount;
    
    private void Awake()
    {
        Instance = this;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
