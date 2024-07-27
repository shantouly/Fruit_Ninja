using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Image fadeImage;
    public Button start;
    public Button reStart;
    public Button menu;
    private int score = 0;
    private Blade blade;
    private Spawner spawner;

    private void Awake()
    {
        //blade = FindObjectOfType<Blade>();
        //spawner = FindObjectOfType<Spawner>();
    }

    private void Start()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();

        spawner.enabled = false;
        //NewGame();
    }

    private void NewGame()
    {
        Time.timeScale = 1f;
        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();

        ClearScene();
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();
        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach(Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }

        foreach(Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence());
    }

    public void OnButtonClick()
    {
        start.gameObject.SetActive(false);
        reStart.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        NewGame();
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float druation = 0.5f;

        while(elapsed < druation)
        {
            //float t = Mathf.Clamp01(elapsed / druation);
            float t = elapsed / druation;
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        NewGame();
        spawner.enabled = false;
        elapsed = 0;

        while(elapsed < druation)
        {
            float t = Mathf.Clamp01(elapsed / druation);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        start.gameObject.SetActive(true);
    }
}
