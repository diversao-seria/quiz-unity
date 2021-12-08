using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InGameInterruptChecker : MonoBehaviour
{
    public GameController gameController;

    public void InGameInterruptSignal(GameMechanicsConstant.InterruptTypes interruptType)
    {

        string playerDataFolder = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataPath;

        try
        {
            if (!Directory.Exists(playerDataFolder))
            {
                System.IO.Directory.CreateDirectory(Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataFolder);
            }
        }
        catch (IOException e)
        {
            Debug.Log(e + ". Caminho é arquivou ou armazenamento cheio.");
        }

        gameController.GetComponent<InterruptSubController>().RegisterInterrupt(interruptType, false);
        GetComponent<PopupHandler>().ReturnToMainMenu();
    }
}
