///스크립트 생성 일자 - 2025 - 04 - 19
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1.1

using UnityEngine;

namespace CHG.Lab
{
	public class UIPanel : MonoBehaviour
	{
		#region Inspector Fields
		[SerializeField, Tooltip("패널 이름")]
		private string _panelName;
		
		#endregion

		#region Fields
		#endregion
		
		#region Properties
		/// <summary>
		/// 패널 이름
		/// </summary>
		public string PanelName
		{
		   get => _panelName;
		   set => _panelName = value;
		}
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