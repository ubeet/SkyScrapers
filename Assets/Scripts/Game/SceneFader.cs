using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image blackScreen;
    public float fadeSpeed = 1f;
    public bool fadeInOnStart = true;

    private void Start()
    {
        if (fadeInOnStart)
            StartCoroutine(FadeInAfterLoad());
    }

    public void FadeToScene(int sceneId)
    {
        StartCoroutine(FadeOut(sceneId));
    }

    private IEnumerator FadeInAfterLoad()
    {
        yield return null;

        yield return null;

        float a = 1f;
        while (a > 0f)
        {
            a -= Time.deltaTime * fadeSpeed;
            blackScreen.color = new Color(0, 0, 0, a);
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 0);
    }

    private IEnumerator FadeOut(int sceneId)
    {
        float a = 0f;
        while (a < 1f)
        {
            a += Time.deltaTime * fadeSpeed;
            blackScreen.color = new Color(0, 0, 0, a);
            yield return null;
        }

        SceneManager.LoadScene(sceneId);
    }
}