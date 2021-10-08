using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

public class LoginController : MonoBehaviour
{
    public GameObject signInPanel, loginPanel;
    public Text text;
    public InputField user, password;
    public InputField rname, remail, rcemail, rusername, rpassword, rcpassword;
    private string jsonString;
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
        RegisterUser();
        signInPanel.SetActive(false);
        loginPanel.SetActive(true);     
    }

    public void BackButton()
    {
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
            Debug.Log("Erro: " + data.error);
            if (data.error == "422 Unprocessable Entity")
            {
                var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(data.text);
                foreach (KeyValuePair<string, List<string>> kvp in jsonObj)
                {
                    Debug.Log(kvp.Key + " " + kvp.Value[0]);
                }
                //Debug.Log("O email informado " + jsonObj["username"]);
               // Debug.Log("O nome de usuário informado " + jsonObj.username[0]);
            }
        }
        else
        {
            Debug.Log("Sucesso! " + data.text);
        }
    }

    private void RegisterUser()
    {
        string vname = rname.text.ToString();
        string vemail = remail.text.ToString();
        string vcemail = rcemail.text.ToString();
        string vusername = rusername.text.ToString();
        string vpass = rpassword.text.ToString();
        string vcpass = rcpassword.text.ToString();


        if (vname == "" || vname.Length > 50)
        {
            Debug.Log("Nome inválido. Seu nome não deve conter mais de 50 caracteres.");
        }
        else if (vemail == "" || !checkEmail(vemail))
        {
            Debug.Log("Email inválido.");
        }
        else if (vemail != vcemail)
        {
            Debug.Log("Os emails informados são diferentes.");
        }
        else if (vusername.Length < 4 || vusername.Length > 20)
        {
            Debug.Log("Nome de usuário inválido. Seu nome de usuário deve conter entre 4 e 20 caracteres.");
        }
        else if (vpass.Length < 6)
        {
            Debug.Log("Senha inválida. Sua senha deve ter no mínimo 6 caracteres.");
        }
        else if (vpass != vcpass)
        {
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
