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