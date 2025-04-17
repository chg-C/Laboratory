///스크립트 생성 일자 - 2025 - 04 - 17
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using UnityEngine;

namespace CHG.Lab
{
	public class Gauge3D : MonoBehaviour
	{
		#region Inspector Fields
		
		[SerializeField, Tooltip("현재 값(0~1)"), Range(0, 1)]
		private float _currentValue;

		[SerializeField, Tooltip("Indicator가 지정할 목표 값"), Range(0, 1)]
		private float _targetValue;
		
		/// <summary>
		/// Indicator가 지정할 목표 값
		/// </summary>
		public float TargetValue
		{
		   get => _targetValue;
		   set
		   {
				_targetValue = value;
				Indicator.transform.localPosition = new Vector3(
				_targetValue - 0.5f, 0, 0
				);
		   }
		}
		
		[SerializeField, Tooltip("게이지 Mesh")]
		private Renderer _gaugeRenderer;
		
		[SerializeField, Tooltip("표시를 위한 세로 작대기")]
		private Renderer _indicator;

		[SerializeField, Tooltip("게이지 0일 때 색상")]
		private Color _zeroColor;
		[SerializeField, Tooltip("게이지 1일 때 색상")]
		private Color _oneColor;
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
		
		/// <summary>
		/// 현재 값(0~1)
		/// </summary>
		public float CurrentValue
		{
		   get => _currentValue;
		   set
		   {
			  _currentValue = value;
				GaugeRenderer.transform.localPosition = 
					new Vector3((-0.5f + (_currentValue/2)), 0, 0);
				GaugeRenderer.transform.localScale =
					new Vector3(_currentValue, 1, 1);
			  if(Application.isPlaying)
			  {
				GaugeRenderer.material.color = 
					Color.Lerp(ZeroColor, OneColor, _currentValue);
			  }
		   }
		}
		

		/// <summary>
		/// 게이지 0일 때 색상
		/// </summary>
		public Color ZeroColor
		{
		   get => _zeroColor;
		   set => _zeroColor = value;
		}
		/// <summary>
		/// 게이지 1일 때 색상
		/// </summary>
		public Color OneColor
		{
		   get => _oneColor;
		   set => _oneColor = value;
		}
		
		/// <summary>
		/// 게이지 Mesh
		/// </summary>
		public Renderer GaugeRenderer
		{
		   get => _gaugeRenderer;
		   set => _gaugeRenderer = value;
		}
		/// <summary>
		/// 표시를 위한 세로 작대기
		/// </summary>
		public Renderer Indicator
		{
		   get => _indicator;
		   set => _indicator = value;
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
            CurrentValue = _currentValue;
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
		#endregion

		
		#region UnityEditor Only Methods
		#if UNITY_EDITOR
		protected virtual void Reset()
		{
			CurrentValue = 0;
			TargetValue = 0;
		}
		protected virtual void OnValidate()
		{
			CurrentValue = _currentValue;
			TargetValue = _targetValue;
		}
		#endif
		#endregion
	}
}