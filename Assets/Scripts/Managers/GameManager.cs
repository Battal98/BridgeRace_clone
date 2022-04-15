using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Bools
    [Header("-- Bools --")]
    public static bool isGameStarted = false;
    public static bool isGameEnded = false;
    public static bool isGameRestarted = false;
    #endregion

    #region GameObject, TextMeshPro and Lists
    [Header("-- Lists --")]
    public List<GameObject> Levels;
    [Space]
    [Header("-- Cams --")]
    public CinemachineVirtualCamera vCamGame;
    [Space]
    #endregion

    #region Player Inputs

    public GameObject Player;
    public float PlayerSpeed;
    public float Gravity = 9.81f;

    #endregion


    public int levelCount = 0;
    public int nextLevel = 0;
    private Volume volume;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        volume = this.GetComponent<Volume>();
    }

    private IEnumerator Start()
    {
        isGameStarted = false;
        isGameEnded = true;
        isGameRestarted = false;
        StartGame();
        UIManager.instance.LevelsText.text = (nextLevel + 1).ToString();
        yield return new WaitForSeconds(0.1f);
        Player = GameObject.FindGameObjectWithTag("Player");

    }
    public void StartGame()
    {
        if (isGameRestarted)
        {
            UIManager.instance.MainMenu.SetActive(false);
            volume.enabled = false;
        }
        levelCount = PlayerPrefs.GetInt("levelCount", levelCount);
        nextLevel = PlayerPrefs.GetInt("nextLevel", nextLevel);

        if (levelCount < 0 || levelCount >= Levels.Count)
        {
            levelCount = 4;
            PlayerPrefs.SetInt("levelCount", levelCount);
        }
        CreateLevel(levelCount);
    }
    // Create Level.
    public void CreateLevel(int Levelindex)
    {
        Instantiate(Levels[Levelindex], new Vector3(0, 0, 0), Levels[Levelindex].transform.rotation);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isGameEnded)
        {
            UIManager.instance.MainMenu.SetActive(false);
            UIManager.instance.GameMenu.SetActive(true);
            volume.enabled = false;
            isGameStarted = true;
            isGameEnded = false;
        }

    }

    public void OnLevelCompleted()
    {
        StartCoroutine(WaitForFinish(0.5f));
    }
    //if Level Failed
    public void OnLevelFailed()
    {
        //Debug.Log("fail");
        UIManager.instance.LoseMenu.SetActive(true);
        UIManager.instance.GameMenu.SetActive(false);
        isGameEnded = true;
        volume.enabled = true;
        isGameStarted = false;
    }

    // Next Level Button
    public void NextLevelButton()
    {
        isGameEnded = false;
        isGameRestarted = true;
        isGameStarted = true;
        levelCount++;
        nextLevel++;
        PlayerPrefs.SetInt("levelCount", levelCount);
        PlayerPrefs.SetInt("nextLevel", nextLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Rest.
    public void RestartButton()
    {
        isGameRestarted = true;
        isGameStarted = true;
        isGameEnded = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator WaitForFinish(float _waitTime)
    {
        isGameEnded = true;
        yield return new WaitForSeconds(_waitTime);
        UIManager.instance.WinMenu.SetActive(true);
        UIManager.instance.GameMenu.SetActive(false);
        volume.enabled = true;
    }
}