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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnClickRestart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
