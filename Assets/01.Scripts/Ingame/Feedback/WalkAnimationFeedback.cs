using System.Collections;
using UnityEngine;

public class WalkAnimationFeedback : MonoBehaviour, IFeedback
{
    private static readonly int AnimatorWalk = Animator.StringToHash("Walk");

    private Animator _animator;
    private Coroutine _walkCoroutine;
    private bool _isPlaying;

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

        // 이미 애니메이션이 재생 중이면 무시 (끊기지 않도록)
        if (_isPlaying)
        {
            return;
        }

        _walkCoroutine = StartCoroutine(PlayWalkAnimation());
    }

    private IEnumerator PlayWalkAnimation()
    {
        _isPlaying = true;
        _animator.SetBool(AnimatorWalk, true);

        // 다음 프레임까지 대기 (애니메이션 상태가 전환될 때까지)
        yield return null;
        yield return null;

        // Walk 애니메이션이 끝날 때까지 대기
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(animationLength);

        _animator.SetBool(AnimatorWalk, false);
        _isPlaying = false;
        _walkCoroutine = null;
    }
}
