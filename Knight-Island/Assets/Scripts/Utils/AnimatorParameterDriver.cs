using UnityEngine;

namespace Utils
{
    using UnityEngine;

    public class AnimatorParameterDriver : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float lastX = animator.GetFloat("InputX");
            float lastY = animator.GetFloat("InputY");

            animator.SetFloat("InputX", lastX);
            animator.SetFloat("InputY", lastY);
        
            Debug.Log($"Attaque lancée vers X:{lastX} Y:{lastY}");
        }
    }
}
