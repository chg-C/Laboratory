///스크립트 생성 일자 - 2025 - 04 - 20
///스크립트 담당자 - 이지훈
///스크립트 생성 버전 - 0.1.1

using System.Collections.Generic;
using UnityEngine;

namespace CHG.Lab
{
	[CreateAssetMenu(fileName = "WeightPuzzleLevel", menuName = "WeightPuzzleLevel", order = 100)]
	public class WeightPuzzleLevel : ScriptableObject
	{
		#region Inspector Fields

		[Tooltip("이 레벨의 목표 무게")]
        private int targetWeight;

        [Tooltip("이 레벨에서 사용할 수 있는 금속 종류")]
        private List<MetalData> usableMetals = new List<MetalData>();
		#endregion

		#region Fields
		#endregion
		
		#region Properties
		public int TargetWeight
		{
			get => targetWeight;
			set => targetWeight = value;
		}

		public List<MetalData> UsableMetals
		{
			get => usableMetals;
			set => usableMetals = value;
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