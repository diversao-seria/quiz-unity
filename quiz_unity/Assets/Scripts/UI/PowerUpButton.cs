using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    private GameObject gameController;
    private PowerUpController powerUpController;

    public GameMechanicsConstant.PowerUpNames powerUp;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController");
        powerUpController = gameController.GetComponent<PowerUpController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleClick()
    {
        powerUpController.UsePowerUp(powerUp);
        powerUpController.RemovePowerUp(this.gameObject);
    }
}
