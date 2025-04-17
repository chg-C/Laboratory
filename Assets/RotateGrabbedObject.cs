using UnityEngine;
using UnityEngine.InputSystem; // Input System 사용
using UnityEngine.XR.Interaction.Toolkit;

// 이 스크립트는 XR Interactor (예: XRDirectInteractor, XRRayInteractor)가 있는
// 컨트롤러 GameObject에 추가해야 합니다.
[RequireComponent(typeof(XRBaseInteractor))]
public class RotateGrabbedObject : MonoBehaviour
{
    [Header("Input Action References")]
    [Tooltip("Primary 버튼에 해당하는 Input Action (예: A/X 버튼)")]
    public InputActionReference primaryButtonAction;

    [Tooltip("Secondary 버튼에 해당하는 Input Action (예: B/Y 버튼)")]
    public InputActionReference secondaryButtonAction;

    [Header("Rotation Settings")]
    [Tooltip("초당 회전 속도 (도)")]
    public float rotationSpeed = 90.0f;

    private XRBaseInteractor interactor;
    private IXRSelectInteractable grabbedInteractable = null; // 캐싱

    void Awake()
    {
        // 스크립트가 붙어있는 GameObject에서 Interactor 컴포넌트 가져오기
        interactor = GetComponent<XRBaseInteractor>();
    }

    void OnEnable()
    {
        // Input Action 활성화
        if (primaryButtonAction != null) primaryButtonAction.action.Enable();
        if (secondaryButtonAction != null) secondaryButtonAction.action.Enable();

        // Interactable 선택/해제 이벤트 구독 (선택적이지만 캐싱에 좋음)
        interactor.selectEntered.AddListener(OnGrab);
        interactor.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        // Input Action 비활성화
        if (primaryButtonAction != null) primaryButtonAction.action.Disable();
        if (secondaryButtonAction != null) secondaryButtonAction.action.Disable();

        // 이벤트 구독 해제
        interactor.selectEntered.RemoveListener(OnGrab);
        interactor.selectExited.RemoveListener(OnRelease);

        grabbedInteractable = null; // 참조 해제
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // 현재 잡은 Interactable 저장
        grabbedInteractable = args.interactableObject;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // 잡고 있는 Interactable 해제
        if (grabbedInteractable == args.interactableObject)
        {
             grabbedInteractable = null;
        }
    }

    void Update()
    {
        // 1. 현재 Interactable을 잡고 있는지 확인
        // if (!interactor.hasSelection) return; // 방법 A: 매번 확인
        if (grabbedInteractable == null) return; // 방법 B: 캐싱된 변수 확인 (더 효율적)

        // 2. 잡고 있는 Interactable의 Transform 가져오기
        Transform grabbedTransform = grabbedInteractable.transform;
        if (grabbedTransform == null) return; // 혹시 모를 경우 대비

        // 3. Primary 버튼 입력 감지 및 회전 (X+ 방향)
        if (primaryButtonAction != null && primaryButtonAction.action.IsPressed())
        {
            // 로컬 X축 기준으로 회전 (Vector3.right 사용)
            grabbedTransform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.Self);
        }
        // 4. Secondary 버튼 입력 감지 및 회전 (X- 방향)
        else if (secondaryButtonAction != null && secondaryButtonAction.action.IsPressed()) // else if로 동시 입력 시 한쪽 우선 처리
        {
            // 로컬 X축 반대 방향으로 회전
            grabbedTransform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}