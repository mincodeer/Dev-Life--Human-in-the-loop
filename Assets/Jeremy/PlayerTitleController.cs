using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTitleController : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text progressText;
    public TMP_Text nextTitleText;
    public TMP_Text skillValueText;
    public Slider titleProgressBar;
    public int developmentSkill = 0;

    void Update()
    {
        skillValueText.text = developmentSkill + " / 100";

        if (developmentSkill <= 20)
        {
            titleText.text = "Beginner Developer";
            titleProgressBar.value = developmentSkill;
            progressText.text = developmentSkill + " / 20";
            nextTitleText.text = "Next: Junior Developer";
        }
        else if (developmentSkill <= 40)
        {
            titleText.text = "Junior Developer";
            titleProgressBar.value = developmentSkill - 20;
            progressText.text = (developmentSkill - 20) + " / 20";
            nextTitleText.text = "Next: Developer";
        }
        else if (developmentSkill <= 60)
        {
            titleText.text = "Developer";
            titleProgressBar.value = developmentSkill - 40;
            progressText.text = (developmentSkill - 40) + " / 20";
            nextTitleText.text = "Next: Senior Developer";
        }
        else if (developmentSkill <= 80)
        {
            titleText.text = "Senior Developer";
            titleProgressBar.value = developmentSkill - 60;
            progressText.text = (developmentSkill - 60) + " / 20";
            nextTitleText.text = "Next: Expert Developer";
        }
        else
        {
            titleText.text = "Expert Developer";
            titleProgressBar.value = 20;
            progressText.text = "MAX";
            nextTitleText.text = "Maximum Title Reached";
        }
    }
}