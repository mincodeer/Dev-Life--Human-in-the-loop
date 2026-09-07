using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTitleController : MonoBehaviour
{
    public TMP_Text titleText;
    public Slider titleProgressBar;
    public int developmentSkill = 0;

    void Update()
    {
        if (developmentSkill <= 20)
        {
            titleText.text = "Beginner Developer";
            titleProgressBar.value = developmentSkill;
        }
        else if (developmentSkill <= 40)
        {
            titleText.text = "Junior Developer";
            titleProgressBar.value = developmentSkill - 20;
        }
        else if (developmentSkill <= 60)
        {
            titleText.text = "Competent Developer";
            titleProgressBar.value = developmentSkill - 40;
        }
        else if (developmentSkill <= 80)
        {
            titleText.text = "Senior Developer";
            titleProgressBar.value = developmentSkill - 60;
        }
        else
        {
            titleText.text = "Expert Developer";
            titleProgressBar.value = 20;
        }
    }
}