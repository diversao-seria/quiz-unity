using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public GameObject balloon1, balloon2, balloon3, balloon4, balloon5, balloon6, nextButton, previousButton, skipButton;
    public Sprite b1, b2, b3, leaf, wind, ice;

    public Transform timer, answerButtonParent, question, exit, questionNumber;
    public Transform PanelBlurr, PanelSafeArea, PanelInteractive;

    private int currentState;

    delegate void TutorialMethod();
    List<TutorialMethod> tutorial = new List<TutorialMethod>();

    // Start is called before the first frame update
    void Start()
    {
        currentState = 0;

        //Coloca as funções na lista tutorial
        tutorial.Add(B1);
        tutorial.Add(B2);
        tutorial.Add(B3);
        tutorial.Add(B4);
        tutorial.Add(B5);
        tutorial.Add(B6);
        tutorial.Add(B7);
        tutorial.Add(B8);
        tutorial.Add(B9);
        tutorial.Add(B10);
        tutorial.Add(B11);
        tutorial.Add(B12);

        //Inicia o tutorial
        tutorial[0]();
    }


    //Vai para o próximo estado
    public void NextState()
    {
        if (currentState < tutorial.Count - 1)
        {
            currentState++;
            tutorial[currentState]();
        }
    }

    //Vai para o estado anterior
    public void PreviousState()
    {
        if (currentState > 0)
        {
            currentState--;
            tutorial[currentState]();
        }
    }

    private void B1()
    {
        //Estado 1 - Intro

        //Desativa todos os balões e deixa apenas o 1
        balloon1.SetActive(true);
        balloon2.SetActive(false);
        balloon3.SetActive(false);
        balloon4.SetActive(false);
        balloon5.SetActive(false);
        balloon6.SetActive(false);

        //Desabilita o botão de voltar
        previousButton.SetActive(false);

        //Desfaz o highlight do estado 2
        timer.SetParent(PanelSafeArea, true);

        //Altera o texto
        Text text = balloon1.GetComponentInChildren<Text>();
        text.text = "Bem-Vindo ao Quizle! Clique nas setas para aprender como se joga.";
    }


    private void B2()
    {
        //Estado 2 - Timer 
        
        //Desfaz as ativações do anterior e próximo estado
        balloon1.SetActive(false);
        balloon3.SetActive(false);
        questionNumber.SetParent(PanelSafeArea, true);

        //Habilita o balão a ser utilizado, o botão de voltar e dá destaque ao timer
        balloon2.SetActive(true);
        previousButton.SetActive(true);
        timer.SetParent(PanelBlurr, true);

        //Muda a sprite do balão 2
        balloon2.GetComponent<Image>().sprite = b1;

        //Altera o texto
        Text text = balloon2.GetComponentInChildren<Text>();
        text.text = "O tempo restante para responder a questão aparece aqui.";
    }

    private void B3()
    {
        //Estado 3 - Questões

        //Desfaz o próximo estado e o anterior
        timer.SetParent(PanelSafeArea, true);
        exit.SetParent(PanelSafeArea, true);

        //Habilita os objetos
        questionNumber.SetParent(PanelBlurr, true);

        //Muda a sprite
        balloon2.GetComponent<Image>().sprite = b3;

        //Altera o texto
        Text text = balloon2.GetComponentInChildren<Text>();
        text.text = "O número da questão e o total aparecem aqui.";
    }

    private void B4()
    {
        //Estado 4 - Saída
        questionNumber.SetParent(PanelSafeArea, true);
        question.SetParent(PanelSafeArea, true);

        exit.SetParent(PanelBlurr, true);

        balloon4.SetActive(false);
        balloon2.SetActive(true);
        balloon2.GetComponent<Image>().sprite = b2;

        Text text = balloon2.GetComponentInChildren<Text>();
        text.text = "Clique aqui para finalizar o quiz e retornar ao menu principal do jogo.";
    }

    private void B5()
    {
        //Estado 5 - Questão
        exit.SetParent(PanelSafeArea, true);
        answerButtonParent.SetParent(PanelSafeArea, true);

        question.SetParent(PanelBlurr, true);

        balloon2.SetActive(false);
        balloon5.SetActive(false);

        //O balão 4 só contém um tipo de texto, que está no objeto
        balloon4.SetActive(true);
    }

    private void B6()    {
        //Estado 6 - Alternativas
        question.SetParent(PanelSafeArea, true);

        answerButtonParent.SetParent(PanelBlurr, true);

        balloon3.SetActive(false);
        balloon4.SetActive(false);

        //O balão 5 também só contem um texto
        balloon5.SetActive(true);
    }

    private void B7()
    {
        //Estado 7 - Poderes Geral
        answerButtonParent.SetParent(PanelSafeArea, true);

        balloon5.SetActive(false);
        balloon6.SetActive(false);

        //O balão 3 só contém um texto e imagens
        balloon3.SetActive(true);
    }

    private void B8()
    {
        //Estado 8 - Poderes Vento
        balloon3.SetActive(false);

        balloon6.SetActive(true);

        //Muda a sprite e o texto do balão 6
        balloon6.transform.GetChild(1).GetComponent<Image>().sprite = wind;
        Text text = balloon6.GetComponentInChildren<Text>();
        text.text = "Eliminar duas alternativas incorretas.";
    }

    private void B9()
    {
        //Estado 9 - Poderes Folha
        balloon6.SetActive(true);

        //Muda a sprite e o texto do balão 6
        balloon6.transform.GetChild(1).GetComponent<Image>().sprite = leaf;
        Text text = balloon6.GetComponentInChildren<Text>();
        text.text = "Responder novamente caso erre. Lembre-se: este poder deve ser ativado antes de responder a questão.";
    }

    private void B10()
    {
        //Estado 10 - Poderes Gelo
        balloon6.SetActive(true);
        balloon1.SetActive(false);

        //Muda a sprite e o texto do balão 6
        balloon6.transform.GetChild(1).GetComponent<Image>().sprite = ice;
        Text text = balloon6.GetComponentInChildren<Text>();
        text.text = "Após usar esse poder, o tempo da questão irá congelar por alguns segundos.";
    }

    private void B11()
    {
        //Estado 11 - Poder fogo
        balloon6.SetActive(false);

        balloon1.SetActive(true);
        nextButton.SetActive(true);
        skipButton.transform.GetChild(0).GetComponent<Text>().text = "Pular Tutorial";

        Text text = balloon1.GetComponentInChildren<Text>();
        text.text = "Se você fizer uma sequência correta de três acertos, os poderes usados serão restaurados.";
    }

    private void B12()
    {
        //Estado 12 - Final
        nextButton.SetActive(false);
        skipButton.transform.GetChild(0).GetComponent<Text>().text = "Começar";

        balloon1.SetActive(true);

        Text text = balloon1.GetComponentInChildren<Text>();
        text.text = "Está preparado? Prossiga nessa aventura do conhecimento. Bom jogo!";
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

}
