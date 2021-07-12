
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimationActive_Trigger : ITrigger
{
    //TODO:动画的需求还得考虑
    //public bool flag = true;
    public string animationName;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Action()
    {
        animator.Play(animationName);
    }



}