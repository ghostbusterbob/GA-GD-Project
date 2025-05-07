using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadingscreenscript : MonoBehaviour
{
    public GameObject Loadingscreen;
    public Slider LoadingscreenSlider;
    public GameObject startingloadingscreen;

    public void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadASynchronously(sceneIndex));
        Loadingscreen.SetActive(true);
        startingloadingscreen.SetActive(true);
    }
    IEnumerator LoadASynchronously(int SceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            yield return null;
        }
    }
}
