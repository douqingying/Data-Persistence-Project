using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Add this
using System.Linq;

public class MenuManager : MonoBehaviour
{
    //public Text BestScoreText;
    public TMP_InputField NameInputField; // Change type
    public Text Top3ScoreText; // Add this field
    private string top3;

    private void Start()
    {
        // Display Top 3
        for (int i = 0; i < 3; i++)
        {
            int score = PlayerPrefs.GetInt($"TopScore{i+1}", 0);
            string name = PlayerPrefs.GetString($"TopName{i+1}", "None");
            top3 += $"{i+1}. {name} : {score}\n";
        }
        Top3ScoreText.text = top3;
    }

    public void StartGame()
    {
        string playerName = NameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            // Optionally, show a warning to the user here            
            return;
        }

        PlayerPrefs.SetString("PlayerName", NameInputField.text);
        SceneManager.LoadScene("main");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}