///스크립트 생성 일자 - 2025 - 04 - 19
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1.1

using System.Collections.Generic;
using CHG.Utilities.Patterns;
using TMPro;
using UnityEngine;

namespace CHG.Lab
{
	public class UIManager : SingletonMonobehaviour<UIManager>
	{
		#region Inspector Fields
		[SerializeField, Tooltip("패널의 기준 Transform")]
		private RectTransform _panelTransform;
		[SerializeField, Tooltip("사용 가능한 UI 패널들")]
		private List<UIPanel> _panels;
		
		#endregion

		#region Fields
		Transform _transform;
		#endregion
		
		#region Properties
		public new Transform transform
		{
			get
			{
				#if UNITY_EDITOR
				if(_transform == null) _transform = GetComponent<Transform>();
				#endif
				return _transform;
			}
		}
		
		/// <summary>
		/// 사용 가능한 UI 패널들
		/// </summary>
		public List<UIPanel> Panels
		{
		   get => _panels;
		   set => _panels = value;
		}
		/// <summary>
		/// 패널의 기준 Transform
		/// </summary>
		public RectTransform PanelTransform
		{
		   get => _panelTransform;
		   set => _panelTransform = value;
		}
		public override bool IsPersistent => false;
		#endregion

		#region	Events

		#endregion
		
		#region MonoBehaviour Methods
		protected virtual void Awake()
		{
			CacheComponents();
		}

        protected override void Start()
        {
            base.Start();
			TogglePanel(0);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 컴퍼넌트를 캐싱
        /// </summary>	
        protected virtual void CacheComponents()
		{
			 _transform = GetComponent<Transform>();
		}
		public void TogglePanel(int index)
		{
			if(index < Panels.Count)
			{
				for(int i = 0; i < Panels.Count; ++i)
				{
					if(i == index)
					{
						Panels[i].gameObject.SetActive(true);
					}
					else
					{
						Panels[i].gameObject.SetActive(false);
					}
				}
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