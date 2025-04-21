using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HoverSelectableUI : XRBaseInteractable
{
    private Color originalColor;
    public Renderer objectRenderer;

    public GameObject popupUI; // Quad로 만든 UI 오브젝트

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        if (popupUI != null)
        {
            popupUI.SetActive(false); // 시작 시 UI 꺼두기
        }
    }

    // 호버 시작 시 UI 켜기
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (objectRenderer != null)
        {
            objectRenderer.material.color = new Color(0.3f, 0.7f, 1.0f); // 하늘색
        }

        if (popupUI != null)
        {
            popupUI.SetActive(true); // UI 켜기
        }
    }

    // 호버 끝나면 UI 끄기
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }

        if (popupUI != null)
        {
            popupUI.SetActive(false); // UI 끄기
        }
    }
}
