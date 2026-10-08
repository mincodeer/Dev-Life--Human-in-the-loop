using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectSetupDropdown : TMP_Dropdown
{
    [Serializable]
    public class OptionAccess
    {
        [Min(1)] public int optionIndex = 1;
        public bool unlocked = true;
        [TextArea] public string requirement;
    }

    // Empty by default: no existing choice is locked without a defined rule.
    [SerializeField] private List<OptionAccess> optionAccess = new List<OptionAccess>();
    public Action<string> LockedOptionClicked;
    private int nextItemIndex;

    public bool IsOptionLocked(int index)
    {
        OptionAccess access = optionAccess.Find(entry => entry.optionIndex == index);
        return index > 0 && access != null && !access.unlocked;
    }

    // Integration point for the real progression system once its rules are defined.
    public void SetOptionUnlocked(int index, bool unlocked)
    {
        OptionAccess access = optionAccess.Find(entry => entry.optionIndex == index);
        if (access == null) return;
        Hide();
        access.unlocked = unlocked;
    }

    protected override GameObject CreateDropdownList(GameObject template)
    {
        nextItemIndex = 0;
        return base.CreateDropdownList(template);
    }

    protected override DropdownItem CreateItem(DropdownItem itemTemplate)
    {
        DropdownItem item = base.CreateItem(itemTemplate);
        int index = nextItemIndex++;
        LockedOptionToggle toggle = item.toggle as LockedOptionToggle;
        if (toggle == null)
        {
            Debug.LogError("Project Setup dropdown Item needs LockedOptionToggle.", this);
            return item;
        }
        toggle.IsLocked = () => IsOptionLocked(index);
        toggle.ShowRequirement = () =>
        {
            OptionAccess access = optionAccess.Find(entry => entry.optionIndex == index);
            string message = access == null || string.IsNullOrWhiteSpace(access.requirement)
                ? "Unlock condition has not been defined yet."
                : access.requirement;
            Hide();
            LockedOptionClicked?.Invoke(options[index].text + "\n\n" + message);
        };
        if (IsOptionLocked(index))
        {
            item.text.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            item.text.rectTransform.offsetMax -= new Vector2(22f, 0f);
            AddLockIcon(item.rectTransform);
        }
        return item;
    }

    // Small monochrome lock made from UI Images; no emoji/font dependency.
    private static void AddLockIcon(RectTransform parent)
    {
        var icon = new GameObject("Lock Icon", typeof(RectTransform));
        RectTransform root = (RectTransform)icon.transform;
        root.SetParent(parent, false);
        root.anchorMin = root.anchorMax = new Vector2(1f, 0.5f);
        root.anchoredPosition = new Vector2(-14f, 0f);
        root.sizeDelta = new Vector2(16f, 18f);
        AddLockPart(root, new Vector2(0f, -3f), new Vector2(12f, 9f));
        AddLockPart(root, new Vector2(-4f, 4f), new Vector2(2f, 7f));
        AddLockPart(root, new Vector2(4f, 4f), new Vector2(2f, 7f));
        AddLockPart(root, new Vector2(0f, 7f), new Vector2(10f, 2f));
    }

    private static void AddLockPart(RectTransform parent, Vector2 position, Vector2 size)
    {
        var part = new GameObject("Lock", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        RectTransform rect = (RectTransform)part.transform;
        rect.SetParent(parent, false);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        Image image = part.GetComponent<Image>();
        image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        image.raycastTarget = false;
    }
}
