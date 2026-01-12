using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] Button exit;
    [SerializeField] Button restart;
    [SerializeField] GameObject resultCanvus;

    private TurnController turnController;

    private void Awake()
    {
        ButtonsAddListener();
    }
    private void ButtonsAddListener()
    {
        exit.onClick.AddListener(OnClickExit);
        restart.onClick.AddListener(OnClickRestart);
    }
    private void OnClickExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    private void OnClickRestart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
