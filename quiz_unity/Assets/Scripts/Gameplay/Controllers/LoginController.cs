using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

public class LoginController : MonoBehaviour
{
    public GameObject signInPanel, loginPanel, popupPanel;
    public Text ErrorText;
    public InputField user, password;
    public InputField rname, remail, rcemail, rusername, rpassword, rcpassword;
    public Button popupCloseButton;
    private string jsonString;
    private UserController userController;

    // Start is called before the first frame update
    void Start()
    {
        popupPanel.SetActive(false);
        signInPanel.SetActive(false);
        loginPanel.SetActive(true);
        userController = GameObject.Find("UserController").GetComponent<UserController>();
        ErrorText.text = "";
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
        ErrorText.text = "";
        signInPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void FinishSignInButton()
    {
        //Finalizar Cadastro
        ErrorText.text = "";
        RegisterUser();
  
    }

    public void BackButton()
    {
        ErrorText.text = "";
        signInPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    private WWW RegisterPostRequest()
    {
        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(jsonString);

        www = new WWW("http://ds-quiz.herokuapp.com/players", formData, postHeader);


        StartCoroutine(WaitForRequest(www));
        return www;
    }

    IEnumerator WaitForRequest(WWW data)
    {
        while (!data.isDone)        // Wait until the download is done
        {
            yield return null;
        }
        yield return data; 
        if (data.error != null)
        {
            popupCloseButton.interactable = true;
            Debug.Log("Erro: " + data.error);
            if (data.error == "422 Unprocessable Entity")
            {
                var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(data.text);
                ErrorText.text = "";
                foreach (KeyValuePair<string, List<string>> kvp in jsonObj)
                {
                    Debug.Log(kvp.Key + " " + kvp.Value[0]);
                    ErrorText.text += kvp.Key + " " + kvp.Value[0] + "\n";
                    if(kvp.Key == "username")
                    {
                        rusername.text = "";
                    }
                    if (kvp.Key == "email")
                    {
                        remail.text = "";
                        rcemail.text = "";
                    }
                }
                //Debug.Log("O email informado " + jsonObj["username"]);
               // Debug.Log("O nome de usuário informado " + jsonObj.username[0]);
            }
        }
        else
        {
            Debug.Log("Sucesso! " + data.text);
            RegisterSuccess();
        }
    }

    private void RegisterSuccess()
    {
        userController.playername = rname.text.ToString();
        userController.email = remail.text.ToString();
        userController.username = rusername.text.ToString();

        rname.text = "";
        remail.text = "";
        rcemail.text = "";
        rusername.text = "";
        rpassword.text = "";
        rcpassword.text = "";

        signInPanel.SetActive(false);
        loginPanel.SetActive(true);
        PopupOpen();
        popupCloseButton.interactable = true;
        ErrorText.text = "Cadastro efetuado com sucesso!";
    }

    private void LoginSuccess()
    {
        userController.loggedIn = true;
        PopupOpen();
        popupCloseButton.interactable = true;
        ErrorText.text = "Login efetuado com sucesso!";
        Debug.Log("Login Successesful");
    }

    private void RegisterUser()
    {
        string vname = rname.text.ToString();
        string vemail = remail.text.ToString();
        string vcemail = rcemail.text.ToString();
        string vusername = rusername.text.ToString();
        string vpass = rpassword.text.ToString();
        string vcpass = rcpassword.text.ToString();

        //Show error
        //Clear input field with error

        if (vname == "" || vname.Length > 50)
        {
            PopupOpen();
            ErrorText.text = "Erro: Nome inválido. Seu nome não deve conter mais de 50 caracteres.";
            rname.text = "";
            Debug.Log("Nome inválido. Seu nome não deve conter mais de 50 caracteres.");
        }
        else if (vemail == "" || !checkEmail(vemail))
        {
            PopupOpen();
            ErrorText.text = "Erro: Email inválido.";
            remail.text = "";
            Debug.Log("Email inválido.");
        }
        else if (vemail != vcemail)
        {
            PopupOpen();
            ErrorText.text = "Erro: Os emails informados são diferentes.";
            Debug.Log("Os emails informados são diferentes.");
        }
        else if (vusername.Length < 4 || vusername.Length > 20)
        {
            PopupOpen();
            ErrorText.text = "Erro: Nome de usuário inválido. Seu nome de usuário deve conter entre 4 e 20 caracteres.";
            rusername.text = "";
            Debug.Log("Nome de usuário inválido. Seu nome de usuário deve conter entre 4 e 20 caracteres.");
        }
        else if (vpass.Length < 6)
        {
            PopupOpen();
            ErrorText.text = "Erro: Senha inválida. Sua senha deve ter no mínimo 6 caracteres.";
            rpassword.text = "";
            rcpassword.text = "";
            Debug.Log("Senha inválida. Sua senha deve ter no mínimo 6 caracteres.");
        }
        else if (vpass != vcpass)
        {
            PopupOpen();
            ErrorText.text = "Erro: As senhas informadas são diferentes.";
            rcpassword.text = "";
            rpassword.text = "";
            Debug.Log("As senhas informadas são diferentes.");
        }
        else
        {
            Data data = new Data();
            data.name = vname;
            data.email = vemail;
            data.username = vusername;
            data.password = PasswordHash.HashPass(vpass);
            // Debug.Log(data.password);
            Player playerObj = new Player();
            playerObj.player = data;

            jsonString = JsonUtility.ToJson(playerObj);

            PopupOpen();
            ErrorText.text = "Aguarde...";
            popupCloseButton.interactable = false;

            RegisterPostRequest();
        }
    }

    private bool checkEmail(string email)
    {
        try
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    [System.Serializable]
    public class Player
    {
        public Data player;
    }

    [System.Serializable]
    public class Data
    {
        public string name;
        public string email;
        public string username;
        public string password;
    }
    [System.Serializable]
    public class serverResponse
    {
        public List<string> username;
        public List<string> email;

        public static serverResponse CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<serverResponse>(jsonString);
        }
    }


    //Username can be Usuário or E-mail
    IEnumerator LoginUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", username);
        form.AddField("password", password);
        PopupOpen();
        ErrorText.text = "Aguarde...";
        popupCloseButton.interactable = false;

        string url = "http://ds-quiz.herokuapp.com/authenticate";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                //Debug.Log(www.error);
                popupCloseButton.interactable = true;
                Debug.Log(www.downloadHandler.text);
                var value = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(www.downloadHandler.text);
                foreach (var resp in value)
                {
                    Debug.Log(resp.Value["player_authentication"]);

                    if (resp.Value["player_authentication"] ==  "invalid credentials")
                    {

                        ErrorText.text = "Credenciais inválidas";
                    }
                }
            }
            else
            {
                ///Debug.Log(JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text));
                var value = JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text);
                foreach (KeyValuePair<string, string> resp in value)
                {
                    Debug.Log(resp.Key + " " + resp.Value);
                    if (resp.Key == "auth_token")
                    {
                        userController.auth_token = resp.Value;
                    }
                    else if(resp.Key == "player_id")
                    {
                        userController.player_id = resp.Value;
                    }
                }
                if (checkEmail(username))
                {
                    userController.email = username;
                }
                else
                {
                    userController.username = username;
                }
                LoginSuccess();
            }
        }
    }

    public void CloseSceneButton()
    {
        userController.isGuest = true;
        SceneManager.UnloadSceneAsync("Login");
    }

    public void PopupCloseButton()
    {
        popupPanel.SetActive(false);
    }

    private void PopupOpen()
    {
        ErrorText.text = "";
        popupPanel.SetActive(true);
    }
}
