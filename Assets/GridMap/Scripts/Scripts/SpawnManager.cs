using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject soldierPrefab1; // Soldier class 1 prefab
    public GameObject soldierPrefab2; // Soldier class 2 prefab
    public GameObject soldierPrefab3; // Soldier class 3 prefab
    public GameObject spawnReference;

    // Add a reference to the buttons
    public Button button1;
    public Button button2;
    public Button button3;

    private void Start()
    {
        // Attach button click events
        button1.onClick.AddListener(OnButton1Click);
        button2.onClick.AddListener(OnButton2Click);
        button3.onClick.AddListener(OnButton3Click);
    }

    public void SpawnSoldiers(int numberOfSoldiers, GameObject soldierPrefab)
    {
        for (int i = 0; i < numberOfSoldiers; i++)
        {
            // Customize the spawn position as needed
            Vector3 spawnPosition = spawnReference.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));

            // Instantiate the soldier prefab at the specified position
            Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
        }
    }


    // Button click events
    public void OnButton1Click()
    {
        SpawnSoldiers(3, soldierPrefab1); // Spawn 3 soldiers of class 1
        StartCoroutine(DisableButtonForSeconds(button1, 5f));
    }

    public void OnButton2Click()
    {
        SpawnSoldiers(3, soldierPrefab2); // Spawn 4 soldiers of class 2
        StartCoroutine(DisableButtonForSeconds(button2, 5f));
    }

    public void OnButton3Click()
    {
        SpawnSoldiers(2, soldierPrefab3); // Spawn 2 soldiers of class 3
        StartCoroutine(DisableButtonForSeconds(button3, 5f));
    }

    // Coroutine to disable a button for a specified duration
    private IEnumerator DisableButtonForSeconds(Button button, float seconds)
    {
        button.interactable = false;
        yield return new WaitForSeconds(seconds);
        button.interactable = true;
    }
}