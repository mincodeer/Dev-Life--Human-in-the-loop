using UnityEngine;
using UnityEngine.InputSystem;

public class RubbishCleaner : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Clicked: " + hit.collider.gameObject.name);

                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Rubbish cleaned!");
                    Destroy(gameObject);
                    ProjectResourceController projectController = FindFirstObjectByType<ProjectResourceController>();
                    if (projectController != null)
                    {
                        projectController.CurrentProjectConditions.ChangeQuality(2f);
                    }
                }
            }
        }
    }
}