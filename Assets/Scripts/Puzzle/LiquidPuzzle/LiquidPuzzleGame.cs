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
		string kGameID = "색상 혼합";

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
		[SerializeField, Tooltip("레벨 시작")]
		private UnityEvent _onLevelStart;
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

        public UnityEvent OnLevelStart => _onLevelStart;
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

			OnLevelStart?.Invoke();
        }
		public static float CalculateRGBSimilaritySimple(Color color1, Color color2)
		{
			 // Use Vector3 for distance calculation in RGB space
			Vector3 rgb1 = new Vector3(color1.r, color1.g, color1.b);
			Vector3 rgb2 = new Vector3(color2.r, color2.g, color2.b);

			float distance = Vector3.Distance(rgb1, rgb2);

			return 1 - distance;
		}
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
			OnStart?.Invoke();
			GlobalEventManager.Instance.Publish("ClearScore", new ClearScoreArgs(kGameID));
            SetLevel(CurrentLevel);
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
					ClearLevel(matched);
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

        private void ClearLevel(float matched)
        {
			OnClear?.Invoke();
			MatchPercents.text = "CLEAR!";
			isPlaying = false;

			float score = CurrentLevelData.ClearScore + (CurrentLevelData.SimilarityScore * matched);
			GlobalEventManager.Instance.Publish("EarnScore", new EarnScoreArgs(kGameID, score));


			Invoke("NextLevel", 2.5f);
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