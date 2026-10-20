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
        public ProjectUnlockProgression.Rule unlockRule;
        [TextArea] public string requirement;
    }

    // Empty by default: no existing choice is locked without a defined rule.
    [SerializeField] private List<OptionAccess> optionAccess = new List<OptionAccess>();
    public Action<string> LockedOptionClicked;
    private int nextItemIndex;
    private int hiddenOptionIndex = -1;

    public void SetHiddenOption(int index, string label, bool visible)
    {
        hiddenOptionIndex = index;
        if (visible && options.Count == index)
        {
            Hide();
            options.Add(new OptionData(label));
            RefreshShownValue();
        }
        else if (!visible && options.Count > index && options[index].text == label)
        {
            Hide();
            options.RemoveAt(index);
            if (value >= options.Count) SetValueWithoutNotify(0);
            RefreshShownValue();
        }
    }

    public bool IsOptionLocked(int index)
    {
        if (index < 0 || index >= options.Count) return true;
        if (index == hiddenOptionIndex)
        {
            ProjectUnlockProgression hiddenProgression = ResourceManager.Instance == null
                ? null : ResourceManager.Instance.GetComponent<ProjectUnlockProgression>();
            return hiddenProgression == null ||
                !hiddenProgression.IsUnlocked(ProjectUnlockProgression.Rule.HiddenContent);
        }
        OptionAccess access = optionAccess.Find(entry => entry.optionIndex == index);
        if (index <= 0 || access == null) return false;
        if (access.unlockRule == ProjectUnlockProgression.Rule.Manual) return !access.unlocked;
        ProjectUnlockProgression progression = ResourceManager.Instance == null
            ? null : ResourceManager.Instance.GetComponent<ProjectUnlockProgression>();
        return progression == null || !progression.IsUnlocked(access.unlockRule);
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
        toggle.RefreshLockVisual();
        return item;
    }

}
