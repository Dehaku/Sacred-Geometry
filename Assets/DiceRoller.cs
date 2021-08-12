using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceRoller : MonoBehaviour
{

    public int availableDice;
    public int spellLevel;
    public int[] primesActive = new int[3];

    public List<int> dice = new List<int>();
    public TMP_Text RolledDice;
    

    int[] primes1 = new int[] { 3, 5, 7 };
    int[] primes2 = new int[] { 11, 13, 17 };
    int[] primes3 = new int[] { 19, 23, 29 };
    int[] primes4 = new int[] { 31, 37, 41 };
    int[] primes5 = new int[] { 43, 47, 53 };
    int[] primes6 = new int[] { 59, 61, 67 };
    int[] primes7 = new int[] { 71, 73, 79 };
    int[] primes8 = new int[] { 83, 89, 97 };
    int[] primes9 = new int[] { 101, 103, 107 };

    public bool sortDice = true;

    // Start is called before the first frame update
    void Start()
    {
        if (spellLevel == 1)
            primesActive = primes1;
        if (spellLevel == 2)
            primesActive = primes2;
        if (spellLevel == 3)
            primesActive = primes3;
        if (spellLevel == 4)
            primesActive = primes4;
        if (spellLevel == 5)
            primesActive = primes5;
        if (spellLevel == 6)
            primesActive = primes6;
        if (spellLevel == 7)
            primesActive = primes7;
        if (spellLevel == 8)
            primesActive = primes8;
        if (spellLevel == 9)
            primesActive = primes9;

        string rolledText = "";
        rolledText += "Spell Level: " + spellLevel + "\n";

        rolledText += "Prime Goals: ";
        for (int i = 0; i < 3; i++)
        {
            rolledText += primesActive[i];
            if (i < 3 - 1)
                rolledText += ", ";
            else
                rolledText += " ";
        }

        rolledText = rolledText + "Dice (" + availableDice + "): \n";
        for (int i = 0; i < availableDice; i++)
        {
            dice.Add(Random.Range(1, 7));
        }

        if (sortDice)
            dice.Sort();

        int iterations = 0;
        foreach (var item in dice)
        {
            iterations++;

            rolledText += item;
            if(iterations != dice.Count)
                rolledText += ", ";

        }
        
       







        RolledDice.text = rolledText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
