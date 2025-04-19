///스크립트 생성 일자 - 2025 - 04 - 20
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1.1

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CHG.Lab
{
	public class WeightPuzzleGame : MonoBehaviour, ILevelingPuzzle
	{
        #region Inspector Fields
		[Header("레벨 데이터")]
        [SerializeField]
        private List<WeightPuzzleLevel> levels;

        [Header("저울")]
        [SerializeField]
        private ElectricScale electricScale;

        [Header("UI")]
        [SerializeField]
        private TextMeshPro targetWeightText;   

		[Header("Level")]
		[SerializeField, Tooltip("게임 시작시 레벨 인덱스")]
		private int currentLevel;

        [Header("Events")]
		[SerializeField, Tooltip("Game Start 이벤트")]
		private UnityEvent onStart;
		[SerializeField, Tooltip("Clear 이벤트")]
		private UnityEvent onClear;
		[SerializeField, Tooltip("마지막 레벨 Clear 이벤트")]
		private UnityEvent onClearLastLevel;
        #endregion

        #region Fields
        #endregion

        #region Properties
		public int CurrentLevel => currentLevel;

        public int MaxLevel => levels.Count;

        public UnityEvent OnClearLastLevel => onClearLastLevel;

        public UnityEvent OnStart => onStart;

        public UnityEvent OnClear => onClear;
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