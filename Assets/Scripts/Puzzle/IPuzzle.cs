///스크립트 생성 일자 - 2025 - 04 - 17
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using UnityEngine;
using UnityEngine.Events;

namespace CHG.Lab
{
	/// <summary>
	/// 시작 / 클리어 가능한 퍼즐
	/// </summary>
	public interface IPuzzle
	{
		public UnityEvent OnStart {get;}
		public UnityEvent OnClear {get;}
	}

	/// <summary>
	/// 퍼즐에 레벨 개념 추가
	/// </summary>
	public interface ILevelingPuzzle : IPuzzle
	{		
		public int CurrentLevel {get;}
		public int MaxLevel {get;}


		public UnityEvent OnClearLastLevel {get;}
	}
}