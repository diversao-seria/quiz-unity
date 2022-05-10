using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public static UserController Instance { get; private set; }

    [HideInInspector] public bool loggedIn; //Verify is the user is Logged In
    [HideInInspector] public bool isGuest; // Verify if the player is playing on guest mode 
    [HideInInspector] public string username;
    [HideInInspector] public string playername;
    [HideInInspector] public string email;
    [HideInInspector] public string auth_token;
    [HideInInspector] public string player_id;

    [HideInInspector] public string history;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }


    //PlayerPrefs.Set isn't implemented
    public void LoadUserData()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            username = PlayerPrefs.GetString("username");
        }

        if (PlayerPrefs.HasKey("playername"))
        {
            playername = PlayerPrefs.GetString("playername");
        }

        if (PlayerPrefs.HasKey("email"))
        {
            email = PlayerPrefs.GetString("email");
        }

        if (PlayerPrefs.HasKey("auth_token"))
        {
            auth_token = PlayerPrefs.GetString("auth_token");
        }

        if (PlayerPrefs.HasKey("player_id"))
        {
            player_id = PlayerPrefs.GetString("player_id");
        }

        if (PlayerPrefs.HasKey("history"))
        {
            history = PlayerPrefs.GetString("history");
        }

    }
}
