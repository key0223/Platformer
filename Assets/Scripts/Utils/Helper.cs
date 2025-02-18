using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    #region Cashing
    public static WaitForSeconds GetWait(float time)
    {
        return new WaitForSeconds(time);
    }
    #endregion
    #region Animation
    public static AnimationClip GetAnimationClip(Animator animator, string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }
    public static float GetRemainingAnimationTime(Animator animator, int layerIndex = 0)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

        float clipLength = stateInfo.length;
        float normalizedTime = stateInfo.normalizedTime % 1f;

        // 남은 시간 계산
        float remainingTime = clipLength * (1f - normalizedTime);

        return remainingTime;
    }

    public static float GetAnimationClipLenth(Animator animator, string name)
    {
        AnimationClip clip = GetAnimationClip(animator, name);

        if (clip != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);

            foreach (AnimatorClipInfo clipInfo in clipInfos)
            {
                if (clipInfo.clip == clip)
                {
                    float clipLength = clip.length;
                    float clipSpeed = stateInfo.speed * animator.speed;

                    float actualLength = clipLength / clipSpeed;

                    return actualLength;
                }
            }
        }
        return 1;
    }
    #endregion
}
