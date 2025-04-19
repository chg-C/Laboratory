///스크립트 생성 일자 - 2025 - 04 - 18
///스크립트 담당자 - 이지훈
///스크립트 생성 버전 - 0.1.1

using UnityEngine;

namespace CHG.Lab
{
	[CreateAssetMenu(fileName = "MetalData", menuName = "MetalData", order = 100)]
	public class MetalData : ScriptableObject
	{
		#region Inspector Fields
		[Tooltip("금속 이름")]
		public string metalName;
		/// <summary>
		/// 
		/// </summary>
		[Tooltip("gram")]
		public int weight;
		#endregion

		#region Fields
		#endregion
		
		#region Properties
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