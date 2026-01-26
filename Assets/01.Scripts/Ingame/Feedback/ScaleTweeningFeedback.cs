using DG.Tweening;
using UnityEngine;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private ClickTarget _owner;


    private void Awake()
    {
        _owner = GetComponent<ClickTarget>();
    }
    
    // 역할: 스케일 트위닝 피드백에 대한 로직을 담당
    public void Play(ClickInfo clickInfo)
    {
        // 0.6초동안 1.2배 확대되었다가 다시 원래 크기로 돌아오는 간단한 코드
        _owner.transform.DOKill(true);
        _owner.transform.DOScale(1.1f, 0.3f).OnComplete(() =>
        {
            _owner.transform.localScale = Vector3.one;
        });
    }
    
}
