using System;
using UnityEngine;

// 게임 매니저라는 갓 클래스 (모든 데이터가 있다.)
public class GameManager : MonoBehaviour
{
    // 1. 게임 매니저라는 이름이 추상적이다.
    //     ㄴ 책임이 많은 갓 클래스 냄새가납니다. (Bad smell)
    // 2. 재화가 많아지면? 공격 방식이 많아지면?? 성장도 여기에 너아햐나??
    //  코드량이 비대해지고 점점 복잡해질것입니다.
    // 클릭(인게임쇼) -> 재화 -> 성장 /업그레이드 -> 로그인 -> DB -> 퀘스트
    
    
    
    public static GameManager Instance;

    // public static event Action OnDataChanged;
    
    // 클리커게임의 숫자 단위는 억,조,경,해 ~~~~ 넘어가요.
    //                영어로  K, M, G, A, B, AA, TT ~~~
    // public double ManualDamage = 1000000000d;          // 21억까지
       
    // int(21억), long(경), biginteger(숫자를 쪼개 문자로관리하면서 연산하니까 매우 느립니다.)
    // float , double , decimal (부동소수점) 같은 자료형 크기 대비 범위가 엄청나게큽니다.
    
    // float   10^38     (여러분들 컵정도의 크기) 정밀도: 6자리
    // double  10^900    (지구정도의 크기)       정밀도가 15자리인가되요.
    // decimal 10^283904 (은하계정도의 크기)      

    // public double AutoDamage = 1000000000d;
    
    private void Awake()
    {
        Instance = this;
    }
}