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

    // Funções:
    // 1) Requisição do quiz.
    //  1.1) inicia-se download
    //  1.2) falha 
    //      1.2.1 Tentar de novo
    //      1.2.2 desistir -> voltar
    
}
