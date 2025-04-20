///스크립트 생성 일자 - 2025 - 04 - 20
///스크립트 담당자 - 이지훈
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
        [SerializeField, Tooltip("현재 정답 무게")]
		private int _correctWeight;

		[Header("레벨 데이터")]
        [SerializeField]
        private WeightPuzzleLevel[] _levelDatas;

        [SerializeField, Tooltip("금속 컨테이너들")]
		private MetalContainer[] _metalContainers;

        [Header("저울")]
        [SerializeField]
        private ElectricScale electricScale;

        [Header("UI")]
        [SerializeField]
        private TextMeshPro targetWeightText;   

		[Header("Level")]
		[SerializeField, Tooltip("게임 시작시 레벨 인덱스")]
		private int _currentLevel;

        [Header("Events")]
		[SerializeField, Tooltip("Game Start 이벤트")]
		private UnityEvent _onStart;

		[SerializeField, Tooltip("Clear 이벤트")]
		private UnityEvent _onClear;

		[SerializeField, Tooltip("마지막 레벨 Clear 이벤트")]
		private UnityEvent _onClearLastLevel;
        private int _targetWeight => _levelDatas[_currentLevel].TargetWeight;

        private bool isPlaying = false;
        #endregion

        #region Fields
        #endregion

        #region Properties
        public int CorrectWeight
        {
            get => _correctWeight;
            set => _correctWeight = value;
        }

        public MetalContainer[] MetalContainers
		{
		   get => _metalContainers;
		   set => _metalContainers = value;
		}
		public int CurrentLevel => _currentLevel;
        private WeightPuzzleLevel CurrentLevelData => _levelDatas[CurrentLevel];

        public WeightPuzzleLevel[] LevelDatas
        {
            get => _levelDatas;
            set => _levelDatas = value;
        }

        public int MaxLevel => _levelDatas.Length;

        public UnityEvent OnClearLastLevel => _onClearLastLevel;

        public UnityEvent OnStart => _onStart;

        public UnityEvent OnClear => _onClear;
        #endregion

        #region Methods
        public void SetLevel(int index)
        {
            _currentLevel = index;
            isPlaying = true;
            CorrectWeight = CurrentLevelData.TargetWeight;

            foreach (var container in MetalContainers)
            {
                container.gameObject.SetActive(false);
            }

            // 2. usableMetal의 weight와 일치하는 컨테이너만 활성화
            foreach (var usable in CurrentLevelData.UsableMetals)
            {
                foreach (var container in MetalContainers)
                {
                    if (container.Weight == usable.weight)
                    {
                        container.gameObject.SetActive(true);
                        break; // 하나 찾았으면 다음 usable로 넘어감
                    }
                }
            }

            if (targetWeightText != null)
                targetWeightText.text = $"Target Weight: {_targetWeight}g";

            // 여기에 usableMetals에 따라 UI 또는 생성기 등에 전달 가능

            Debug.Log($"[GameManager] Level {index} 시작 - 목표 {_targetWeight}g");

            // 레벨 리셋 이벤트 발행 가능
        }

        void NextLevel()
        {
            _currentLevel++;
            if (_currentLevel >= _levelDatas.Length)
            {
                Debug.Log("모든 레벨 완료!");
                OnClearLastLevel?.Invoke();
            }
            else
            {
                SetLevel(_currentLevel);
            }
        }

        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            SetLevel(CurrentLevel);
            OnStart?.Invoke();
        }

        void Update()
        {
            if (!isPlaying || electricScale == null) return;

            int currentWeight = electricScale.currentWeight;

            if (currentWeight == _targetWeight)
            {
                Debug.Log("퍼즐 클리어!");
                OnClear?.Invoke();
                isPlaying = false;

                Invoke(nameof(NextLevel), 2.5f);
            }
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