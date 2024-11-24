using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public GameObject m_Square;
    public GameObject m_GameOver;
    public GameObject m_StopBar;
    public GameObject m_WichPlayer;
    public Image m_ScoreBoxPlayerOne;
    public Image m_ScoreBoxPlayerTwo;

    public Transform m_Panel;
    public TextMeshProUGUI m_ScoreText;
    public TextMeshProUGUI m_TestText;
    public TextMeshProUGUI m_ScoreText2;

    public Button m_Restart;
    public Button m_Quit;
    public Button m_Mute;
    public Button m_Stop;
    public Button m_Continue;
    public Button m_ReturnToMenu;

    public Sprite m_MuteSprite;
    public Sprite m_SoundSprite;

    public ColorBlock m_ColorBlock;

    public List<Sprite> m_TestSprite = new List<Sprite>();
    public List<Sprite> m_t = new List<Sprite>();

    public bool m_ItsOver;
    bool m_AllowedScale = true;
    bool m_DisplayStopBar = false;

    AudioSource m_Audio;
    public AudioClip m_Win;

    public static int m_Test1 = 0;
    int m_Rand;
    int m_RandImg;
    int m_PlayGameOverOneTime = 0;

    List<string> m_NbToFind = new List<string>();
    List<GameObject> m_SquaresObject = new List<GameObject>();

    void Start()
    {
        OnClick.m_TurnIsOver = true;
        m_ScoreBoxPlayerOne.GetComponent<Image>().color = m_ColorBlock.highlightedColor;
        m_WichPlayer.SetActive(false);

        OnClick.m_Score = 13;

        m_Audio = GetComponent<AudioSource>();
        m_Mute.GetComponent<Image>().sprite = m_SoundSprite;

        NbSquaresToInstantiate();
        InstantiateSquares();

        //m_WichPlayer.SetActive(false);
        m_ScoreText2.gameObject.SetActive(false);
        m_TestText.gameObject.SetActive(false);

        m_Restart.onClick.AddListener(Restart);
        m_Quit.onClick.AddListener(Quit);
        m_Mute.onClick.AddListener(Mute);
        m_Stop.onClick.AddListener(DisplayStopGameBar);
        m_Continue.onClick.AddListener(ClickOnContinue);
        m_ReturnToMenu.onClick.AddListener(ClickOnQuit);
    }

    public void Update()
    {
        if (ChooseNumberPlayer.m_HowPlayers == 2)
        {
            m_ScoreText2.gameObject.SetActive(true);
            m_ScoreText.gameObject.SetActive(true);
            m_ScoreText.text = GetName.m_PlayerOneName + " : " + OnClick.m_ScorePlayerOne;
            m_ScoreText2.text = GetName.m_PlayerTwoName + " : " + OnClick.m_ScorePlayerTwo;
        }
        else
        {
            m_ScoreText.text = "Score : " + OnClick.m_Score;
            m_TestText.text = "Essai : " + OnClick.m_Test;
        }

        if (m_SquaresObject.Count != 0)
        {
            if (OnClick.m_InClick)
            {
                TwoCardsAtTime(false);
            }
            else
            {
                TwoCardsAtTime(true);
            }
        }

        IsGameOver(OnClick.m_ScorePlayerOne + OnClick.m_ScorePlayerTwo, OnClick.m_Score);

        if(ChooseNumberPlayer.m_HowPlayers == 2)
        {
            if (OnClick.m_TurnIsOver == true)
            {
                StartCoroutine(WichPlayer());
                m_Test1++;
            }
        }
        else
        {
            m_TestText.gameObject.SetActive(true);
        }
    }


    public IEnumerator WichPlayer()
    {
        //Tout ça s'affiche une fois les cartes retourner (côté dos comme au départ)
        //ClickableImageOrNot(false);

        if (OnClick.m_InClick == true)
        {
            //Si joueur 1 (m_Test == impaire)
            if (m_Test1 % 2 != 0)
            {
                if (OnClick.m_IsTrue)
                {
                    m_Test1--;
                    OnClick.m_ScorePlayerOne++;
                    m_ScoreBoxPlayerOne.color = m_ColorBlock.highlightedColor;
                    m_ScoreBoxPlayerTwo.color = m_ColorBlock.normalColor;
                }
                else
                {
                    m_ScoreBoxPlayerTwo.color = m_ColorBlock.highlightedColor;
                    m_ScoreBoxPlayerOne.color = m_ColorBlock.normalColor;
                }
            }
            //Si joueur 2 (m_Test == paire)
            else
            {
                if (OnClick.m_IsTrue)
                {
                    m_Test1--;
                    OnClick.m_ScorePlayerTwo++;
                    m_ScoreBoxPlayerTwo.color = m_ColorBlock.highlightedColor;
                    m_ScoreBoxPlayerOne.color = m_ColorBlock.normalColor;
                }
                else
                {
                    m_ScoreBoxPlayerOne.color = m_ColorBlock.highlightedColor;
                    m_ScoreBoxPlayerTwo.color = m_ColorBlock.normalColor;
                }
            }
        }

        //Désactive l'affiche de nom du joueur suivant si la partie est terminée
        if(OnClick.m_ScorePlayerOne + OnClick.m_ScorePlayerTwo <= 13)
        {
            m_WichPlayer.SetActive(true);

            if (m_Test1 % 2 == 0)
            {
                m_WichPlayer.GetComponentInChildren<TextMeshProUGUI>().text = "C'est à " + GetName.m_PlayerOneName + " de jouer";
            }
            else
            {
                m_WichPlayer.GetComponentInChildren<TextMeshProUGUI>().text = "C'est à " + GetName.m_PlayerTwoName + " de jouer";
            }
        }
        else
        {
            m_WichPlayer.GetComponentInChildren<TextMeshProUGUI>().text = "La partie est terminée !";
            m_ScoreBoxPlayerOne.color = m_ColorBlock.normalColor;
            m_ScoreBoxPlayerTwo.color = m_ColorBlock.normalColor;
        }

        
        

        OnClick.m_TurnIsOver = false;
        yield return new WaitForSeconds(1.5f);

        //ClickableImageOrNot(true);
        //m_WichPlayer.SetActive(false);
    }

    // Pendant l'annonce du joueur qui doit jouer, les carte sont disabled (incliquable)
    public void ClickableImageOrNot(bool b)
    {
        int o = 0;
        if (m_ItsOver == false)
        {
            for (int i = 0; i < m_SquaresObject.Count; i++)
            {
                if (m_SquaresObject[i].activeInHierarchy)
                {
                    o++;
                    print(o);
                    //m_SquaresObject[i].GetComponentInChildren<Image>().raycastTarget = b;
                }
            }
        }
    }

    public void ClickOnContinue()
    {
        m_DisplayStopBar = false;
        StartCoroutine(CanvasScale(m_StopBar, new Vector3(0, 0, 0), 0.8f, false));
    }

    public void ClickOnQuit()
    {
        SceneManager.LoadScene(0);
    }

    public void NbSquaresToInstantiate()
    {
        //Add each numbers, two times in the list
        for (int i = 0; i < 2; i++)
        {
            for (int y = 0; y < m_TestSprite.Count; y++)
            {
                m_t.Add(m_TestSprite[y]);
            }
        }
    }

    public void TwoCardsAtTime(bool b)
    {
        for (int i = 0; i < m_SquaresObject.Count; i++)
        {
            m_SquaresObject[i].GetComponent<OnClick>().enabled = b;
        }
    }

    public void InstantiateSquares()
    {
        for (int i = 0; i < 28; i++)
        {
            //Square clone
            GameObject go = Instantiate(m_Square, m_Panel);
            m_SquaresObject.Add(go);

            //Random location for each numbers
            //m_Rand = Random.Range(0, m_NbToFind.Count);

             /////// IMAGE VERSION/////////
            
            // Pour jeu version image
            m_RandImg = Random.Range(0, m_t.Count);
           //Récupère l'image dans l'image (pour square image)
            var GetLastChild = go.GetComponentInChildren<Image>().transform.GetChild(0);
            GetLastChild.GetComponent<Image>().sprite = m_t[m_RandImg];

            GetLastChild.gameObject.SetActive(false);

            // Le nom de personnage deviens le nom de l'image (dernier enfant)
            go.GetComponentInChildren<Image>().transform.name = go.GetComponentInChildren<Image>().transform.GetChild(0).GetComponent<Image>().sprite.name;

            //////////////////////////

            // Pour jeu version chiffre
            //go.GetComponentInChildren<TextMeshProUGUI>().text = m_NbToFind[m_Rand];

            //Each squares text are hidden
            //go.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);

            //After the instanciation, each number is deleted form the list to avoid duplications
            m_t.RemoveAt(m_RandImg);
        }
    }

    public void IsGameOver(int twoScores, int onlyScore)
    {
        int score;

        if(ChooseNumberPlayer.m_HowPlayers == 2)
        {
            score = twoScores;
        }
        else
        {
            score = onlyScore;
        }

        if (score >= 14)
        {
            m_PlayGameOverOneTime++;
            m_GameOver.SetActive(true);
            m_ItsOver = true;

            if (ChooseNumberPlayer.m_HowPlayers == 2)
            {
                if(OnClick.m_ScorePlayerOne == OnClick.m_ScorePlayerTwo)
                {
                    m_GameOver.GetComponentInChildren<TextMeshProUGUI>().text = "Incroyable ! C'est une égalité";
                }
                else if (OnClick.m_ScorePlayerOne > OnClick.m_ScorePlayerTwo)
                {
                    m_GameOver.GetComponentInChildren<TextMeshProUGUI>().text = "Bravo " + GetName.m_PlayerOneName + " ! Tu as gagné";
                }
                else
                {
                    m_GameOver.GetComponentInChildren<TextMeshProUGUI>().text = "Bravo " + GetName.m_PlayerTwoName + " ! Tu as gagné";
                }
            }
            else
            {
                m_GameOver.GetComponentInChildren<TextMeshProUGUI>().text = "Bravo ! Tu as fini la partie";
            }

            StartCoroutine(CanvasScale(m_GameOver, new Vector3(1f, 1f, 1f), 1.5f, true));

            if (m_PlayGameOverOneTime == 1)
            {
                m_Audio.PlayOneShot(m_Win);
            }
        }
        else
        {
            m_GameOver.SetActive(false);
        }
    }

    public void DisplayStopGameBar()
    {
        if (m_AllowedScale)
        {
            m_DisplayStopBar = !m_DisplayStopBar;

            if (m_DisplayStopBar)
            {
                m_StopBar.SetActive(true);
                StartCoroutine(CanvasScale(m_StopBar, new Vector3(0.65f, 0.65f, 0.65f), 0.8f, true));
            }
            else
            {
                StartCoroutine(CanvasScale(m_StopBar, new Vector3(0,0,0), 0.8f, false));
            
            }
        }
    }

    public void Restart()
    {
        DestroyGameObject();
        IsGameOver(OnClick.m_ScorePlayerOne + OnClick.m_ScorePlayerTwo, OnClick.m_Score);
        NbSquaresToInstantiate();
        InstantiateSquares();

        OnClick.m_ScorePlayerOne = 0;
        OnClick.m_ScorePlayerTwo = 0;
        OnClick.m_Score = 0;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    public void DestroyGameObject()
    {
        for (int i = 0; i < m_SquaresObject.Count; i++)
        {
            Destroy(m_SquaresObject[i]);
        }

        m_SquaresObject.Clear(); // 0
        m_NbToFind.Clear(); // 0
        OnClick.m_CardObject.Clear(); // 0
        OnClick.m_ScorePlayerOne = 0;
        OnClick.m_Test = 0;
    }

    public void Mute()
    {
        m_Audio.mute = !m_Audio.mute;
        OnClick.m_Audio.mute = !OnClick.m_Audio.mute;

        if (m_Audio.mute)
        {
            m_Mute.GetComponent<Image>().sprite = m_MuteSprite;
        }
        else
        {
            m_Mute.GetComponent<Image>().sprite = m_SoundSprite;
        }
    }

    public IEnumerator CanvasScale(GameObject go, Vector3 v, float duration, bool b)
    {
        m_AllowedScale = false;
        Vector3 Gotoposition = v;
        Vector3 currentPos = go.transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            go.transform.localScale = Vector3.Lerp(currentPos, Gotoposition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
 
            yield return null;
        }

        m_StopBar.SetActive(b);
        m_AllowedScale = true;
    }
}
