using UnityEngine;

public class Closegameonescape : MonoBehaviour
{
    private float doubleTapTime = 0.3f;
    private float lastTapTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.time - lastTapTime <= doubleTapTime)
            {
                Application.Quit();
            }
            else
            {
                lastTapTime = Time.time;
            }
        }
    }
}