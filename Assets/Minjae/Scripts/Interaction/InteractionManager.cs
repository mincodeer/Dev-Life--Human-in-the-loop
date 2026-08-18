using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    private void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            //If you clicked UI, the world object will not interact - Minjae
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Vector2 mouseScreenPosition =
            Mouse.current.position.ReadValue();

        Vector2 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        RaycastHit2D hit =
            Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

        if (hit.collider == null)
        {
            return;
        }

        IInteractable interactable =
            hit.collider.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact();
        }
    }
}