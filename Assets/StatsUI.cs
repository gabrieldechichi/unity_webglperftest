using UnityEngine;

public class StatsUI : MonoBehaviour
{
    private const int BufferSize = 20;
    private float[] frameDeltas = new float[BufferSize];
    private int frameIndex = 0;
    private float totalDeltaTimes = 0.0f;

    private float averageFPS = 0.0f;
    private float currentDeltaTime = 0.0f;
    private GridSpawner gridSpawner;

    private void Start()
    {
        gridSpawner = FindObjectOfType<GridSpawner>();
    }

    private void Update()
    {
        float newDeltaTime = Time.unscaledDeltaTime;

        totalDeltaTimes -= frameDeltas[frameIndex];
        frameDeltas[frameIndex] = newDeltaTime;
        totalDeltaTimes += newDeltaTime;

        frameIndex = (frameIndex + 1) % BufferSize;

        averageFPS = BufferSize / totalDeltaTimes;
        currentDeltaTime = newDeltaTime;
    }

    private void OnGUI()
    {
        int yOffset = 20;
        int lineHeight = 40;
        int labelWidth = 320;
        int labelHeight = 35;

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 22;
        labelStyle.normal.textColor = Color.white;

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.fontSize = 18;

        GUI.Box(new Rect(10, 10, 350, 320), "Performance Stats", boxStyle);

        GUI.Label(new Rect(20, yOffset + lineHeight * 1, labelWidth, labelHeight),
                  string.Format("Frame Time: {0:0.00} ms", currentDeltaTime * 1000), labelStyle);

        GUI.Label(new Rect(20, yOffset + lineHeight * 2, labelWidth, labelHeight),
                  string.Format("FPS: {0:0.0}", averageFPS), labelStyle);

        GUI.Label(new Rect(20, yOffset + lineHeight * 3, labelWidth, labelHeight),
                  "CPU Memory: 128 MB", labelStyle);

        GUI.Label(new Rect(20, yOffset + lineHeight * 4, labelWidth, labelHeight),
                  "GPU Memory: 256 MB", labelStyle);

        GUI.Label(new Rect(20, yOffset + lineHeight * 5, labelWidth, labelHeight),
                  string.Format("Instance Count: {0}", gridSpawner != null ? gridSpawner.animCount : 0), labelStyle);

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 16;

        if (GUI.Button(new Rect(20, yOffset + lineHeight * 6, 120, 40), "+100", buttonStyle))
        {
            if (gridSpawner != null)
            {
                gridSpawner.SpawnInstances(100);
                gridSpawner.RepositionAllInstances();
            }
        }

        if (GUI.Button(new Rect(150, yOffset + lineHeight * 6, 120, 40), "-100", buttonStyle))
        {
            if (gridSpawner != null)
            {
                gridSpawner.DespawnInstances(100);
                gridSpawner.RepositionAllInstances();
            }
        }
    }
}