// Student ID: 23208000

using UnityEngine;

/// <summary>
/// Stores the visual and text information for one ending.
/// This allows each ending to be edited from the Unity Inspector.
/// </summary>
[CreateAssetMenu(
    fileName = "New Ending Data",
    menuName = "Dev Life/Ending Data")]
public class EndingData : ScriptableObject
{
    [Header("Ending Information")]

    // The title displayed at the top of the ending screen.
    public string endingTitle;

    // Colour used for the ending title.
    public Color titleColor = Color.red;

    // The description explaining why the game ended.
    [TextArea(3, 6)]
    public string endingDescription;

    // Extra information about the player's final outcome.
    [TextArea(2, 4)]
    public string endingStats;

    [Header("Ending Visuals")]

    // Image displayed on the ending screen.
    public Sprite endingImage;

    // Background image used for this ending.
    public Sprite endingBackground;
}