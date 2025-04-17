///스크립트 생성 일자 - 2025 - 04 - 17
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using CHG.EventDriven;
using UnityEngine;

namespace CHG.Lab
{
	public class Beaker : MonoBehaviour, IFillable
	{
		#region Inspector Fields
		[SerializeField, Tooltip("이 비커의 현재 채워진 양")]
		private float _currentVolume;
		[SerializeField, Tooltip("이 비커의 최대 용량"), Min(0.1f)]
		private float _maxVolume = 10;

		[SerializeField, Tooltip("비커의 액체 Renderer")]
		private Renderer _beakerLiquid;

		[SerializeField, Tooltip("초기 색상")]
		private Color _initialColor;
		
		/// <summary>
		/// 비커의 액체 Renderer
		/// </summary>
		public Renderer BeakerLiquid
		{
		   get => _beakerLiquid;
		   set => _beakerLiquid = value;
		}
		#endregion

		#region Fields
		Transform _transform;
		private Color _currentColor;
		
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
		/// 초기 색상
		/// </summary>
		public Color InitialColor
		{
		   get => _initialColor;
		   set => _initialColor = value;
		}
		/// <summary>
		/// 현재 색상
		/// </summary>
		public Color CurrentColor
		{
			get => _currentColor;
			set => _currentColor = value;
		}
		/// <summary>
		/// 이 비커의 현재 채워진 양
		/// </summary>
		public float CurrentVolume
		{
		   get => _currentVolume;
		   set
		   {
			   _currentVolume = value;
		   }
		}
		
		/// <summary>
		/// 이 비커의 최대 용량
		/// </summary>
		public float MaxVolume
		{
		   get => _maxVolume;
		   set => _maxVolume = value;
		}
		
		public float CurrentAmount => CurrentVolume / MaxVolume;

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
            CurrentColor = InitialColor;
        }
        void FixedUpdate()
        {
            BeakerLiquid.transform.localScale = new Vector3(1, CurrentAmount, 1);
			BeakerLiquid.material.color = CurrentColor;
        }

        void OnEnable()
        {
            GlobalEventManager.Instance.Subscribe("ResetLevel", EmptyBeaker);
        }
        void OnDisable()
        {
            if(GlobalEventManager.IsAvailable)
			{
				GlobalEventManager.Instance.Unsubscribe("ResetLevel", EmptyBeaker);
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
		}
        public void Fill(Color color, float power)
        {
			Debug.Log("Filling");

			if(CurrentVolume < MaxVolume)
			{
				if(CurrentVolume > 0.0001f)
				{
					float existingAmount = CurrentVolume - power;
					float lerpFactor = power / CurrentVolume;
					CurrentColor = Color.Lerp(CurrentColor, color, lerpFactor);
				}
				else
				{
					CurrentColor = color;
				}
				CurrentVolume += power;
			}
        }

		public void EmptyBeaker()
		{
			CurrentColor = InitialColor;
			CurrentVolume = 0;
		}
		#endregion

		
		#region UnityEditor Only Methods
		#if UNITY_EDITOR
		protected virtual void Reset()
		{
		}
		protected virtual void OnValidate()
		{
            BeakerLiquid.transform.localScale = new Vector3(1, CurrentAmount, 1);
		}

		#endif
        #endregion
    }
}