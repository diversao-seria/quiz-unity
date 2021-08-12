using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject balloon1, balloon2, balloon3, balloon4, balloon5, balloon6, nextButton, previousButton;
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

        //Coloca as fun��es na lista tutorial
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
        tutorial.Add(B13);

        //Inicia o tutorial
        tutorial[0]();
    }


    //Vai para o pr�ximo estado
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

        //Desativa todos os bal�es e deixa apenas o 1
        balloon1.SetActive(true);
        balloon2.SetActive(false);
        balloon3.SetActive(false);
        balloon4.SetActive(false);
        balloon5.SetActive(false);
        balloon6.SetActive(false);

        //Desabilita o bot�o de voltar
        previousButton.SetActive(false);

        //Desfaz o highlight do estado 2
        timer.SetParent(PanelSafeArea, true);

        //Altera o texto
        Text text = balloon1.GetComponentInChildren<Text>();
        text.text = "Ol�, vamos iniciar o tutorial.\nVoc� pode pular ele a qualquer momento, caso deseje...\nClique nas setas para prosseguir.";
    }

    private void B2()
    {
        //Estado 2 - Timer 1
        
        //Desfaz as ativa��es do anterior e pr�ximo estado
        balloon1.SetActive(false);
        balloon3.SetActive(false);

        //Habilita o bal�o a ser utilizado, o bot�o de voltar e d� destaque ao timer
        balloon2.SetActive(true);
        previousButton.SetActive(true);
        timer.SetParent(PanelBlurr, true);

        //Muda a sprite do bal�o 2
        balloon2.GetComponent<Image>().sprite = b1;

        //Altera o texto
        Text text = balloon2.GetComponentInChildren<Text>();
        text.text = "Aqui voc� pode consultar o tempo restante para responder a quest�o.";
    }

    private void B3()
    {
        //Estado 3 - Timer 2

        //Desfaz o pr�ximo estado
        questionNumber.SetParent(PanelSafeArea, true);

        //Habilita os objetos
        timer.SetParent(PanelBlurr, true);

        //Muda a sprite
        balloon2.GetComponent<Image>().sprite = b1;

        //Altera o texto
        Text text = balloon2.GetComponentInChildren<Text>();
        text.text = "O tempo n�o contribu� para a sua pontua��o final! Ou seja, responder uma quest�o mais r�pido n�o aumentar� a sua pontua��o.";
    }

    private void B4()
    {
        //Estado 4 - Quest�es
        timer.SetParent(PanelSafeArea, true);
        questionNumber.SetParent(PanelBlurr, true);

        exit.SetParent(PanelSafeArea, true);

        balloon2.GetComponent<Image>().sprite = b3;

        Text text = balloon2.GetComponentInChildren<Text>(); 
        text.text = "Aqui voc� pode consultar a quantidade de quest�es que o quiz possu� e qual delas voc� est� respondendo no momento.";
    }

    private void B5()
    {
        //Estado 5 - Sa�da 1
        questionNumber.SetParent(PanelSafeArea, true);

        exit.SetParent(PanelBlurr, true);

        balloon2.GetComponent<Image>().sprite = b2;

        Text text = balloon2.GetComponentInChildren<Text>();
        text.text = "Voc� pode clicar aqui a qualquer momento para sair do quiz.";
    }

    private void B6()    {
        //Estado 6 - Sa�da 2
        question.SetParent(PanelSafeArea, true);        
        balloon4.SetActive(false);

        balloon2.SetActive(true);
        exit.SetParent(PanelBlurr, true);

        balloon2.GetComponent<Image>().sprite = b2;

        Text text = balloon2.GetComponentInChildren<Text>();
        text.text = "Vale lembrar que as quest�es respondidas ser�o salvas e registradas na sua pontua��o final ainda assim.";
    }

    private void B7()
    {
        //Estado 7 - Quest�o
        exit.SetParent(PanelSafeArea, true);
        answerButtonParent.SetParent(PanelSafeArea, true);

        question.SetParent(PanelBlurr, true);

        balloon2.SetActive(false);
        balloon5.SetActive(false);

        //O bal�o 4 s� cont�m um tipo de texto, que est� no objeto
        balloon4.SetActive(true);
    }

    private void B8()
    {
        //Estado 8 - Alternativas
        question.SetParent(PanelSafeArea, true);

        answerButtonParent.SetParent(PanelBlurr, true);

        balloon3.SetActive(false);
        balloon4.SetActive(false);

        //O bal�o 5 tamb�m s� contem um texto
        balloon5.SetActive(true);
    }

    private void B9()
    {
        //Estado 9 - Poderes Geral
        answerButtonParent.SetParent(PanelSafeArea, true);
       
        balloon5.SetActive(false);
        balloon6.SetActive(false);

        //O bal�o 3 s� cont�m um texto e imagens
        balloon3.SetActive(true);
    }

    private void B10()
    {
        //Estado 10 - Poderes Vento
        balloon3.SetActive(false);

        balloon6.SetActive(true);

        //Muda a sprite e o texto do bal�o 6
        balloon6.GetComponentInChildren<Image>().sprite = wind;
        Text text = balloon6.GetComponentInChildren<Text>();
        text.text = "Ao usar este poder, duas alternativas incorretas s�o eliminadas.";
    }

    private void B11()
    {
        //Estado 11 - Poderes Folha
        balloon6.SetActive(true);

        //Muda a sprite e o texto do bal�o 6
        balloon6.GetComponentInChildren<Image>().sprite = leaf;
        Text text = balloon6.GetComponentInChildren<Text>();
        text.text = "Usando esse poder voc� ganha uma segunda chance caso erre a alternativa nesta quest�o";
    }

    private void B12()
    {
        //Estado 12 - Poderes Gelo
        balloon6.SetActive(true);

        //Muda a sprite e o texto do bal�o 6
        balloon6.GetComponentInChildren<Image>().sprite = ice;
        Text text = balloon6.GetComponentInChildren<Text>();
        text.text = "Ap�s usar esse poder, o tempo da quest�o ir� congelar por alguns segundos.";
    }

    private void B13()
    {
        //Estado 13 - Final
        balloon6.SetActive(false);

        balloon1.SetActive(true);

        Text text = balloon1.GetComponentInChildren<Text>();
        text.text = "Finalmente, terminamos o tutorial."; 
        //"Prossiga para uma partida de teste"?
    }

}
