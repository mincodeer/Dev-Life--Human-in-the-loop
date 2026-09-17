using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Swaps a UI image to match the genre currently displayed by the Review UI.
/// This keeps the existing review scripts unchanged.
/// </summary>
public class GenreIconDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image targetImage;
    [SerializeField] private TMP_Text genreText;

    [Header("Genre Icons")]
    [SerializeField] private Sprite rpgIcon;
    [SerializeField] private Sprite actionIcon;
    [SerializeField] private Sprite simulationIcon;
    [SerializeField] private Sprite fallbackIcon;

    private string lastGenre;

    private void Reset()
    {
        targetImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        lastGenre = null;
        RefreshFromText();
    }

    private void LateUpdate()
    {
        RefreshFromText();
    }

    private void RefreshFromText()
    {
        if (genreText == null)
        {
            return;
        }

        string currentGenre = genreText.text.Trim();

        if (currentGenre == lastGenre)
        {
            return;
        }

        lastGenre = currentGenre;
        SetGenre(currentGenre);
    }

    public void SetGenre(string genre)
    {
        if (targetImage == null)
        {
            return;
        }

        Sprite selectedIcon;

        switch ((genre ?? string.Empty).Trim().ToUpperInvariant())
        {
            case "RPG":
                selectedIcon = rpgIcon;
                break;

            case "ACTION":
                selectedIcon = actionIcon;
                break;

            case "SIMULATION":
                selectedIcon = simulationIcon;
                break;

            default:
                selectedIcon = fallbackIcon;
                break;
        }

        targetImage.sprite = selectedIcon;
        targetImage.preserveAspect = true;
        targetImage.enabled = selectedIcon != null;
    }
}

