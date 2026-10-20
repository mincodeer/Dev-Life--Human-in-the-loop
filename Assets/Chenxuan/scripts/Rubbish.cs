using UnityEngine;

public class Rubbish : MonoBehaviour
{
    public void Clean()
    {
        Destroy(gameObject);
        Debug.Log("Rubbish cleaned!");
    }
    private void OnMouseDown()
    {
        Clean();
    }
}
