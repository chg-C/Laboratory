///스크립트 생성 일자 - 2025 - 04 - 16
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using System;
using System.Security.Cryptography;
using CHG.EventDriven;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CHG.Lab
{
	public class LiquidPuzzleGame : MonoBehaviour, ILevelingPuzzle
	{
		#region Inspector Fields
		[Header("Correction")]
		[SerializeField, Tooltip("현재 정답 색상")]
		private Color _correctionColor;
		[SerializeField, Tooltip("정답 색상 출력용 MeshRenderer")]
		private MeshRenderer _correctionPresentor;

		[Header("Pallete")]
		[SerializeField, Tooltip("시약 컨테이너들")]
		private ReagentContainer[] _reagentContainers;

		[Header("Guides")]
		[SerializeField, Tooltip("일치율 표시 TMPro")]
		private TextMeshPro _matchPercents;
		[SerializeField, Tooltip("일치율 표시 3D 게이지")]
		private Gauge3D _gauge3D;
		
		[Header("Level")]
		[SerializeField, Tooltip("게임 시작시 레벨 인덱스")]
		private int _currentLevel;
		[SerializeField, Tooltip("게임 레벨 설정")]
		private LiquidPuzzleLevel[] _levelData;

		[Header("Events")]
		[SerializeField, Tooltip("Game Start 이벤트")]
		private UnityEvent _onStart;
		[SerializeField, Tooltip("Clear 이벤트")]
		private UnityEvent _onClear;
		[SerializeField, Tooltip("마지막 레벨 Clear 이벤트")]
		private UnityEvent _onClearLastLevel;
		
		//TODO: TEMP
		public Beaker beaker;
		public bool isPlaying;
		
		#endregion

		#region Fields
		#endregion
		
		#region Properties
		/// <summary>
		/// 정답 색상
		/// </summary>
		public Color CorrectionColor
		{
		   get => _correctionColor;
		   set {
			#if !UNITY_EDITOR
			if(value == _correctionColor)
			{
				return;
			}
			#endif
			_correctionColor = value;
			if(CorrectionPresentor != null && CorrectionPresentor.sharedMaterial != null)
				CorrectionPresentor.sharedMaterial.color = value;
		   }
		}
		/// <summary>
		/// 정답 색상 출력용 MeshRenderer
		/// </summary>
		public MeshRenderer CorrectionPresentor
		{
		   get => _correctionPresentor;
		   set => _correctionPresentor = value;
		}
				/// <summary>
		/// 일치율 표시 3D 게이지
		/// </summary>
		public Gauge3D Gauge3D
		{
		   get => _gauge3D;
		   set => _gauge3D = value;
		}
		
		/// <summary>
		/// 일치율 표시 TMPro
		/// </summary>
		public TextMeshPro MatchPercents
		{
		   get => _matchPercents;
		   set => _matchPercents = value;
		}
		
		/// <summary>
		/// 시약 컨테이너들
		/// </summary>
		public ReagentContainer[] ReagentContainers
		{
		   get => _reagentContainers;
		   set => _reagentContainers = value;
		}

		/// <summary>
		/// 현재 진행중인 레벨
		/// </summary>
        public int CurrentLevel => _currentLevel;
		private LiquidPuzzleLevel CurrentLevelData => _levelData[CurrentLevel];
		
		/// <summary>
		/// 게임 레벨 설정
		/// </summary>
		public LiquidPuzzleLevel[] LevelData
		{
		   get => _levelData;
		   set => _levelData = value;
		}
		/// <summary>
		/// 최고 레벨
		/// </summary>
        public int MaxLevel => LevelData.Length;

        public UnityEvent OnClearLastLevel => _onClearLastLevel;

        public UnityEvent OnStart => _onStart;

        public UnityEvent OnClear => _onClear;
        #endregion

        #region Methods
		public void SetCorrectionColor(Color color)
		{
			CorrectionColor = color;
			CorrectionPresentor.sharedMaterial.color = color;
		}
        private void SetLevel(int currentLevel)
        {
			_currentLevel = currentLevel;
			Gauge3D.TargetValue = CurrentLevelData.MatchRequired;
			CorrectionColor = CurrentLevelData.CorrectionColors[UnityEngine.Random.Range(0, CurrentLevelData.CorrectionColors.Length)];

			for(int i = 0; i < ReagentContainers.Length; ++i)
			{
				if(CurrentLevelData.UsableColors.Length > i)
				{
					ReagentContainers[i].ReagentColor = CurrentLevelData.UsableColors[i];
					ReagentContainers[i].gameObject.SetActive(true);
				}
				else
				{
					ReagentContainers[i].gameObject.SetActive(false);
				}
			}

			isPlaying = true;

			GlobalEventManager.Instance.Publish("ResetLevel");
        }
		public static float CalculateRGBSimilaritySimple(Color colorA, Color colorB)
		{
			// 각 RGB 채널의 절대 차이를 계산합니다. (각 값은 0.0 ~ 1.0 범위)
			float diffR = Mathf.Abs(colorA.r - colorB.r);
			float diffG = Mathf.Abs(colorA.g - colorB.g);
			float diffB = Mathf.Abs(colorA.b - colorB.b);

			// 총 차이값 계산 (범위: 0.0 ~ 3.0)
			float totalDifference = diffR + diffG + diffB;

			// 총 차이를 0.0 ~ 1.0 범위로 정규화합니다. (최대 차이는 3.0)
			float normalizedDifference = totalDifference / 3.0f;

			// 유사도는 1.0 에서 정규화된 차이를 뺀 값입니다.
			float similarity = 1.0f - normalizedDifference;

			return similarity;
		}
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            SetLevel(CurrentLevel);
			OnStart?.Invoke();
        }
        void FixedUpdate()
        {
			if(isPlaying)
			{
				float matched = beaker.CurrentAmount < 0.01f ? 0 :
								CalculateRGBSimilaritySimple(CorrectionColor, beaker.CurrentColor);
				if(matched >= CurrentLevelData.MatchRequired && 
					beaker.CurrentAmount >= CurrentLevelData.MinAmount)
				{
					OnClear?.Invoke();
					MatchPercents.text = "CLEAR!";
					isPlaying = false;

					Invoke("NextLevel", 2.5f);
				}
				else
				{
					MatchPercents.text = (matched * 100).ToString("f02")+"%";
					Gauge3D.CurrentValue = matched;
				}
			}
			else
			{

			}
        }
		//TEMP
		void NextLevel()
		{
			++_currentLevel;
			if(CurrentLevel >= LevelData.Length)
			{
				//모든 레벨 클리어
				OnClearLastLevel?.Invoke();
			}
			else
			{
				//다음 레벨 시작
				SetLevel(_currentLevel);
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
			CorrectionColor = _correctionColor;
		}
		#endif
		#endregion
	}
}