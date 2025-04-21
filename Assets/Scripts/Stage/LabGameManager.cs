///스크립트 생성 일자 - 2025 - 04 - 18
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1.1

using System;
using System.Collections;
using System.Collections.Generic;
using CHG.EventDriven;
using CHG.EventDriven.Arguments;
using UnityEngine;

namespace CHG.Lab
{

	public class ScoreSource : IComparable
	{
		public string id;
		public float score;

        public int CompareTo(object obj)
        {
            if(obj is ScoreSource source)
			{
				return id.CompareTo(source.id);
			}
			else
				return 0;
        }
    }
	public class LabGameManager : MonoBehaviour
	{
		#region Inspector Fields
		public GameObject initialFloor;
		public List<GameObject> floorPrefabs;
		public GameObject currentFloor;
		public float lastHeight = 0;

		public List<ScoreSource> scores = new List<ScoreSource>();

		public float TotalScore
		{
			get 
			{
				float total = 0;
				foreach(var ss in scores)
				{
					total += ss.score;
				}

				return total;
			}
		}

		#endregion

		#region Fields
		Transform _transform;

		int _currentFloor = 0;
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
		#endregion

		#region	Events

		#endregion
		
		#region MonoBehaviour Methods
		protected virtual void Awake()
		{
			CacheComponents();

			if(initialFloor == null) initialFloor = floorPrefabs[0];
		}

        void Start()
        {
			scores.Clear();
            GenerateFloor(initialFloor);
        }

        void OnEnable()
        {
			GlobalEventManager.Instance.Subscribe<EarnScoreArgs>("EarnScore", EarnScore);
			GlobalEventManager.Instance.Subscribe<ClearScoreArgs>("ClearScore", ClearScore);
        }
        void OnDisable()
        {
            
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
		public void GenerateFloor(int index)
		{
			if(index < floorPrefabs.Count)
				GenerateFloor(floorPrefabs[index]);
		}
		public void GenerateFloor(GameObject floorPrefab)
		{
			if(!isLoading)
			{
				isLoading = true;
				StartCoroutine(AsyncGenerateFloor(floorPrefab));
			}
		}

		bool isLoading = false;
		IEnumerator AsyncGenerateFloor(GameObject floorPrefab)
		{
			var operation = InstantiateAsync(floorPrefab, transform, new Vector3(0, lastHeight + 4.25f, 0), Quaternion.identity);

			yield return operation;

			if(operation.isDone && operation.Result != null)
			{
				if(currentFloor != null)
				{
					Destroy(currentFloor);
					currentFloor = null;
				}

				currentFloor = operation.Result[0];
				lastHeight += 4.25f;				
			}
			else
			{

			}

			isLoading = false;
		}


		public void EarnScore(EarnScoreArgs args)
		{
			if(scores.Exists(x => x.id == args.id))
			{
				var s = scores.Find(x => x.id == args.id);
				s.score += args.earnedScore;
			}
			else
			{
				ScoreSource ss = new ScoreSource();
				ss.id = args.id;
				ss.score = args.earnedScore;
				
				scores.Add(ss);
			}			
		}
		public void ClearScore(ClearScoreArgs args)
		{
			if(scores.Exists(x => x.id == args.id))
			{
				var s = scores.Find(x => x.id == args.id);
				s.score = 0;
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
	public class ClearScoreArgs : BaseEventArgs
	{
		public string id;
		public ClearScoreArgs(string id)
		{
			this.id = id;
		}
	}
	public class EarnScoreArgs : BaseEventArgs
	{
		public string id;
		public float earnedScore;

		public EarnScoreArgs(string id, float earnedScore)
		{
			this.id = id;
			this.earnedScore = earnedScore;
		}
	}
}