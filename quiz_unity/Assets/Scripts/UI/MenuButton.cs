using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void MenuButtonPress()
    {
        SceneManager.LoadScene(1);
    }

    public void ConfigButtonPress()
    {
        //Add config scene to Build
    }
}
