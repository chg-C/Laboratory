using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

[RequireComponent(typeof(XRBaseInteractor))] // 이 스크립트는 Interactor에 붙어야 함
public class MoveGrabbedObject : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference moveForwardAction; // 해당 손의 Primary Button Action (A/X)
    public InputActionReference moveBackwardAction; // 해당 손의 Secondary Button Action (B/Y)

    [Header("Movement Settings")]
    public float moveSpeed = 0.5f;
    public float minDistance = 0.1f;
    public float maxDistance = 1.5f;

    // (선택 사항) 특정 태그/컴포넌트를 가진 오브젝트만 이동시킬지 여부
    public bool requireTag = false;
    public string targetTag = "CanBePushedPulled"; // 이 태그가 있는 오브젝트만 이동시킴

    private XRBaseInteractor interactor;
    private IXRSelectInteractable currentSelectedInteractable = null; // 현재 잡고 있는 오브젝트

    void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
    }

    void OnEnable()
    {
        // Interactor의 Select 이벤트에 리스너 등록
        interactor.selectEntered.AddListener(OnSelectEntered);
        interactor.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        // 리스너 해제
        interactor.selectEntered.RemoveListener(OnSelectEntered);
        interactor.selectExited.RemoveListener(OnSelectExited);
        currentSelectedInteractable = null; // 참조 해제
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        currentSelectedInteractable = args.interactableObject;
         // Debug.Log($"{this.name} selected {args.interactableObject.transform.name}");
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
         // Debug.Log($"{this.name} deselected {args.interactableObject.transform.name}");
        // args.interactableObject 가 현재 선택된 것과 같은지 확인하는 것이 더 안전함
        if (currentSelectedInteractable == args.interactableObject)
        {
            currentSelectedInteractable = null;
        }
    }

    void Update()
    {
        // 잡고 있는 오브젝트가 없으면 아무것도 안 함
        if (currentSelectedInteractable == null) return;

        // (선택 사항) 특정 태그를 가진 오브젝트만 이동
        if (requireTag && !currentSelectedInteractable.transform.CompareTag(targetTag))
        {
            return;
        }

        // ----- Interactable의 회전 비활성화 확인 (중요) -----
        // 이 Interactor 스크립트는 Interactable의 회전을 직접 제어하지 않음.
        // 이동시키려는 Interactable 자체의 XRGrabInteractable 설정에서
        // "Track Rotation" 이 False 로 설정되어 있어야 원하는 대로 동작함.
        // (또는 아래에서 강제로 설정할 수도 있지만 권장하지 않음)
        // var grabInteractable = currentSelectedInteractable as XRGrabInteractable;
        // if (grabInteractable != null && grabInteractable.trackRotation) {
        //      grabInteractable.trackRotation = false; // 런타임 변경은 다른 효과 유발 가능
        // }


        // ----- 버튼 입력 읽기 및 이동 로직 -----
        float forwardInput = moveForwardAction.action.ReadValue<float>();
        float backwardInput = moveBackwardAction.action.ReadValue<float>();
        float pressThreshold = 0.1f;

         Vector3 moveVector = Vector3.zero;

        // 현재 손(Interactor)과 오브젝트 사이의 거리 계산
        // Interactor의 Attach Transform 사용 (더 정확할 수 있음)
        Transform interactorAttachTransform = interactor.attachTransform != null ? interactor.attachTransform : interactor.transform;
         // Interactable의 Attach Transform 가져오기 (없으면 자신의 Transform 사용)
         Transform interactableAttachTransform = currentSelectedInteractable.GetAttachTransform(interactor);
        if (interactableAttachTransform == null) interactableAttachTransform = currentSelectedInteractable.transform;

         Vector3 handToObjectVector = interactableAttachTransform.position - interactorAttachTransform.position;
         float currentDistance = handToObjectVector.magnitude;

        if (forwardInput > pressThreshold)
        {
            moveVector = interactor.transform.forward * moveSpeed * Time.deltaTime;

             // 최대 거리 체크
             if (currentDistance + moveVector.magnitude > maxDistance)
             {
                 float projectedMove = Vector3.Dot(moveVector, handToObjectVector.normalized);
                 if (projectedMove > 0) // 멀어지려는 경우에만
                 {
                      // 정확히 최대 거리에 위치하도록 벡터 계산
                     Vector3 targetPosition = interactorAttachTransform.position + handToObjectVector.normalized * maxDistance;
                     moveVector = targetPosition - interactableAttachTransform.position;
                     // 부동소수점 오류 방지 위해 작은 임계값 사용 가능
                     if(moveVector.magnitude < 0.001f) moveVector = Vector3.zero;
                 }
             }
        }
        else if (backwardInput > pressThreshold)
        {
             moveVector = -interactor.transform.forward * moveSpeed * Time.deltaTime;

             // 최소 거리 체크
              float nextDistance = (interactableAttachTransform.position + moveVector - interactorAttachTransform.position).magnitude;
             if (nextDistance < minDistance)
             {
                // 정확히 최소 거리에 위치하도록 벡터 계산
                 Vector3 targetPosition = interactorAttachTransform.position + handToObjectVector.normalized * minDistance;
                  moveVector = targetPosition - interactableAttachTransform.position;
                   if(moveVector.magnitude < 0.001f) moveVector = Vector3.zero;

                  // 더 가까워지려는 경우(음수 투영), 이동을 멈추거나 조정할 수 있음. 위 계산으로 대체됨.
                 // float projectedMove = Vector3.Dot(moveVector, handToObjectVector.normalized);
                 // if (projectedMove < 0) ...
             }
        }

         // 계산된 벡터만큼 잡고 있는 오브젝트 이동
        if (moveVector != Vector3.zero)
        {
            // Rigidbody가 있다면 MovePosition 사용 권장
             Rigidbody rb = currentSelectedInteractable.transform.GetComponent<Rigidbody>();
             if (rb != null)
             {
                 rb.MovePosition(rb.position + moveVector);
             }
             else // Rigidbody 없으면 Transform 직접 이동
             {
                  currentSelectedInteractable.transform.position += moveVector;
             }
        }
    }
}