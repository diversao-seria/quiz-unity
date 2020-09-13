using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    private string name;
    private int id;

    public string Name { get; set; }
    public int ID { get; set; }

    public Player(string name, int id)
    {
        this.Name = name;
        this.ID = id;
    }


}
