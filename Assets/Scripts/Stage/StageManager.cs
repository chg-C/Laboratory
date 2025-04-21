///스크립트 생성 일자 - 2025 - 04 - 16
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CHG.Lab
{
	public class StageManager : MonoBehaviour
	{
		#region Inspector Fields
		//
		[SerializeField, Tooltip("스테이지 시작 이벤트")]
		private UnityEvent _onStageStart;
		[SerializeField, Tooltip("스테이지 클리어 이벤트")]
		private UnityEvent _onStageClear;
		
		/// <summary>
		/// 스테이지 시작 이벤트
		/// </summary>
		public UnityEvent OnStageStart
		{
		   get => _onStageStart;
		   set => _onStageStart = value;
		}
		/// <summary>
		/// 스테이지 클리어 이벤트
		/// </summary>
		public UnityEvent OnStageClear
		{
		   get => _onStageClear;
		   set => _onStageClear = value;
		}

		float backup = 0;

        #endregion

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods
        void Start()
        {
            OnStageStart?.Invoke();
        }
        #endregion

        #region MonoBehaviour Methods
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