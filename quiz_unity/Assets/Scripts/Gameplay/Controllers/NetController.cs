using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TO DO:
    public bool RequestQuiz(string quizCode)
    {

        // Funções:
        // 1) Realiza a requisição
        // 1.1) inicia-se download (sem erros) -> true.
        // 1.2) falha 
        //      1.2.1 Tentar de novo (até um certo limite de tempo).
        //      1.2.2 desistir -> voltar

        return true;
    }
}
