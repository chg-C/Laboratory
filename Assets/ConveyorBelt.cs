using System.Collections;
using System.Collections.Generic;
using CHG.Lab;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{

    
    [SerializeField] private GameObject[] samplePrefabs; // 생성할 샘플 프리팹 목록 (다양한 종류 가능)
    [SerializeField] private Transform spawnPoint;       // 샘플이 생성될 위치 (이 오브젝트의 위치/방향)
    [SerializeField] private float initialSpawnInterval = 2.0f; // 초기 생성 간격 (초)
    [SerializeField] private float minSpawnInterval = 0.5f;   // 최소 생성 간격
    [SerializeField] private float intervalDecreaseRate = 0.01f; // 초당 간격 감소량 (난이도 상승)
    
    public float spawnX = -7.5f;
    public float limitX = 7.5f;
    
    public float speed;

    [SerializeField] private Vector3 beltDirection = Vector3.forward; // 벨트 이동 방향 (전역 기준)
    private float currentSpawnInterval;
    float currentOffset = 0;

    public Renderer belt;
    void Start()
    {
        if (samplePrefabs == null || samplePrefabs.Length == 0)
        {
            Debug.LogError("Sample Prefabs are not assigned in the Spawner!");
            enabled = false;
            return;
        }
        if (spawnPoint == null)
        {
            spawnPoint = transform; // spawnPoint가 없으면 이 오브젝트의 위치/방향 사용
            Debug.LogWarning("Spawn Point not assigned, using Spawner's transform.");
        }

        // 벨트 방향을 스폰 지점의 로컬 forward로 설정하고 싶다면 주석 해제:
        // beltDirection = spawnPoint.forward;

        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true) // 게임이 실행되는 동안 계속 반복
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            // 생성 로직
            SpawnSample();

            // 난이도 상승: 스폰 간격 점진적 감소 (옵션)
            if (currentSpawnInterval > minSpawnInterval)
            {
                currentSpawnInterval -= intervalDecreaseRate * currentSpawnInterval * Time.deltaTime; // 시간에 따라 감소 (비선형)
                // 또는 단순 감소: currentSpawnInterval -= intervalDecreaseRate;
                currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnInterval); // 최소 간격 이하로 내려가지 않도록 함
            }
        }
    }

    void SpawnSample()
    {
        // 랜덤하게 샘플 프리팹 선택
        int randomIndex = Random.Range(0, samplePrefabs.Length);
        GameObject selectedPrefab = samplePrefabs[randomIndex];

        // 샘플 인스턴스화 (생성)
        GameObject newSample = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);

        // 생성된 샘플의 SampleMovement 스크립트에 이동 파라미터 설정
        SortableObject movementScript = newSample.GetComponent<SortableObject>();
        if (movementScript != null)
        {
            // 벨트 방향을 Spawn Point의 Forward 방향으로 설정하려면 beltDirection 대신 spawnPoint.forward 전달
            movementScript.Initialize(speed, beltDirection);
        }
        else
        {
            Debug.LogError($"Spawned sample '{selectedPrefab.name}' is missing SampleMovement script!", newSample);
            Destroy(newSample); // 문제가 있는 샘플은 즉시 제거
        }
    }

    void FixedUpdate()
    {
        currentOffset += Time.fixedDeltaTime * speed;
        if(currentOffset > 1) currentOffset -= 1;

        belt.material.mainTextureOffset = new Vector2(currentOffset, 0);
    }
}
