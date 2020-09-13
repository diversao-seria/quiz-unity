using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuScreenController : MonoBehaviour
{
    public Button m_classic, m_survival, m_competition;

    public Image m_profile, m_exit;

    public Image m_exitPopup;


    public GameObject TransitionCanvas;
    private Animator animator;
    private float transitionDelay = 1f;
    public EventSystem eventSystem;


    void Awake()
    {
        m_classic.onClick.AddListener(ClassicBehaviour);
        m_survival.onClick.AddListener(SurvivalBehaviour);
        m_competition.onClick.AddListener(CompetitionBehaviour);
    }

    void Update()
    {

    }

    void ClassicBehaviour()
    {
        StartCoroutine(TransitionAnimation("Game"));
    }

    IEnumerator TransitionAnimation(string scene)
    {
        animator = TransitionCanvas.GetComponent<Animator>();
        animator.SetTrigger("TransitionTrigger");
        yield return new WaitForSeconds(transitionDelay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    void SurvivalBehaviour()
    {
        Debug.Log(" Carregue Survival");
    }

    void CompetitionBehaviour()
    {
        Debug.Log(" Carregue Competição");
    }

    public void InitialExitBehaviour()
    {
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();

        // Make anything child of canvas not interactable.
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }


        ToggleObjectsFromParent(m_exitPopup.transform, true);
    }

    public void ToggleObjectsFromParent(Transform transform, bool boolean)
    {
        transform.gameObject.SetActive(boolean);
        foreach(Transform child in transform)
        {
            ToggleObjectsFromParent(child, boolean);
        }
    }

    void ProfileBehaviour()
    {
        Debug.Log("Profile creation/edit");
    }
}