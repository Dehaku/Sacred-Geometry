using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


class Cell
{
    public double x;
    public double y;
    public string operation;
    public bool mono; // Only using x variable.
    public bool fromMerged;

    public double Value()
    {
        if (mono)
            return x;
            
        if(operation == "+")
            return x + y;
        else if (operation == "-")
            return x - y;
        else if (operation == "*")
            return x * y;
        else if (operation == "/")
            return x / y;
        return 0;
    }

    public string Stringify()
    {
        if (mono)
            return x.ToString();

        return "(" + x + operation + y + ")";
    }

    public string StringifyPlusSum()
    {
        if (mono)
            return x.ToString();

        return "(" + x + operation + y + ": " + Value() + ")";
    }

    public Cell()
    {
        x = 0;
        y = 0;
        operation = "";
        mono = false;
        fromMerged = false;
    }

    public Cell(double monoVar)
    {
        x = monoVar;
        mono = true;
    }

    public Cell(double var1, double var2, string operationSymbol)
    {
        x = var1;
        y = var2;
        operation = operationSymbol;
        mono = false;
    }
}

public class Solver : MonoBehaviour
{
    public int maxAttempts = 300;
    public int currentAttempt = 0;
    
    
    
    public TMP_Text SolverText;
    public DiceRoller diceRoller;
    public List<int> dice = new List<int>();




    public bool primeFound = false;
    bool allowedToRun = true;

    public List<string> attemptLogs = new List<string>();

    public GameObject LogList;
    public GameObject textPrefab;
    public ScrollRect scrollView;

    string[] symbol = new string[] { "+", "-", "*", "/" };
    string RandomOperator(bool ignoreDivide = false)
    {
        if(ignoreDivide)
            return symbol[Random.Range(0, 3)];

        return symbol[Random.Range(0, 4)];
    }

    // Start is called before the first frame update
    void Start()
    {
        scrollView.verticalNormalizedPosition = 1;
        dice = diceRoller.dice;


    }

