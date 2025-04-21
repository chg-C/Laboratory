///스크립트 생성 일자 - 2025 - 04 - 19
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1.1

using TMPro;
using UnityEngine;

namespace CHG.Lab
{

	public enum AnalyzerState
	{
		Waiting,
		Progressing,
		Completed
	}
	public class Analyzer : MonoBehaviour
	{
		#region Inspector Fields
		[SerializeField, Tooltip("현재 상태 표시 Text")]
		private TextMeshPro _stateText;
		
		/// <summary>
		/// 현재 상태 표시 Text
		/// </summary>
		public TextMeshPro StateText
		{
		   get => _stateText;
		   set => _stateText = value;
		}
		#endregion

		#region Fields
		Transform _transform;
		private float _progress;
		
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
		public float Progress
		{
			get => _progress;
			set => _progress = value;
		}
		#endregion

		#region	Events

		#endregion
		
		#region MonoBehaviour Methods
		protected virtual void Awake()
		{
			CacheComponents();
		}

        void Update()
        {
            
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
		}
		protected virtual void OnValidate()
		{
		}
		#endif
		#endregion
	}
}