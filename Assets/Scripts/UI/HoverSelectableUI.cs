using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HoverSelectableUI : XRBaseInteractable
{
    public Material highlightMaterial; // Hover �� ����� ��Ƽ����
    private Material originalMaterial; // ���� ��Ƽ���� �����
    private Renderer objectRenderer;   // ��Ƽ���� �ٲ��� ������

    public GameObject popupUI; // Ŭ�� �� Ȱ��ȭ/��Ȱ��ȭ �� UI

    private bool isUIActive = false; // ���� UI�� ���� �ִ��� ���� Ȯ��

    protected override void Awake()
    {
        base.Awake();

        // ������Ʈ�� �ִ� Renderer �������� (��Ƽ���� �����)
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            // ���� �� ���� ��Ƽ���� ����
            originalMaterial = objectRenderer.material;
        }

        // ���� �� UI ���� �ְ� ����
        if (popupUI != null)
        {
            popupUI.SetActive(false);
        }
    }

    // Hover ����: ��Ƽ���� �� �ٲٱ�
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if (objectRenderer != null && highlightMaterial != null)
        {
            objectRenderer.material = highlightMaterial;
        }
    }

    // Hover ��: ��Ƽ���� ������� ����
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        if (objectRenderer != null && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }

    // ��ư ���� (��: Ʈ���� Ŭ�� ��) �� UI ���
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // UI�� ����Ǿ� ���� ���� ����
        if (popupUI != null)
        {
            isUIActive = !isUIActive; // ���� ���� ����
            popupUI.SetActive(isUIActive); // UI �ѱ�/����
        }
    }
}
