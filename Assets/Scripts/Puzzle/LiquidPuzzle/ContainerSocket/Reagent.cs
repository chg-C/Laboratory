///스크립트 생성 일자 - 2025 - 04 - 17
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace CHG.Lab
{
	[RequireComponent(typeof(XRGrabInteractable))]
	public class Reagent : MonoBehaviour
	{
		#region Inspector Fields

		[Header("Liquid")]
		[SerializeField, Tooltip("시약의 색상")]
		private Color _reagentColor;
		[SerializeField, Tooltip("시약을 출력할 Renderer")]
		private Renderer _reagentPresentor;
		public float tempAmount = 1;

		[Header("Pour")]
		[SerializeField, Tooltip("기본 붓는 속도(초당)")]
		private float _basePourRate = 0.1f;
		
		[SerializeField, Tooltip("내용물이 떨어지는 데 필요한 각도")]
		private float _pouringAngle = 45;

		[SerializeField, Tooltip("액체 낙하가 발생하는 위치(이 아래에 있으면 = 액체가 떨어짐)")]
		private Transform _pourPoint;
		
		[SerializeField, Tooltip("액체 낙하 Particle")]
		private ParticleSystem _pourParticle;

		[SerializeField, Tooltip("낙하하는 액체와 상호작용할 Layer")]
		private LayerMask _pouringLayer;

		[SerializeField, Tooltip("최대 낙하 체크 거리")]
		private float _pourMaxDistance = 10;
		
		#endregion

		#region Fields
		Transform _transform;
		private XRGrabInteractable _interactable;

		private float _destroyTimer;
		
		private bool _waitForDestroy;

		private float _pouringPower;
		private bool _isPouring;

		private RaycastHit _hitData;
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
		public XRGrabInteractable Interactable
		{
			get => _interactable;
			set => _interactable = value;
		}
		private float DestroyTimer
		{
			get => _destroyTimer;
			set => _destroyTimer = value;
		}
		public bool WaitForDestroy
		{
			get => _waitForDestroy;
			set
			{
				if(value == false)
				{
					DestroyTimer = 1f;
				}
			}
		}
		/// <summary>
		/// 기본 붓는 속도(초당)
		/// </summary>
		public float BasePourRate
		{
		   get => _basePourRate;
		   set => _basePourRate = value;
		}
		public float CurrentPourRate => BasePourRate * PouringPower;
		/// <summary>
		/// 내용물이 떨어지는 데 필요한 각도
		/// </summary>
		public float PouringAngle
		{
		   get => _pouringAngle;
		   set => _pouringAngle = value;
		}
		/// <summary>
		/// 액체가 낙하하는 힘 계수
		/// </summary>
		public float PouringPower
		{
			get => _pouringPower;
			set => _pouringPower = value;
		}
		/// <summary>
		/// 낙하 진행 여부
		/// </summary>
		public bool IsPouring
		{
			get => _isPouring;
			set => _isPouring = value;
		}
		
		/// <summary>
		/// 최대 낙하 체크 거리
		/// </summary>
		public float PourMaxDistance
		{
		   get => _pourMaxDistance;
		   set => _pourMaxDistance = value;
		}
		
		/// <summary>
		/// 액체 낙하가 발생하는 위치(이 아래에 있으면 = 액체가 떨어짐)
		/// </summary>
		public Transform PourPoint
		{
		   get => _pourPoint;
		   set => _pourPoint = value;
		}
		/// <summary>
		/// 낙하하는 액체와 상호작용할 Layer
		/// </summary>
		public LayerMask PouringLayer
		{
		   get => _pouringLayer;
		   set => _pouringLayer = value;
		}
		
		/// <summary>
		/// 액체 낙하 Particle
		/// </summary>
		public ParticleSystem PourParticle
		{
		   get => _pourParticle;
		   set => _pourParticle = value;
		}
		
		
		/// <summary>
		/// 시약의 색상
		/// </summary>
		public Color ReagentColor
		{
		   get => _reagentColor;
		   set
		   {
			   _reagentColor = value;
			   if(Application.isPlaying)
			   {
			   	ReagentPresentor.material.color = _reagentColor;
				if(PourParticle != null)
				{
					var main = PourParticle.main;
					main.startColor = _reagentColor;
				}
			   }
		   }
		}
		/// <summary>
		/// 시약 출력용 Renderer
		/// </summary>
		public Renderer ReagentPresentor
		{
		   get => _reagentPresentor;
		   set => _reagentPresentor = value;
		}

		/// <summary>
		/// Pour Raycast 데이터 캐시
		/// </summary>
		public RaycastHit hitData
		{
			get => _hitData;
			set => _hitData = value;
		}
        #endregion

        #region	Events
        #endregion

        #region MonoBehaviour Methods
        protected virtual void Awake()
		{
			CacheComponents();
		}
        void Start()
        {
            ReagentColor = _reagentColor;
        }
        void Update()
        {
            if(!Interactable.isSelected)
			{
				DestroyTimer -= Time.deltaTime;
				if(DestroyTimer <= 0)
				{
					Destroy(this.gameObject);
				}
			}
			else
			{
				DestroyTimer = 1f;
				HandlePouring();
			}
			ReagentPresentor.transform.localScale = new Vector3(1, tempAmount/5, 1);
        }

        private void HandlePouring()
        {
			if(tempAmount <= 0f)
			{
				if(IsPouring) StopPouring();
				return;
			}

			float tiltedAngle = CalculateTiltAngle();
			if(tiltedAngle > PouringAngle)
			{
				float maxExcessTilt = 180.0f; // 예시: 최대로 더 기울일 수 있는 각도 (조절 필요)
				float minMultiplier = 0.5f;  // 최소 배율
				float maxMultiplier = 7.0f;  // 최대 배율

				float excessTilt = tiltedAngle - PouringAngle;
				
				// excessTilt를 0~1 범위로 정규화
				float normalizedExcessTilt = Mathf.Clamp01(excessTilt / maxExcessTilt);

				PouringPower = Mathf.Lerp(minMultiplier, maxMultiplier, normalizedExcessTilt);

				if(!IsPouring) StartPouring();

				Pour();
			}
			else
			{
				if(IsPouring) StopPouring();
			}
        }

        private void Pour()
        {
			var emission = PourParticle.emission;
			emission.rateOverTime = CurrentPourRate * 200;
			float power = BasePourRate * PouringPower * Time.deltaTime;

			if(Physics.Raycast(PourPoint.position, Vector3.down, out _hitData, PourMaxDistance, PouringLayer, QueryTriggerInteraction.Collide))
			{
				var fillable = hitData.collider.GetComponentInParent<IFillable>();
				if(fillable != null)
				{
					fillable.Fill(ReagentColor, power);
				}
			}

			tempAmount -= power;
			
        }

        private void StopPouring()
        {
            IsPouring = false;
			if(PourParticle != null)
			{
				PourParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
        }

        private void StartPouring()
        {
			IsPouring = true;
			if(PourParticle != null)
			{
				PourParticle.Play();
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
			_interactable = GetComponent<XRGrabInteractable>();
		}

		protected virtual float CalculateTiltAngle()
		{
			return Vector3.Angle(transform.up, Vector3.up);
		}
		#endregion

		
		#region UnityEditor Only Methods
		#if UNITY_EDITOR
		protected virtual void Reset()
		{
		}
		protected virtual void OnValidate()
		{
			ReagentColor = _reagentColor;
		}
		#endif
		#endregion
	}
}