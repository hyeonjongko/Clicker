using UnityEngine;

public class ClickTarget : MonoBehaviour, Clickable
{
   [SerializeField] private string _name;


   public bool OnClick(ClickInfo clickInfo)
   {
      Debug.Log($"{_name}: 다음부터는 늦지 않겠습니다.");
      
      // 클릭에대한 여러 가지 피드백을 보여줘야 합니다.
      
      
      // S : 한 클래스는 하나의 역할/책임만 가지자
      // ClickTarget: 타겟에대한 중앙 관리자이자.. 소통의 창구(객체지향 상호작용) , 피드백을 실행시켜주는 역할도 

      var feedbacks = GetComponentsInChildren<IFeedback>();
      foreach (var feedback in feedbacks)
      {
         feedback.Play(clickInfo);
      }
      
      // 'CBD' < ECS: 
      // 1. 클릭 이펙트
      // 2. 캐릭터 애니메이션(있으면)
      // 3. 스케일 트위닝
      // 4. 플래시 
      // 4. 대미지 플로팅
      // 5. 사운드 재생
      // 6. 화면 흔들림
      // 7. 재화 떨구기

      return true;
   }


   private void EffectFeedback()
   {
      
   }

   private void AnimationFeedback()
   {
      
   }

}
