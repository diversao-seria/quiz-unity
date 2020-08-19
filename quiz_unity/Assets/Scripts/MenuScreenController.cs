using UnityEngine;
using UnityEngine.UI;

public class MenuScreenController : MonoBehaviour
{
    public Button m_classic, m_survival, m_competition;

    void Start()
    {
        m_classic.onClick.AddListener(ClassicBehaviour);
        m_survival.onClick.AddListener(SurvivalBehaviour);
        m_competition.onClick.AddListener(CompetitionBehaviour);
    }

    void ClassicBehaviour()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    void SurvivalBehaviour()
    {
        Debug.Log(" Carregue Survival");
    }

    void CompetitionBehaviour()
    {
        Debug.Log(" Carregue Competição");
    }

}