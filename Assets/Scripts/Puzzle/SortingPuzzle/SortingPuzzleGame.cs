///스크립트 생성 일자 - 2025 - 04 - 19
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1.1

using UnityEngine;
using UnityEngine.Events;

namespace CHG.Lab
{
	public class SortingPuzzleGame : MonoBehaviour, ILevelingPuzzle
	{
		int kGameID = 2;

        #region Inspector Fields
		bool isPlaying = false;

		[Header("Objects")]
		
		[Header("Level")]
		[SerializeField, Tooltip("게임 시작시 레벨 인덱스")]
		private int _currentLevel;
		[Header("Events")]
		[SerializeField, Tooltip("Game Start 이벤트")]
		private UnityEvent _onStart;
		[SerializeField, Tooltip("Clear 이벤트")]
		private UnityEvent _onClear;
		[SerializeField, Tooltip("레벨 시작")]
		private UnityEvent _onLevelStart;
		[SerializeField, Tooltip("마지막 레벨 Clear 이벤트")]
		private UnityEvent _onClearLastLevel;
        #endregion

        #region Fields
        #endregion

        #region Properties
		
		/// <summary>
		/// 현재 진행중인 레벨
		/// </summary>
        public int CurrentLevel => _currentLevel;
		/// <summary>
		/// 최고 레벨
		/// </summary>
        public int MaxLevel => 1;
        public UnityEvent OnClearLastLevel => _onClearLastLevel;

        public UnityEvent OnStart => _onStart;

        public UnityEvent OnClear => _onClear;

        public UnityEvent OnLevelStart => _onLevelStart;
        #endregion

        #region Methods
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