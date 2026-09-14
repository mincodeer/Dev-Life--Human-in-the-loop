using UnityEngine;
using UnityEngine.SceneManagement;

public class ReviewNavigation : MonoBehaviour
{
    public void ReturnToDashboard()
    {
        SceneManager.LoadScene("Dashboard");
    }
}