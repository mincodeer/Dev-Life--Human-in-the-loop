using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// A disabled Toggle cannot receive clicks. Keep navigation/clicks enabled,
// but stop locked choices before TMP_Dropdown changes its value or fires events.
[AddComponentMenu("UI/Project Setup/Locked Option Toggle")]
public class LockedOptionToggle : Toggle
{
    public Func<bool> IsLocked;
    public Action ShowRequirement;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (TryShowRequirement()) return;
        base.OnPointerClick(eventData);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        if (TryShowRequirement()) return;
        base.OnSubmit(eventData);
    }

    private bool TryShowRequirement()
    {
        if (!IsActive() || !IsInteractable()) return false;
        if (IsLocked == null || !IsLocked()) return false;
        ShowRequirement?.Invoke();
        return true;
    }
}
