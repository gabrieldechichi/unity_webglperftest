using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private const int BufferSize = 20;  // Size of the circular buffer.
    private float[] frameDeltas = new float[BufferSize];
    private int frameIndex = 0;
    private float totalDeltaTimes = 0.0f;

    private float averageFPS = 0.0f;
    private float currentDeltaTime = 0.0f;

    private void Update()
    {
        // Calculate current frame's delta time
        float newDeltaTime = Time.unscaledDeltaTime;

        // Update the circular buffer to "average" the last 20 deltas
        totalDeltaTimes -= frameDeltas[frameIndex];
        frameDeltas[frameIndex] = newDeltaTime;
        totalDeltaTimes += newDeltaTime;

        frameIndex = (frameIndex + 1) % BufferSize;

        // Calculate average FPS and current delta time
        averageFPS = BufferSize / totalDeltaTimes;
        currentDeltaTime = newDeltaTime;
    }

    private void OnGUI()
    {
        // Display Average FPS and current Delta Time in milliseconds.
        GUI.Label(new Rect(10, 10, 150, 20), string.Format("FPS: {0:0.0}", averageFPS));
        GUI.Label(new Rect(10, 30, 150, 20), string.Format("Delta Time: {0:0.000} ms", currentDeltaTime * 1000));
    }
}