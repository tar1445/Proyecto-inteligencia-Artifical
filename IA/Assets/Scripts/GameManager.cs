using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
  
  
    [Header("Time System")]
    [SerializeField] private float secondsPerHour = 360f; 
    private float timer;
    private int currentHour = 12;

    [Header("UI")]
    [SerializeField] private Text hourText;
    [SerializeField] private GameObject winImage;
    [SerializeField] private GameObject loseImage;

    private bool gameEnded = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ResetGame();
        UpdateUI();
        winImage.SetActive(false);
        loseImage.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        timer += Time.deltaTime;

        if (timer >= secondsPerHour)
        {
            timer = 0f;
            currentHour++;

            if (currentHour > 6)
            {
                StartCoroutine(Win());
            }
            else
            {
                UpdateUI();
            }
        }
    }

    void UpdateUI()
    {
        hourText.text = currentHour + " AM";
    }

    public void PlayerDied()
    {
        if (gameEnded) return;
        StartCoroutine(Lose());
    }

    IEnumerator Win()
    {
        gameEnded = true;
        winImage.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined; 
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Menu"); 
    }

    IEnumerator Lose()
    {
        gameEnded = true;
        loseImage.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined; 
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Menu");
    }

    public void ResetGame()
    {
       
        currentHour = 12;
        timer = 0f;
        gameEnded = false;

        if (winImage != null) winImage.SetActive(false);
        if (loseImage != null) loseImage.SetActive(false);
    }
}