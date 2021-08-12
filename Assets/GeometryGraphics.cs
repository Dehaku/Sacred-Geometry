using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


class NumText
{
    public int number;
    public TMP_Text text;
}

public class GeometryGraphics : MonoBehaviour
{
    public Solver solver;
    public DiceRoller diceRoller;

    public float globalSpin = 0;
    public float globalSpinHalf = 0;

    public Vector3 centerPointOfSpin;
    public float spinDistance;

    public GameObject spinnyObject1;
    public GameObject spinnyObject2;
    public GameObject spinnyObject3;
    public float spinSpeed;
    public float variance = 0;

    public GameObject geoTextParent;
    List<TextMeshProUGUI> geoTextList = new List<TextMeshProUGUI>();
    public GameObject textPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setupDice());

    }

    void AddGeoText(string entry)
    {
        GameObject scrollItem = Instantiate(textPrefab);
        scrollItem.GetComponent<TextMeshProUGUI>().text = entry;
        scrollItem.transform.SetParent(geoTextParent.transform, false);
        geoTextList.Add(scrollItem.GetComponent<TextMeshProUGUI>());
    }

    IEnumerator setupDice()
    {
        yield return new WaitForSeconds(1);

        foreach (var item in diceRoller.dice)
        {
            AddGeoText(item.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
       


        globalSpin += Time.deltaTime * spinSpeed;
        if (globalSpin > 360)
            globalSpin -= 360;
        globalSpinHalf += Time.deltaTime * (spinSpeed / 2);
        if (globalSpinHalf > 360)
            globalSpinHalf -= 360;

        float circleSlotOffset = 6.28f / geoTextList.Count;

        float increment = 0;
        foreach (var item in geoTextList)
        {
            Vector3 position = centerPointOfSpin;
            Quaternion rot = Quaternion.identity;

            position.x = centerPointOfSpin.x + (Mathf.Sin(globalSpin + (circleSlotOffset * increment)) * spinDistance);
            position.y = centerPointOfSpin.y + (Mathf.Cos(globalSpin + (circleSlotOffset * increment)) * spinDistance);

            item.transform.SetPositionAndRotation(position, rot);

            increment++;
        }
    }
}