    void AddScrollLog(string entry)
    {
        GameObject scrollItem = Instantiate(textPrefab);
        scrollItem.GetComponent<TextMeshProUGUI>().text = entry;
        scrollItem.transform.SetParent(LogList.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        float timeTook = 0;
        while(allowedToRun)
        {
            currentAttempt++;
            bool isPrime = false;

            if(currentAttempt == 1)
            {
                int additionSum = AdditionOnly();
                isPrime = EqualsDesiredPrimes(additionSum);
                if(isPrime)
                {
                    primeFound = true;
                    allowedToRun = false;
                    AddScrollLog("Addition Prime Found! Lucky!");
                    SucceededToSolve("Lucky! Addition produced Prime, Easy!");
                }
                    
            }
            else
            {
                string results = BruteForce();
                timeTook += Time.deltaTime;
                
                if(results != "")
                { // We've found one!
                    Debug.Log("We found one!: " + results);
                    primeFound = true;
                    allowedToRun = false;
                    AddScrollLog("Brute Force Prime Found! \n" + results);
                    SucceededToSolve("Brute Force! \n" + results);
                }

                results = RandomForce();
                if (results != "")
                { // We've found one!
                    Debug.Log("We found one!: " + results);
                    primeFound = true;
                    allowedToRun = false;
                    AddScrollLog("Random Force Prime Found! \n" + results);
                    SucceededToSolve("Random Force! \n" + results);
                }

                // results = InOrderRandomForce();
                if (results != "")
                { // We've found one!
                    Debug.Log("We found one!: " + results);
                    primeFound = true;
                    allowedToRun = false;
                    AddScrollLog("In Order Random Force Prime Found! \n" + results);
                    SucceededToSolve("In Order Random Force! \n" + results);
                }

                if (results != "")
                {
                    AddScrollLog("Time: " + timeTook);
                }

            }
            


            if (currentAttempt >= maxAttempts)
            {
                allowedToRun = false;
                FailedToSolve("Too many attempts");
            }
        }
    }


    string BruteForce()
    {


        return "";
    }

    string RandomForce()
    {
        string returnString = "";
        List<Cell> cells = new List<Cell>();

        string cellLog = "";
        string orderLog = "";
        string calcLog = "";
        for (int i = 0; i < dice.Count; i++)
        {
            int fNum = dice[i];
            Cell monoCell = new Cell(fNum);
            cells.Add(monoCell);
            //cellLog += monoCell.Stringify();

        }

        bool randomKill = false;
        
        

        int whileSafety = 0;
        while (cells.Count > 1 && whileSafety <= 200)
        {
            
            int ran1 = Random.Range(0, cells.Count);
            // Debug.Log("Cells + ran1:" + cells.Count + ":" + ran1);
            Cell cell1 = cells[ran1];
            cells.RemoveAt(ran1);
            // Debug.Log("Cell1: " + cell1.Value());

            int ran2 = Random.Range(0, cells.Count);
            // Debug.Log("Cells + ran2:" + cells.Count + ":" + ran2);
            Cell cell2 = cells[ran2];
            cells.RemoveAt(ran2);
            // Debug.Log("Cell2: " + cell2.Value());




            Cell mergeCell = new Cell(cell1.Value(), cell2.Value(),RandomOperator(true));
            mergeCell.fromMerged = true;
            // Debug.Log("Merged:" + mergeCell.Stringify() +":"+ mergeCell.Value());
            cells.Add(mergeCell);
            cellLog += mergeCell.Stringify(); // + ":" + mergeCell.Value();


            {
                calcLog += "(";
                
                if (!cell1.fromMerged)
                    calcLog += "<color=#00FF00>";
                calcLog += cell1.Value();
                if (!cell1.fromMerged)
                    calcLog += "</color>";
                
                calcLog += mergeCell.operation;

                if (!cell2.fromMerged)
                    calcLog += "<color=#00FF00>";
                calcLog += cell2.Value();
                if (!cell2.fromMerged)
                    calcLog += "</color>";

                calcLog += ": ";
                calcLog += mergeCell.Value();
                calcLog += ")";
                


            }
            
            // calcLog += mergeCell.StringifyPlusSum();


          

            if (!cell1.fromMerged)
            {
                orderLog += "<color=#00FF00>" + cell1.Stringify() + "</color>" + ", ";
            }
            if (!cell2.fromMerged)
            {
                orderLog += "<color=#00FF00>" + cell2.Stringify() + "</color>" + ", ";
            }





            whileSafety++;
        }

        if(cells.Count == 1)
        {
            //Debug.LogWarning("Final Cell: " + cells[0].Stringify() + ":" + cells[0].Value());
            AddScrollLog(currentAttempt+": Final Cell: " + cells[0].Stringify() + cells[0].Value());

            if(EqualsDesiredPrimes((int)cells[0].Value()))
            {
                AddScrollLog(cellLog);
                AddScrollLog("Order:" + orderLog);
                AddScrollLog("\nCalc:" + calcLog);
                return cells[0].Value().ToString();
            }
                

        }
        Debug.LogWarning("Left Zone: ");






        //AddScrollLog(cellLog);

        // currentAttempt++;

        return "";
    }

    string InOrderRandomForce()
    {
        string returnString = "";
        float workNumber = 0;
        List<Cell> cells = new List<Cell>();

        string cellLog = "";
        for (int i = 0; i < dice.Count; i++)
        {
            int fNum = dice[i];

            if (i == dice.Count - 1)
            {
                Cell monoCell = new Cell(fNum);
                cells.Add(monoCell);
                cellLog += monoCell.Stringify();
                continue;
            }
                
            
            int lNum = dice[i + 1];


            int ranOp = Random.Range(0, 3);
            
            Cell cell = new Cell(fNum, lNum, symbol[ranOp]);

            cells.Add(cell);
            cellLog += cell.Stringify();

            i++;
        }
        List<Cell> cells2 = new List<Cell>();

        cellLog += "\n";

        //while()



        
        for (int i = 0; i < cells.Count; i++)
        {
            double fNum = cells[i].Value();
            if (i == cells.Count - 1)
            {
                Cell monoCell = new Cell(fNum);
                cells2.Add(monoCell);
                cellLog += monoCell.Stringify();
                continue;
            }

            double lNum = cells[i + 1].Value();

            int ranOp = Random.Range(0, 3);

            Cell cell = new Cell(fNum, lNum, symbol[ranOp]);

            cells2.Add(cell);
            cellLog += cell.Stringify();


            i++;
        }
        

        AddScrollLog(cellLog);




        for (int i = 0; i < dice.Count; i++)
        {
            if (i == dice.Count - 1)
                continue;
            int fNum = dice[i];
            int lNum = dice[i + 1];




            int ranOp = Random.Range(0, 3);
            string vlog = "";
            vlog += fNum + symbol[ranOp] + lNum;

            Cell cell = new Cell(fNum,lNum,symbol[ranOp]);
            // Debug.Log(cell.Value());

            // AddScrollLog(vlog + ": " + cell.Value());
            
            if(ranOp == 0) // +
            {
                //workNumber +=
            }
        }

        


        return "";
    }

    string SmartForce()
    {
        int sum = 0;
        foreach (var item in dice)
        {
            sum += item;
        }

        if(sum < diceRoller.primesActive[0])
        {
            // Too small, multiply slot 0 and 1 together, re test
        }

        return "";
    }
    

    private bool EqualsDesiredPrimes(int additionSum)
    {
        foreach (var item in diceRoller.primesActive)
        {
            if (additionSum == item)
                return true;
        }

        return false;
    }

    int AdditionOnly()
    {
        string logEntry = currentAttempt + ": Addition Only: ";
        int sum = 0;
        foreach (var item in dice)
        {
            sum += item;
        }
        logEntry += sum;

        
        
        attemptLogs.Add(logEntry);
        AddScrollLog(logEntry);

        return sum;
    }

    void FailedToSolve(string reason)
    {
        Debug.LogWarning(reason);
    }
    private void SucceededToSolve(string v)
    {
        Debug.LogWarning(v);
    }
}
