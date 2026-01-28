using UnityEngine;

// 게임 매니저라는 갓 클래스 (모든 데이터가 있다.)
public class GameManager : MonoBehaviour
{
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
