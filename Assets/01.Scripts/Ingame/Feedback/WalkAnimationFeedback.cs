using System.Collections;
using UnityEngine;

public class WalkAnimationFeedback : MonoBehaviour, IFeedback
{
    private static readonly int AnimatorWalk = Animator.StringToHash("Walk");

    [SerializeField] private float _walkDuration = 0.5f;

    private Animator _animator;
    private Coroutine _walkCoroutine;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Play(ClickInfo clickInfo)
    {
        if (_animator == null) return;

        if (clickInfo.Type == EClickType.Auto)
        {
            return;
        }

        // 이미 실행 중인 코루틴이 있으면 중지
        if (_walkCoroutine != null)
        {
            StopCoroutine(_walkCoroutine);
        }

        _walkCoroutine = StartCoroutine(PlayWalkAnimation());
    }

    private IEnumerator PlayWalkAnimation()
    {
        _animator.SetBool(AnimatorWalk, true);
        yield return new WaitForSeconds(_walkDuration);
        _animator.SetBool(AnimatorWalk, false);
        _walkCoroutine = null;
    }
}
