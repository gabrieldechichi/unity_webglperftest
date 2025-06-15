using UnityEngine;
public class GridSpawner : MonoBehaviour
{
    public GameObject myPrefab;
    public AnimationClip clip;
    public int animCount = 20;
    public float spacing = 1.0f;

    void Start()
    {
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(animCount));

        for (int j = 0; j < animCount; j++)
        {
            int gridX = j % gridSize;
            int gridZ = j / gridSize;
            Vector3 position = new Vector3(
                (gridX - gridSize / 2) * spacing,
                0,
                (gridZ - gridSize / 2) * spacing - 2
            );

            var instance = Instantiate(myPrefab, position, Quaternion.Euler(0, 180, 0));

            Animator animator = instance.GetComponent<Animator>();

            if (animator != null)
            {
                // Start the animation at a specific normalized time
                float animationTime = (j * 12.0f) / clip.length;
                animator.Play(0, -1, animationTime);
            }
        }
    }
}