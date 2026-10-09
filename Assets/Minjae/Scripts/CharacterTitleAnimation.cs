using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterTitleAnimation : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController oldCharacterController;
    [SerializeField] private RuntimeAnimatorController newCharacterController;

    // Developer starts above 40, matching PlayerTitleController.
    private const int DeveloperSkillThreshold = 41;
    private Animator characterAnimator;

    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        RefreshAnimation();
    }

    private void Update()
    {
        RefreshAnimation();
    }

    private void RefreshAnimation()
    {
        if (ResourceManager.Instance == null)
            return;

        RuntimeAnimatorController controller =
            ResourceManager.Instance.DevelopmentSkill >= DeveloperSkillThreshold
                ? newCharacterController
                : oldCharacterController;

        // Only change controllers on a title transition, so the clip keeps playing.
        if (controller != null && characterAnimator.runtimeAnimatorController != controller)
        {
            characterAnimator.runtimeAnimatorController = controller;
        }
    }
}
