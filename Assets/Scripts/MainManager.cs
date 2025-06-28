using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string m_PlayerName; // Add this field

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerName = PlayerPrefs.GetString("PlayerName", "Player"); // Load player name

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        // Initialize score display with player name
        ScoreText.text = $"Score : {m_Points}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        // Load existing top 3 scores and names
        int[] topScores = new int[3];
        string[] topNames = new string[3];
        for (int i = 0; i < 3; i++)
        {
            topScores[i] = PlayerPrefs.GetInt($"TopScore{i + 1}", 0);
            topNames[i] = PlayerPrefs.GetString($"TopName{i + 1}", "None");
        }

        // Insert current score if it's in the top 3
        for (int i = 0; i < 3; i++)
        {
            if (m_Points > topScores[i])
            {
                // Shift lower scores down
                for (int j = 2; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                    topNames[j] = topNames[j - 1];
                }
                // Insert new score
                topScores[i] = m_Points;
                topNames[i] = m_PlayerName;
                break;
            }
        }


        // Save updated top 3 to PlayerPrefs
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt($"TopScore{i + 1}", topScores[i]);
            PlayerPrefs.SetString($"TopName{i + 1}", topNames[i]);
        }
        PlayerPrefs.Save();
    }
}
