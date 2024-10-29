using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtension
{
    public static float GetAnimationFrameCount(this Animator animator, string clipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach (var clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length * clip.frameRate;
            }
        }

        Debug.LogWarning($"Animation clip '{clipName}' not found.");
        return 0;
    }
}