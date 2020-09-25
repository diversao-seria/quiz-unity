using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScreen : MonoBehaviour
{

    public GameObject resultTemplate;
    public Transform resultScrollView;

    private int numberOfQuestions;

    // Start is called before the first frame update
    void Start()
    {
        numberOfQuestions = 10;

        for(int i = 0; i < numberOfQuestions; i++)
        {
            Instantiate(resultTemplate,resultScrollView);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
