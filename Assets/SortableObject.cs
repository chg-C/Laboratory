///스크립트 생성 일자 - 2025 - 04 - 19
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1.1

using UnityEngine;

namespace CHG.Lab
{
	public class SortableObject : MonoBehaviour
	{
		#region Inspector Fields
		[SerializeField, Tooltip("이 Object의 Score")]
		private float _score;
		
		private float moveSpeed = 1.0f; // 스포너로부터 전달받을 속도
		private Vector3 moveDirection = Vector3.forward; // 스포너로부터 전달받을 방향
		private Rigidbody rb;
		private bool isInitialized = false;
		private bool isGrabbed = false; // XR Grab Interactable에서 상태를 받아올 변수
		/// <summary>
		/// 이 Object의 Score
		/// </summary>
		public float Score
		{
		   get => _score;
		   set => _score = value;
		}
		#endregion

		#region Fields
		Transform _transform;
		#endregion
		
		#region Properties
		public new Transform transform
		{
			get
			{
				#if UNITY_EDITOR
				if(_transform == null) _transform = GetComponent<Transform>();
				#endif
				return _transform;
			}
		}
		#endregion

		#region	Events

		#endregion
		
		#region MonoBehaviour Methods
		protected virtual void Awake()
		{
			CacheComponents();
		}

		// 스포너가 호출하여 이동 파라미터를 설정해주는 함수
		public void Initialize(float speed, Vector3 direction)
		{
			moveSpeed = speed;
			moveDirection = direction.normalized; // 방향 벡터 정규화
			isInitialized = true;

			// XRI Grab Interactable 이벤트에 리스너 등록 (선택적이지만 유용함)
			var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>();
			if (interactable != null)
			{
				interactable.selectEntered.AddListener(OnGrabbed);
				interactable.selectExited.AddListener(OnReleased);
			}
		}

		void FixedUpdate()
		{
			// 초기화가 완료되고, 플레이어가 잡고 있지 않을 때만 이동
			if (isInitialized && !rb.isKinematic)
			{
				// Rigidbody를 이용한 이동 (물리 기반 상호작용에 더 적합)
				Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
				rb.MovePosition(targetPosition);

				// 또는 transform.Translate 사용 (Kinematic일 때나 간단할 때)
				// transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime, Space.World);
			}
		}

		// 샘플이 잡혔을 때 호출될 함수
		private void OnGrabbed(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
		{
			isGrabbed = true;
			rb.isKinematic = true;
			
			// 잡혔을 때는 Rigidbody의 Kinematic 상태나 다른 설정을 변경해야 할 수도 있음
			// 예: rb.isKinematic = false; // 플레이어가 물리적으로 움직일 수 있게
		}

		// 샘플이 놓아졌을 때 호출될 함수
		private void OnReleased(UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs args)
		{
			isGrabbed = false;
			rb.isKinematic = false;
			rb.useGravity = true;
			// 놓아졌을 때 원래 Rigidbody 설정으로 복구
			// 예: rb.useGravity = false; rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero;
		}

		// 벨트 끝 트리거와 충돌 감지
		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("BeltEndTrigger")) // 벨트 끝 트리거의 태그 확인
			{
				Debug.Log("Sample reached the end and was destroyed.");
				// 여기에 게임 오버 조건이나 점수 감점 로직 추가 가능
				// GameManager.Instance.MissedSample();
				Destroy(gameObject); // 오브젝트 제거
			}
		}

		// 이벤트 리스너 해제 (오브젝트 파괴 시 중요)
		void OnDestroy()
		{
			var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>();
			if (interactable != null)
			{
				interactable.selectEntered.RemoveListener(OnGrabbed);
				interactable.selectExited.RemoveListener(OnReleased);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// 컴퍼넌트를 캐싱
		/// </summary>	
		protected virtual void CacheComponents()
		{
			 _transform = GetComponent<Transform>();
			 
			rb = GetComponent<Rigidbody>();
			if (rb == null)
			{
				Debug.LogError("SampleMovement requires a Rigidbody component!");
				enabled = false; // Rigidbody 없으면 스크립트 비활성화
			}
			// Rigidbody 설정을 여기서 하거나 Inspector에서 미리 설정
			// 예: 중력 비활성화, Kinematic 등
			rb.useGravity = false; // 벨트 위에서 중력 영향 안 받게
			// rb.isKinematic = true; // 잡을 때 문제가 없다면 Kinematic도 고려 가능
		}
		#endregion

		
		#region UnityEditor Only Methods
		#if UNITY_EDITOR
		protected virtual void Reset()
		{
		}
		protected virtual void OnValidate()
		{
		}
		#endif
		#endregion
	}
}