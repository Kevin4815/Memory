using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using System;

public class GetName : MonoBehaviour
{
    public InputField m_PlayerOne;
    public InputField m_PlayerTwo;

    public GameObject m_AlertMessage;

    public static string m_PlayerOneName;
    public static string m_PlayerTwoName;

    bool m_FirstPlayerName;
    bool m_SecondPlayerName;

    public Button m_Play;


    public void Start()
    {
        m_Play.onClick.AddListener(GetNames);
        m_AlertMessage.SetActive(false);
    }

    public void GetNames()
    {
        if (m_PlayerOne.text != string.Empty && m_PlayerTwo.text != string.Empty)
        {
            string getNameOne = m_PlayerOne.text.Replace(" ", "");
            string getNameTwo = m_PlayerTwo.text.Replace(" ", "");

            m_PlayerOneName = CaptitalizeFirstLetter(getNameOne);
            m_PlayerTwoName = CaptitalizeFirstLetter(getNameTwo);

            m_FirstPlayerName = IsAllLetters(m_PlayerOneName);
            m_SecondPlayerName = IsAllLetters(m_PlayerTwoName);

            if(m_FirstPlayerName == true && m_SecondPlayerName == true)
            {
                if (m_PlayerOneName.Length <= 9 && m_PlayerTwoName.Length <= 9)
                {
                    SceneManager.LoadScene(1);
                }
                else
                {
                    m_AlertMessage.GetComponentInChildren<TextMeshProUGUI>().text = "Nom de 9 lettres maximum";
                    StartCoroutine(DisplayAlert());
                }
            }
            else
            {
                m_AlertMessage.GetComponentInChildren<TextMeshProUGUI>().text = "Caractères spéciaux interdits";
                StartCoroutine(DisplayAlert());
            }
        }
        else
        {
            m_AlertMessage.GetComponentInChildren<TextMeshProUGUI>().text = "Veuillez inscrire des noms";
            StartCoroutine(DisplayAlert());
        }
    }

    public string CaptitalizeFirstLetter(string name)
    {
        return name[0].ToString().ToUpper() + name.Substring(1, name.Length-1).ToLower();

    }

    public IEnumerator DisplayAlert()
    {
        m_AlertMessage.SetActive(true);

        yield return new WaitForSeconds(2f);

        m_AlertMessage.SetActive(false);
    }

    public static bool IsAllLetters(string s)
    {
        foreach (char c in s)
        {
            if (!Char.IsLetter(c))
                return false;
        }
        return true;
    }

}
