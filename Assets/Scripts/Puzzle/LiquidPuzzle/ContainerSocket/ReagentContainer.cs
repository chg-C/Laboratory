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
	public class ReagentContainer : MonoBehaviour
	{
		#region Inspector Fields

		[SerializeField, Tooltip("시약 Prefab")]
		private Reagent _reagentPrefab;
		[SerializeField, Tooltip("시약 색상")]
		private Color _reagentColor = Color.white;
		[SerializeField, Tooltip("시약의 재생성 주기")]
		private float _reagentRespawnTimer = 1f;
		
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
		/// <summary>
		/// 연결된 Socket
		/// </summary>
		public XRSocketInteractor Socket
		{
		   get => _socket;
		   set => _socket = value;
		}
		
		/// <summary>
		/// 시약 Prefab
		/// </summary>
		public Reagent ReagentPrefab
		{
		   get => _reagentPrefab;
		   set => _reagentPrefab = value;
		}		
		/// <summary>
		/// 시약 색상
		/// </summary>
		public Color ReagentColor
		{
		   get => _reagentColor;
		   set => _reagentColor = value;
		}
		/// <summary>
		/// 시약의 재생성 시간
		/// </summary>
		public float ReagentRespawnTimer
		{
		   get => _reagentRespawnTimer;
		   set => _reagentRespawnTimer = value;
		}
		#endregion

		#region	Events

		#endregion
		
		#region MonoBehaviour Methods
		protected virtual void Awake()
		{
			CacheComponents();
		}

        void OnEnable()
        {
			ReagentRespawnTimer = 0.2f;
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
				ReagentRespawnTimer -= Time.deltaTime;
				if(ReagentRespawnTimer <= 0f)
				{
					ReagentRespawnTimer = 1f;

					SpawnReagent();
				}
			}
			else
			{
				ReagentRespawnTimer = 1f;
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
		private void SpawnReagent()
        {
            Reagent reagent = Instantiate(ReagentPrefab, Socket.attachTransform.position, Socket.attachTransform.rotation);
			reagent.ReagentColor = ReagentColor;

			Socket.interactionManager.SelectEnter(Socket, reagent.Interactable);
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