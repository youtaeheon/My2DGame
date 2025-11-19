using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    [Header("Fade Settings")]
    public CanvasGroup fadePanel;
    public float fadeDuration = 1.2f;

    [Header("Curtain Settings")]
    public RectTransform curtainTop;
    public RectTransform curtainBottom;
    public float curtainSpeed = 800f;

    [Header("Truck Settings")]
    public Transform truck;
    public Vector3 startPos;
    public Vector3 endPos;
    public float truckMoveDuration = 4f;

    [Header("Camera Settings")]
    public Transform cam;
    public Vector3 cameraOffset = new Vector3(0, 1.5f, -10);
    public float cameraFollowSpeed = 4f;

    [Header("Scene Settings")]
    public string nextSceneName;

    void Start()
    {
        curtainTop.anchoredPosition = new Vector2(0, 0);
        curtainBottom.anchoredPosition = new Vector2(0, 0);

        fadePanel.alpha = 1;
        StartCoroutine(CutsceneRoutine());
    }

    IEnumerator CutsceneRoutine()
    {
        yield return Fade(1, 0);
        yield return OpenCurtains();
        yield return FocusCameraOnTruck();
        yield return MoveTruck();
        yield return CloseCurtains();
        yield return Fade(0, 1);

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            fadePanel.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }

        fadePanel.alpha = to;
    }

    IEnumerator OpenCurtains()
    {
        while (curtainTop.anchoredPosition.y < 600f)
        {
            curtainTop.anchoredPosition += new Vector2(0, curtainSpeed * Time.deltaTime);
            curtainBottom.anchoredPosition -= new Vector2(0, curtainSpeed * Time.deltaTime);
            yield return null;
        }

    }
    IEnumerator CloseCurtains()
    {
        while (curtainTop.anchoredPosition.y > 0)
        {
            curtainTop.anchoredPosition -= new Vector2(0, curtainSpeed * Time.deltaTime);
            curtainBottom.anchoredPosition += new Vector2(0, curtainSpeed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator FocusCameraOnTruck()
    {
        float t = 0;
        Vector3 initialPos = cam.position;
        Vector3 targetPos = truck.position + cameraOffset;

        while (t < 1f)
        {
            cam.position = Vector3.Lerp(initialPos, targetPos, t);
            t += Time.deltaTime * cameraFollowSpeed;
            yield return null;
        }
        cam.position = targetPos;
    }
    IEnumerator MoveTruck()
    {
        float t = 0;
        while (t < truckMoveDuration)
        {
            truck.position = Vector3.Lerp(startPos, endPos, t / truckMoveDuration);
            cam.position = truck.position + cameraOffset;
            t += Time.deltaTime;
            yield return null;
        }
        truck.position = endPos;
    }
}
