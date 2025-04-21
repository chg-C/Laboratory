using System.Collections;
using System.Collections.Generic;
using CHG.EventDriven;
using CHG.Lab;
using UnityEngine;

public class SorterScorer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hello");
        if(other.TryGetComponent(out SortableObject sortable))
        {
            GlobalEventManager.Instance.Publish("EarnScore", new EarnScoreArgs("샘플 분류", sortable.Score));
            Destroy(other.gameObject);
        }
    }
}
