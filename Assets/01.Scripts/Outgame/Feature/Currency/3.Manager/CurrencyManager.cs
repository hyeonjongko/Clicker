using System;
using UnityEngine;

// 오직 데이터(재화)를 "관리"하는 클래스입니다.
// 클린 아키텍처에서는 "서비스"라는 이름을 쓴다. (그러나 게임에서는 보통 "매니저"라고 표현한다.)
public class CurrencyManager : MonoBehaviour
{
   
   public static CurrencyManager Instance;
   // CRUD
   //  관리란: "데이터에 대한 생성 / 조회 / 사용 / 소모 / 이벤트 등" 로직
   //         ㄴ 비즈니스 로직(게임 로직): 데이터 사용에 대한 핵심 규칙

   // 이벤트
   public static event Action OnDataChanged;

   
   // 재화 데이터들 (배열로 관리)
   private Currency[] _currencies = new Currency[(int)ECurrencyType.Count];
   
   // 저장소
   // 의존이란 한 객체가 동작하기 위해서 다른 객체를 참조하는것을
   // DIP: 구현체에 의존하지 말고 약속에 의존해라 
   private ICurrencyRepository _repository;
   
   private void Awake()
   {
      Instance = this;

        //_repository = new LocalCurrencyRepository();
        _repository = new FirebaseCurrencyRepository();
      
      
      Currency currency1 = new Currency(10000);
      Currency currency2 = new Currency(30000);
      Currency currency3 = currency1 + currency2;
    
      Debug.Log(currency3);  // 40k
   }

   private async void Start()
   {
        CurrencySaveData saveData = await _repository.Load();
      double[] currencyValues = saveData.Currencies;
      for (int i = 0; i < _currencies.Length; i++)
      {
         _currencies[i] = currencyValues[i];
      }
      
   }
   
   
   // 0. 재화 조회
   public Currency Get(ECurrencyType currencyType)
   {
      return _currencies[(int)currencyType];
   }
   
   // - 어쩔수 없는 재화 조회 편의 기능... ㅠㅠ
   
   public Currency Gold => Get(ECurrencyType.Gold);
   public Currency Ruby => Get(ECurrencyType.Ruby);
   //public Currency Jelly => Get(ECurrencyType.Jelly);
   
   
   // 1. 재화 추가
   public void Add(ECurrencyType type, Currency amount)
   {
      _currencies[(int)type] += amount;

      Save();
      
      OnDataChanged?.Invoke();
   }
   
   // 2. 재화 소모
   public bool TrySpend(ECurrencyType type, Currency amount)
   {
      if (_currencies[(int)type] >= amount)
      {
         _currencies[(int)type] -= amount;

         Save();
         
         OnDataChanged?.Invoke();

         return true;
      }

      return false;
   }
   
   // 3. 돈 있으세요? 
   public bool CanAfford(ECurrencyType type, Currency amount)
   {
      return _currencies[(int)type] >= amount;
   }
   
   private void Save()
   {
      var saveData = new CurrencySaveData();
      saveData.Currencies = new double[_currencies.Length];
      for (int i = 0; i < _currencies.Length; i++)
      {
         saveData.Currencies[i] = (double)_currencies[i];
      }
      _repository.Save(saveData).Forget();
   }

  
}
