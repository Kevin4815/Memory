using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button m_Play;
    public Button m_Quit;

    public void Start()
    {
        m_Play.onClick.AddListener(Play);
        m_Quit.onClick.AddListener(Quit);
    }

    private void Play()
    {
        SceneManager.LoadScene(3);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                                        Application.Quit();
        #endif
    }
}
