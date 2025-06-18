using UnityEngine;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
{
    public GameObject myPrefab;
    public AnimationClip clip;
    public int animCount = 20;
    public float spacing = 1.0f;
    
    private List<GameObject> spawnedInstances = new List<GameObject>();

    void Start()
    {
        SpawnInstances(animCount);
        RepositionAllInstances();
    }
    
    public void SpawnInstances(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var instance = Instantiate(myPrefab, Vector3.zero, Quaternion.Euler(0, 180, 0));
            spawnedInstances.Add(instance);
            
            Animator animator = instance.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(0, -1, Random.Range(0, clip.length));
            }
        }
        animCount = spawnedInstances.Count;
    }
    
    public void DespawnInstances(int count)
    {
        int toRemove = Mathf.Min(count, spawnedInstances.Count);
        
        for (int i = 0; i < toRemove; i++)
        {
            int lastIndex = spawnedInstances.Count - 1;
            if (lastIndex >= 0 && spawnedInstances[lastIndex] != null)
            {
                DestroyImmediate(spawnedInstances[lastIndex]);
            }
            spawnedInstances.RemoveAt(lastIndex);
        }
        animCount = spawnedInstances.Count;
    }
    
    public void RepositionAllInstances()
    {
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(spawnedInstances.Count));
        
        for (int j = 0; j < spawnedInstances.Count; j++)
        {
            if (spawnedInstances[j] != null)
            {
                int gridX = j % gridSize;
                int gridZ = j / gridSize;
                Vector3 position = new Vector3(
                    (gridX - gridSize / 2) * spacing,
                    0,
                    (gridZ - gridSize / 2) * spacing - 2
                );
                spawnedInstances[j].transform.position = position;
            }
        }
    }
}