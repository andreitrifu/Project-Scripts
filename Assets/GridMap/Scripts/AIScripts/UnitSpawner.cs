using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject[] unitPrefabs; 
    public Vector3 spawnLocation; 
    public int[] unitCounts; 
    public float spawnInterval = 5f; 

    void Start()
    {
        StartCoroutine(SpawnUnitsRoutine());
    }

    IEnumerator SpawnUnitsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Randomly select a unit prefab
            int randomIndex = Random.Range(0, unitPrefabs.Length);
            GameObject selectedPrefab = unitPrefabs[randomIndex];

            // Get the corresponding unit count
            int unitCountIndex = Mathf.Clamp(randomIndex, 0, unitCounts.Length - 1);
            int selectedUnitCount = unitCounts[unitCountIndex];

            // Spawn the selected number of units of the selected prefab at the spawn location
            for (int i = 0; i < selectedUnitCount; i++)
            {
                Instantiate(selectedPrefab, spawnLocation, Quaternion.identity);
            }
        }
    }
}
