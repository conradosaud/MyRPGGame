using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Skill
{
    
    public string name { get; set; }
    public int damage { get; set; }
    public float countdown { get; set; }
    public float countdownElapsed { get; set; }
    public int range { get; set; }

    public bool basicAttack { get; set; }

    public Skill(string name, int damage,  float countdown, int range)
    {
        this.name = name;
        this.damage = damage;
        this.countdown = countdown;
        this.countdownElapsed = 0;
        this.range = range;
    }

}
