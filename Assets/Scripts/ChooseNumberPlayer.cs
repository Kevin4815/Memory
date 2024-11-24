using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseNumberPlayer : MonoBehaviour
{
    public static int m_HowPlayers;

    public Button m_OnePlayer;
    public Button m_TwoPlayers;

    void Start()
    {
        m_OnePlayer.onClick.AddListener(OnePlayer);
        m_TwoPlayers.onClick.AddListener(TwoPlayers);
    }

   public void OnePlayer()
   {
        m_HowPlayers = 1;
        SceneManager.LoadScene(1);
   }

    public void TwoPlayers()
    {
        m_HowPlayers = 2;
        SceneManager.LoadScene(2);
    }
}
