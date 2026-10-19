using UnityEngine;
using UnityEngine.InputSystem;

public class RubbishClean : MonoBehaviour
{
    [SerializeField] private AudioClip cleaningSound;

    private Camera mainCamera;
    private AudioSource sfxSource;

    void Start()
    {
        mainCamera = Camera.main;

        GameObject sourceObject = GameObject.Find("SFXSource");

        if (sourceObject != null)
        {
            sfxSource = sourceObject.GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Mouse.current == null || mainCamera == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
                new Vector3(mousePosition.x, mousePosition.y, 0f)
            );

            Collider2D hit = GetComponent<Collider2D>();

            if (hit != null && hit.OverlapPoint(worldPosition))
            {
                ProjectResourceController controller =
                    FindFirstObjectByType<ProjectResourceController>();

                if (controller != null)
                {
                    controller.CurrentProjectConditions.ChangeQuality(2f);
                }

                if (sfxSource != null && cleaningSound != null)
                {
                    sfxSource.PlayOneShot(cleaningSound);
                }

                Destroy(gameObject);
            }
        }
    }
}