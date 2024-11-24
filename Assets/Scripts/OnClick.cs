using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class OnClick : MonoBehaviour, IPointerClickHandler
{

    //NUMERO S'EFFACE TROP VITE AU RETOURNEMENT DE LA CARTE

    public static string m_FirstCard;
    public static string m_SecondCard;

    public static int m_NbClick = 0;
    public static int m_ScorePlayerOne;
    public static int m_ScorePlayerTwo;
    public static int m_Test;


    public static int m_Score;

    public static bool m_IsTrue = false;
    public static bool m_InClick = false;
    public static bool m_TurnIsOver = false;
    public static bool m_testBool = false;

    public ColorBlock m_SquareColor;

    public static AudioSource m_Audio;
    public AudioClip m_WinDing;
    public AudioClip m_WrongDing;
    public AudioClip m_TestSound;

    public Sprite m_FlowerCard;

    public static List<GameObject> m_CardObject = new List<GameObject>();
    GameObject go;

    public void Start()
    {
        m_Audio = GetComponent<AudioSource>();

        //m_ScorePlayerOne = 6;
        //m_ScorePlayerTwo = 7;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        m_TurnIsOver = false;
        //Prevent the text part from rotating
        if (!eventData.pointerCurrentRaycast.gameObject.ToString().Contains("Text"))
        {
            go = eventData.pointerCurrentRaycast.gameObject;
            m_NbClick++;
            StartCoroutine(RotateCard(0.7f, 0, 180, go));

            try
            {
                if (m_NbClick == 1)
                {
                    StartCoroutine(DisplayNumber(go));
                    //m_FirstCard = go.transform.GetChild(0).name;
                    m_FirstCard = go.transform.name;
                   // print(m_FirstCard);
                    m_CardObject.Add(go);
                    EnableFirstCardForStoppedError(m_NbClick);
                    m_Audio.PlayOneShot(m_TestSound);
                    StartCoroutine(ChangeFaceColor(0));
                    return;
                }
                else if (m_NbClick == 2)
                {
                    StartCoroutine(DisplayNumber(go));
                    m_SecondCard = go.transform.name;
                    m_CardObject.Add(go);
                    EnableFirstCardForStoppedError(m_NbClick);
                    m_Audio.PlayOneShot(m_TestSound);
                    StartCoroutine(ChangeFaceColor(1));
                    m_InClick = true;
                }

                if (m_FirstCard == m_SecondCard)
                {
                    StartCoroutine(SetColor(m_SquareColor.pressedColor));
                    StartCoroutine(ReturnCard());
                    m_NbClick = 0;
                    m_IsTrue = true;
                }
                else
                {
                    StartCoroutine(SetColor(m_SquareColor.selectedColor));
                    StartCoroutine(ReturnCard());
                    m_NbClick = 0;
                }
            }
            catch
            {
                print("erreur");
            }
        }
    }


    //Affiche couleur bleu (sans la fleur)
    public IEnumerator ChangeFaceColor(int i)
    {
        yield return new WaitForSeconds(0.4f);
        m_CardObject[i].transform.GetComponent<Image>().color = m_SquareColor.disabledColor;
        m_CardObject[i].transform.GetComponent<Image>().sprite = null;
    }

    public void EnableFirstCardForStoppedError(int nbClick)
    {
        if (nbClick == 1)
        {
            m_CardObject[0].GetComponent<Image>().raycastTarget = false;
        }
        else 
        {
            m_CardObject[0].GetComponent<Image>().raycastTarget = true;
        }
    }

    public void PlayWinSound()
    {
        if (m_IsTrue)
        {
            m_Audio.PlayOneShot(m_WinDing);
        }
        else
        {
            m_Audio.PlayOneShot(m_WrongDing);
        }
    }

    public IEnumerator SetColor(Color color)
    {
        yield return new WaitForSeconds(1f);
        PlayWinSound();
        m_Test++;

        //Affiche la couleur (rouge ou vert)
        for (int i = 0; i < m_CardObject.Count; i++)
        {
            m_CardObject[i].transform.GetComponent<Image>().color = color;
        }
    }

    public IEnumerator DisplayNumber(GameObject go)
    {
        yield return new WaitForSeconds(0.4f);
        go.transform.GetChild(0).gameObject.SetActive(true);
    }

    public IEnumerator ReturnCard()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < m_CardObject.Count; i++)
        {
            StartCoroutine(RotateCard(0.7f, 180, 0, m_CardObject[i]));

            if (!m_IsTrue)
            {
                m_CardObject[i].transform.GetComponent<Image>().color = m_SquareColor.disabledColor;
               
            }
        }
        //Ne surtout pas déplacer (permet de savoir quand le joueur a eu juste (en version 2 joueurs).

        m_TurnIsOver = true;

        yield return new WaitForSeconds(0.3f);
        SetFlowerWhenTheyAreWrong();

        if (m_IsTrue)
        {
            CardIsFind();
        }
        m_CardObject.Clear();
        m_InClick = false;
    }

    public void SetFlowerWhenTheyAreWrong()
    {
        //Remet la fleur lorsque la carte se retourne
        for (int y = 0; y < m_CardObject.Count; y++)
        {
            m_CardObject[y].transform.GetComponent<Image>().color = m_SquareColor.normalColor;
            m_CardObject[y].transform.GetComponent<Image>().sprite = m_FlowerCard;
            m_CardObject[y].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void CardIsFind()
    {
        for (int i = 0; i < m_CardObject.Count; i++)
        {
            m_CardObject[i].SetActive(false);
        }
        m_CardObject.Clear();
        m_IsTrue = false;
        m_Score++;
    }

    public IEnumerator RotateCard(float duration, float startRotation, float endRotation, GameObject go)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration);
            go.transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation,
            go.transform.eulerAngles.z);

            yield return null;
        }
    }
}
