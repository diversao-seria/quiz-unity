using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    public GameObject signInPanel, loginPanel;

    // Start is called before the first frame update
    void Start()
    {
        signInPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoginButton()
    {
        //Login
    }

    public void SignInButton()
    {
        signInPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void FinishSignInButton()
    {
        //Finalizar Cadastro
        signInPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void BackButton()
    {
        signInPanel.SetActive(false);
        loginPanel.SetActive(true);

    }
}
