///스크립트 생성 일자 - 2025 - 04 - 17
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using UnityEngine;

namespace CHG.Lab
{
	[CreateAssetMenu(fileName = "LiquidPuzzleLevel", menuName = "Puzzle/Liquid Puzzle Level Data", order = 100)]
	public class LiquidPuzzleLevel : ScriptableObject
	{
		#region Inspector Fields
		[SerializeField, Tooltip("이 레벨의 이름")]
		private string _levelName;
		[SerializeField, Tooltip("이 레벨의 설명 혹은 힌트"), TextArea]
		private string _description;

		[SerializeField, Tooltip("클리어시 얻는 기본 점수")]
		private int _clearScore;
		[SerializeField, Tooltip("일치율에 따라 얻는 점수(100% 기준)")]
		private float _similarityScore;
		
		/// <summary>
		/// 일치율에 따라 얻는 점수(100% 기준)
		/// </summary>
		public float SimilarityScore
		{
		   get => _similarityScore;
		   set => _similarityScore = value;
		}
		
		/// <summary>
		/// 클리어시 얻는 기본 점수
		/// </summary>
		public int ClearScore
		{
		   get => _clearScore;
		   set => _clearScore = value;
		}
		

		[SerializeField, Tooltip("정답 색상(랜덤 선택)")]
		private Color[] _correctionColors;
		[SerializeField, Tooltip("사용 가능한 색상 목록(10개까지)")]
		private Color[] _usableColors;

		[SerializeField, Tooltip("레벨 클리어를 위해 채워야 할 최소 %"), Range(0.2f, 0.99f)]
		private float _minAmount = 0.8f;
		
		[SerializeField, Tooltip("레벨 클리어에 필요한 색상 일치율"), Range(0.2f, 0.99f)]
		private float _matchRequired = 0.8f;
		#endregion

		#region Fields
		#endregion
		
		#region Properties
		/// <summary>
		/// 정답 색상(랜덤 선택)
		/// </summary>
		public Color[] CorrectionColors
		{
		   get => _correctionColors;
		   set => _correctionColors = value;
		}
		/// <summary>
		/// 사용 가능한 색상 목록(10개까지)
		/// </summary>
		public Color[] UsableColors
		{
		   get => _usableColors;
		   set => _usableColors = value;
		}
		
		/// <summary>
		/// 채워야 할 최소 %
		/// </summary>
		public float MinAmount
		{
		   get => _minAmount;
		   set => _minAmount = value;
		}
		/// <summary>
		/// 레벨 클리어에 필요한 색상 일치율
		/// </summary>
		public float MatchRequired
		{
		   get => _matchRequired;
		   set => _matchRequired = value;
		}
		
		/// <summary>
		/// 이 레벨의 이름
		/// </summary>
		public string LevelName
		{
		   get => _levelName;
		   set => _levelName = value;
		}
		/// <summary>
		/// 이 레벨의 설명 혹은 힌트
		/// </summary>
		public string Description
		{
		   get => _description;
		   set => _description = value;
		}
		
		#endregion

		#region Methods
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