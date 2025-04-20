using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HoverSelectableUI : XRBaseInteractable
{
    public Material highlightMaterial; // Hover 시 사용할 머티리얼
    private Material originalMaterial; // 원래 머티리얼 저장용
    private Renderer objectRenderer;   // 머티리얼 바꿔줄 렌더러

    public GameObject popupUI; // 클릭 시 활성화/비활성화 될 UI

    private bool isUIActive = false; // 현재 UI가 켜져 있는지 상태 확인

    protected override void Awake()
    {
        base.Awake();

        // 오브젝트에 있는 Renderer 가져오기 (머티리얼 변경용)
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            // 시작 시 원래 머티리얼 저장
            originalMaterial = objectRenderer.material;
        }

        // 시작 시 UI 꺼져 있게 설정
        if (popupUI != null)
        {
            popupUI.SetActive(false);
        }
    }

    // Hover 시작: 머티리얼 색 바꾸기
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if (objectRenderer != null && highlightMaterial != null)
        {
            objectRenderer.material = highlightMaterial;
        }
    }

    // Hover 끝: 머티리얼 원래대로 복구
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        if (objectRenderer != null && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }

    // 버튼 누름 (예: 트리거 클릭 등) → UI 토글
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // UI가 연결되어 있을 때만 실행
        if (popupUI != null)
        {
            isUIActive = !isUIActive; // 현재 상태 반전
            popupUI.SetActive(isUIActive); // UI 켜기/끄기
        }
    }
}
