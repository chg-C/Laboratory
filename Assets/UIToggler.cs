using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIToggler : MonoBehaviour
{
     public GameObject wristUICanvasObject; // 
    public InputActionReference toggleActionReference; // Inspector에서 설정한 Input Action Asset의 ToggleWristUI 액션을 연결해주세요.

    private InputAction toggleAction;

    void Awake()
    {
        if (toggleActionReference == null)
        {
            Debug.LogError("Toggle Action Reference가 연결되지 않았습니다!");
            return;
        }
        toggleAction = toggleActionReference.action;

        // 액션이 시작될 때(눌렸을 때) OnToggleWristUI 메서드를 호출하도록 콜백 등록
        toggleAction.started += OnToggleWristUI;

        if (wristUICanvasObject == null)
        {
            Debug.LogError("손목 UI Canvas 오브젝트가 연결되지 않았습니다!");
        }
    }

    void OnEnable()
    {
        // 스크립트가 활성화될 때 Input Action도 활성화
        if (toggleAction != null)
        {
            toggleAction.Enable();
        }
    }

    void OnDisable()
    {
        // 스크립트가 비활성화될 때 Input Action도 비활성화
        if (toggleAction != null)
        {
            toggleAction.Disable();
        }
    }

    // Input Action이 시작(버튼 눌림)될 때 호출될 함수
    private void OnToggleWristUI(InputAction.CallbackContext context)
    {
        if (wristUICanvasObject != null)
        {
            // UI Canvas의 활성 상태를 토글합니다.
            wristUICanvasObject.SetActive(!wristUICanvasObject.activeSelf);
            // 디버깅용 로그
            // Debug.Log($"Wrist UI Toggled: {wristUICanvasObject.activeSelf}");
        }
    }

    // 스크립트 파괴 시 콜백 제거 (메모리 누수 방지)
    void OnDestroy()
    {
        if (toggleAction != null)
        {
            toggleAction.started -= OnToggleWristUI;
        }
    }
}
