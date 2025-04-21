using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ContextualRotator : MonoBehaviour
{
    [Header("Configuration")]
    public float rotationSpeed = 10.0f; // 회전 속도

    [Header("Tilt Control")]
    public bool maintainManualTilt = true; // true: 잡은 손의 기울기를 유지, false: 고정된 목표 기울기 사용 (이 경우 아래 targetTilt 필요)
    // public Quaternion targetWorldTilt = Quaternion.identity; // maintainManualTilt가 false일 때 사용할 목표 기울기 (예: 붓기 각도)

    [Header("Required References")]
    public XRBaseInteractable interactable;

    // 내부 상태 변수
    private bool shouldAlign = false;
    private IXRSelectInteractor currentInteractor;

    void Awake()
    {
        if (interactable == null)
            interactable = GetComponent<XRGrabInteractable>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnSelectEntered);
            interactable.selectExited.AddListener(OnRelease);
        }
        else Debug.LogError("Interactable not found!", this);
    }
    private void OnSelectEntered(SelectEnterEventArgs args) { currentInteractor = args.interactorObject; }
    private void OnRelease(SelectExitEventArgs args) { shouldAlign = false; currentInteractor = null; }

    void Update()
    {
        // (옵션) 정렬 상태 아닐 때 손 회전 따라가도록 복구 로직
        if (!shouldAlign && interactable != null && interactable.isSelected && currentInteractor != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, currentInteractor.transform.rotation, Time.deltaTime * rotationSpeed * 0.5f);
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelectEntered);
            interactable.selectExited.RemoveListener(OnRelease);
        }
    }
}