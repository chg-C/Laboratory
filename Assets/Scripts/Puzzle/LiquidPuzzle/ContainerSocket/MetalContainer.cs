///스크립트 생성 일자 - 2025 - 04 - 17
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CHG.Lab
{
	/// <summary>
	/// 주기적으로 시약을 생성하는 Container
	/// 시약을 집어들면 일정 시간 후 재생성
	/// </summary>

	[RequireComponent(typeof(XRSocketInteractor))]
	public class MetalContainer : MonoBehaviour
	{
		#region Inspector Fields

		[SerializeField, Tooltip("금속 Prefab")]
		private Metal _metalPrefab;

		[SerializeField, Tooltip("금속 Prefab의 무게")]
		private int _weight;

		[SerializeField, Tooltip("금속의 재생성 주기")]
		private float _metalRespawnTimer = 1f;
		
		#endregion

		#region Fields
		private Transform _transform;
		private XRSocketInteractor _socket;
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

		public int Weight
		{
			get => _weight;
			set => _weight = value;
		}

		/// <summary>
		/// 연결된 Socket
		/// </summary>
		public XRSocketInteractor Socket
		{
		   get => _socket;
		   set => _socket = value;
		}
		
		/// <summary>
		/// 금속 Prefab
		/// </summary>
		public Metal MetalPrefab
		{
		   get => _metalPrefab;
		   set => _metalPrefab = value;
		}		
		
		/// <summary>
		/// 금속의 재생성 시간
		/// </summary>
		public float MetalRespawnTimer
		{
		   get => _metalRespawnTimer;
		   set => _metalRespawnTimer = value;
		}
		#endregion

		#region	Events

		#endregion
		
		#region MonoBehaviour Methods
		protected virtual void Awake()
		{
			CacheComponents();
			Weight = MetalPrefab.GetWeight();
		}

        void OnEnable()
        {
			MetalRespawnTimer = 0.2f;
        }
        void OnDisable()
        {
            if(Socket.hasSelection)
			{
				Destroy(Socket.firstInteractableSelected.transform.gameObject);
			}
        }
        void Update()
        {
            if(!Socket.hasSelection)
			{
				MetalRespawnTimer -= Time.deltaTime;
				if(MetalRespawnTimer <= 0f)
				{
					MetalRespawnTimer = 1f;

					SpawnMetal();
				}
			}
			else
			{
				MetalRespawnTimer = 1f;
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
			 _socket = GetComponent<XRSocketInteractor>();
		}        
		/// <summary>
		/// 시약 생성하기
		/// </summary>
		private void SpawnMetal()
        {
            Metal metal = Instantiate(MetalPrefab, Socket.attachTransform.position, Socket.attachTransform.rotation);

			Socket.interactionManager.SelectEnter(Socket, metal.Interactable);
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