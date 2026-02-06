using System.Collections.Generic;
using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    private void Start()
    {
        // Start()는 모든 Awake() 이후에 실행되므로 안전!
        _timer = 0f;
    }

    // 역할: 정해진 시간 간격마다 Clickable한 친구를 때린다.
    [SerializeField] private float _interval;       // 시간 간격
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval)  // 1. 시간 간격마다.
        {
            _timer = 0f;
            
            
            // 2. Clickable 게임 오브젝트를 모두 찾아와서 (여러분들은 캐싱하세요.)
            GameObject[] clickables = GameObject.FindGameObjectsWithTag("Clickable");
            // GameObject[] clickables = ClickTargetManager.Instance.GetActiveTargets();
            foreach (GameObject clickable in clickables)
            {
                // 3. 클릭한다.
                Clickable clickableScript = clickable.GetComponent<Clickable>();

                double damage = GetAutoClickDamage();

                ClickInfo clickInfo = new ClickInfo
                {
                    Type = EClickType.Auto,
                    Damage = damage,
                };
                
                clickableScript.OnClick(clickInfo);
            }
            
        }
    }
    private double GetAutoClickDamage()
    {
        double flat = UpgradeManager.Instance.Get(EUpgradeType.AutoClickDamagePlusAdd).Damage;
        double percent = UpgradeManager.Instance.Get(EUpgradeType.AutoClickDamagePercentAdd).Damage;
        double percent2 = UpgradeManager.Instance.Get(EUpgradeType.AutoClick2DamagePercentAdd).Damage;


        double baseDamage = 1; // 기본 오토 대미지
        return (baseDamage + flat) * (1 + percent / 100.0) * (1 + percent2 / 100.0);
    }

}
