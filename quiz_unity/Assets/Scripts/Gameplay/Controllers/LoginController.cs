using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class LoginController : MonoBehaviour
{
    public GameObject signInPanel, loginPanel;
    public Text text;
    public InputField user, password;
    // Start is called before the first frame update
    void Start()
    {
        signInPanel.SetActive(false);
        loginPanel.SetActive(true);

        //StartCoroutine(LoginUser("teste", "12345"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoginButton()
    {
        //Login
        StartCoroutine(LoginUser(user.text, password.text));
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

    IEnumerator LoginUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", username);
        form.AddField("password", password);

        string url = "http://ds-quiz.herokuapp.com/authenticate";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                //Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
                var value = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(www.downloadHandler.text);
                foreach (var resp in value)
                {
                    Debug.Log(resp.Value["player_authentication"]);
                    text.text = resp.Value["player_authentication"];
                }
            }
            else
            {
                ///Debug.Log(JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text));
                var value = JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text);
                foreach (KeyValuePair<string, string> resp in value)
                {
                    Debug.Log(resp.Key + " " + resp.Value);
                    text.text += resp.Key + " " + resp.Value + "\n";
                }
                Debug.Log(www.downloadHandler.text +  "\n success");
            }
        }
    }


}
