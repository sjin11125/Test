using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GridScript : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    int columns = 5;
    int rows = 6;
    float squaresGap = 0f;
    public GameObject gridSquare;
    Vector2 startPosition = new Vector2(-375f, 275f);
    float squareScale = 1.5f;
    float everySquareOffset = 12f;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private System.Collections.Generic.List<GameObject> _gridSquares = new System.Collections.Generic.List<GameObject>();
    private LineIndicator _lineIndicator;

    public GameObject gameOver;
    public int keepSquareIndex;
    int trashCanIndex;
    public GameObject effectShape;

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }

    void Start()
    {
        Time.timeScale = 1f;
        _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
    }

    void Update()
    {
        if(keepSquareIndex != 30)
        {
            UseKeep();
        }
        if(trashCanIndex != 30)
        {
            UseTrashCan();
        }
        SettingKeep();
        GetInformation();

       if(GridSquare.UseKeepBool==true)//ŵ �ÿ��
        {
            CheckIfKeepLineIsCompleted();
        }
    }

        private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPositions();
    }

    private void SpawnGridSquares()
    {
        //0, 1, 2, 3, 4,
        //5, 6, 7, 8, 9

        int square_index = 0;

        for (var row = 0; row < rows; ++row)
        {
            for (var column = 0; column < columns; ++column)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);

                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(_lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);
                square_index++;
            }
        }
    }

    private void SetGridSquaresPositions()
    {
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();

        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;

        foreach (GameObject square in _gridSquares)
        {
            if (column_number + 1 > columns)
            {
                square_gap_number.x = 0;
                //go to next col
                column_number = 0;
                row_number++;
                row_moved = true;
            }

            var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaresGap);
            var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaresGap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;
            }

            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaresGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset,
                startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset,
                startPosition.y - pos_y_offset, 0.0f);

            column_number++;
        }
    }

    public void CheckIfShapeCanBePlaced()
    {
        var squareIndexes = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();//�� ������ ������ ������ ���������� ����

            if (gridSquare.Selected && !gridSquare.SquareOccupied)//����� ����ִ� ����
            {
                squareIndexes.Add(gridSquare.SquareIndex);//squareIndexes�� ���� ������ ��ġ �ε��� ����_0-24
                gridSquare.Selected = false;//������ ���� ���ø��ϰ�
                //gridSquare.ActivateSquare();
            }
        }

        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();//�������� ������ ���޹���
        if (currentSelectedShape == null) return; //there is no selected shape;

        if (currentSelectedShape.TotalSquareNumber == squareIndexes.Count)//shape��ũ��Ʈ TotalSquareNumber�� squareIndexes�� ����ִ� �� ���� ��
        {
            //Debug.Log(squareIndexes.Count);//�׻� 1�� ����
            foreach (var squareIndex in squareIndexes)//squareIndexes�迭���ִ� �ε�������
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard();//�� �ε����� ������� �Լ��ѱ�[0-24]_��Ƽ��Ʈ��� ����� ǥ��
            }

            var shapeLeft = 0;

            foreach (var shape in shapeStorage.shapeList)//Shape�� �ϳ��� �˻�
            {
                if (shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())//������ ������϶�_������� �����̰������� ���ϴ°��� �׸��� ����� ������ ����
                {
                    shapeLeft++;
                }
            }

            if (shapeLeft == 0)//������ΰ� ����==�ùٸ� ������� ����
            {
                GameEvents.RequestNewShapes();//���ο� ������ ���� shapeStorage�� ����
            }

            else//������ΰ� �ִ�
            {
                GameEvents.SetShapeInactive();//�����̰�
            }

            //�������� ������ ����� ������
            CheckIfAnyLineIsCompleted();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();//ó����ġ��
        }
    }
    public void CheckIfKeepLineIsCompleted()//ŵ�� ���Ͷ� ������ ���⶧���� ���ο� �Լ� ������
    {
        List<int[]> lines = new List<int[]>();

        //columns
        foreach (var column in _lineIndicator.columnIndexes)//0-5
        {
            lines.Add(_lineIndicator.GetVerticalLine(column));//column�� 0-4_5��
        }

        //rows
        for (var row = 0; row < 5; row++)
        {
            List<int> data = new List<int>(5);
            for (var index = 0; index < 5; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]); //5���� data�� ����
            }
            lines.Add(data.ToArray());//lines�� ����
        }

        var completedLines = CheckIfSquaresAreCompleted(lines);//��(0-5)��(0-5) �������� �� ������ ��ȯ int�� ����

        if (completedLines > 2)
        {
            //TODO: Play bouns animation.
        }

        var totalScores = 10 * completedLines;
        GameEvents.AddScores(totalScores);
        if (GameOver())
        {
            gameOver.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();

        //columns
        foreach (var column in _lineIndicator.columnIndexes)//0-5
        {
            lines.Add(_lineIndicator.GetVerticalLine(column));//column�� 0-4_5��
        }

        //rows
        for (var row = 0; row < 5; row++)
        {
            List<int> data = new List<int>(5);
            for (var index = 0; index < 5; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]); //5���� data�� ����
            }
            lines.Add(data.ToArray());//lines�� ����
        }

        var completedLines = CheckIfSquaresAreCompleted(lines);//��(0-5)��(0-5) �������� �� ������ ��ȯ int�� ����

        if (completedLines > 2)
        {
            //TODO: Play bouns animation.
        }

        var totalScores = 10 * completedLines;
        GameEvents.AddScores(totalScores);
        if (GameOver())
        {
            gameOver.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public int[] sameColorColumLine = new int[5];
    public int[] sameColorRowLine = new int[5];
    public int[] sameColorZeroLine = new int[5];
    public int[] sameColorOneLine = new int[5];
    public int[] completeIndexArray = new int[5];
    private int CheckIfSquaresAreCompleted(List<int[]> data)//�����������_data.Count==10;
    {
        List<int[]> completedLines = new List<int[]>();
        var linesCompleted = 0;

        if (CheckColumColor())      //�� üũ
        {
            completedLines.Add(sameColorColumLine);
        }
        if (CheckRowColor())         //�� üũ
        {
            completedLines.Add(sameColorRowLine);
        } 
        if (CheckDiaZeroColor())            //�밢 üũ
        {
            completedLines.Add(sameColorZeroLine);           
        }
        if (CheckDiaOneColor())
        {
            completedLines.Add(sameColorOneLine);
        }
        foreach (var line in completedLines)//����� ��� ���� ����
        {
            var i = 0;

            foreach (var squareIndex in line)
            {
                completeIndexArray[i] = squareIndex;
                i++;
            }          
            if (SameColorLines())
            {
                linesCompleted++;

                GameObject MainTimerObj = GameObject.FindGameObjectWithTag("MainTimer");
                if (MainTimerObj != null)
                {
                    MainTimerObj.GetComponent<Timer>().timeLeft += 10;//������ ���⶧���� �ð��� �þ
                }
            }
        }
        return linesCompleted;
    }

    public bool SameColorLines()
    {
        var sameColorLine = false;
        List<GridSquare> comp = new List<GridSquare>();
        
        for (int i = 0; i < completeIndexArray.Length; i++)
        {
            var com = _gridSquares[completeIndexArray[i]].GetComponent<GridSquare>();
            com.Deactivate();
            com.ClearOccupied();
           
            GameObject effect = Instantiate(effectShape, new Vector3(_gridSquares[completeIndexArray[i]].transform.localPosition.x,
                _gridSquares[completeIndexArray[i]].transform.localPosition.y, 0), Quaternion.identity) as GameObject;
            effect.transform.SetParent(GameObject.FindGameObjectWithTag("Grid").transform, false);
            //square�� ������� �� ��ġ ���� �޾Ƽ� Instance ����
        }
        sameColorLine = true;

        return sameColorLine;
    }

    public string[] colors = new string[30];
    public string[] shapes = new string[30];
    public void GetInformation()
    {
        for (int i = 0; i < 30; i++)
        {
            colors[i] = _gridSquares[i].GetComponent<GridSquare>().currentColor;
            shapes[i] = _gridSquares[i].GetComponent<GridSquare>().currentShape;
        }
    }
    public bool CheckDiaZeroColor()
    {
        var sameTrueDiaz = false;
        var sameColorTrueDiaz = false;
        var sameShapeTrueDiaz = false;

        if (colors[0] != null && colors[6] != null && colors[12] != null && colors[18] != null && colors[24] != null)
        {
            if (colors[0] == colors[6] && colors[0] == colors[12] && colors[0] == colors[18] && colors[0] == colors[24])
            {
                sameColorTrueDiaz =  true;
                int j = 0;
                for (int i = 0; i < 25; i += 6)
                {
                    sameColorZeroLine[j] = i;
                    j++;
                }
            }
            if (shapes[0] == shapes[6] && shapes[0] == shapes[12] && shapes[0] == shapes[18] && shapes[0] == shapes[24])
            {
                sameShapeTrueDiaz = true;
                int j = 0;
                for (int i = 0; i < 25; i += 6)
                {
                    sameColorZeroLine[j] = i;
                    j++;
                }
            }
        }

        if(sameColorTrueDiaz || sameShapeTrueDiaz)
        {
            sameTrueDiaz = true;
        }
        else
        {
            sameTrueDiaz = false;
        }
        return sameTrueDiaz;
    }

    public bool CheckDiaOneColor()
    {
        var sameTrueDia = false;
        var sameColorTrueDia = false;
        var sameShapeTrueDia = false;

        if (colors[4] != null && colors[8] != null && colors[12] != null && colors[16] != null && colors[20] != null)
        {
            if (colors[4] == colors[8] && colors[4] == colors[12] && colors[4] == colors[16] && colors[4] == colors[20])
            {
                sameColorTrueDia = true;
                int j = 0;
                for (int i = 4; i < 21; i += 4)
                {
                    sameColorOneLine[j] = i;
                    j++;
                }
            }

            if (shapes[4] == shapes[8] && shapes[4] == shapes[12] && shapes[4] == shapes[16] && shapes[4] == shapes[20])
            {
                sameShapeTrueDia = true;
                int j = 0;
                for (int i = 4; i < 21; i += 4)
                {
                    sameColorOneLine[j] = i;
                    j++;
                }
            }
        }

        if (sameColorTrueDia || sameShapeTrueDia)
        {
            sameTrueDia = true;
        }
        else
        {
            sameTrueDia = false;
        }
        return sameTrueDia;
    }

    public bool CheckColumColor()
    {
        var sameCompCol = 0;
        var sameTrueCol = false;
        var sameColorTrueCol = false;
        var sameShapeTrueCol = false;

        for (int i = 0; i < 21; i += 5)//0 5 10 15 20
        {
            if (colors[i] != null && colors[i + 1] != null && colors[i + 2] != null && colors[i + 3] != null && colors[i + 4] != null)
            {
                if (colors[i] == colors[i + 1] && colors[i] == colors[i + 2] && colors[i] == colors[i + 3] && colors[i] == colors[i + 4])
                {
                    sameCompCol = i;
                    sameColorTrueCol = true;
                }
                if (shapes[i] == shapes[i + 1] && shapes[i] == shapes[i + 2] && shapes[i] == shapes[i + 3] && shapes[i] == shapes[i + 4])
                {
                    sameCompCol = i;
                    sameShapeTrueCol = true;
                }
            }
        }

        if(sameColorTrueCol|| sameShapeTrueCol)
        {
            sameTrueCol = true;
            for (int i = 0; i < 5; i++)
            {
                sameColorColumLine[i] = sameCompCol + i;
            }
        }
        else
        {
            sameTrueCol = false;
        }
        return sameTrueCol;
    }

    public bool CheckRowColor() 
    {
        var sameCompRow = 0;
        var sameTrueRow = false;
        var sameColorTrueRow = false;
        var sameShapeTrueRow = false;

        for (int i = 0; i < 5; i++)
        {
            if (colors[i] != null && colors[i + 5] != null && colors[i + 10] != null && colors[i + 15] != null && colors[i + 20] != null)
            {
                if (colors[i] == colors[i + 5] && colors[i] == colors[i + 10] && colors[i] == colors[i + 15] && colors[i] == colors[i + 20])
                {
                    sameCompRow = i;
                    sameColorTrueRow = true;
                }
                if (shapes[i] == shapes[i + 5] && shapes[i] == shapes[i + 10] && shapes[i] == shapes[i + 15] && shapes[i] == shapes[i + 20])
                {
                    sameCompRow = i;
                    sameShapeTrueRow = true;
                }
            }
        }

        if (sameColorTrueRow || sameShapeTrueRow)
        {
            sameTrueRow = true;
            int j = 0;
            for (int i = 0; i < 21; i += 5)
            {
                sameColorRowLine[j] = sameCompRow + i;
                j++;
            }
        }
        else
        {
            sameTrueRow = false;
        }
        return sameTrueRow;
    }

    public bool GameOver()
    {
        bool isGameover = false;
        int fullNum = 0;

        for (int i = 0; i < 25; i++)
        {         
            if (colors[i] != null)
            {
                fullNum++;
            }
        }
        if(fullNum == 25)
        {
            isGameover = true;
        }

        return isGameover;
    }
    public GameObject KeepShapeObj;
    public Sprite KeepImg;
    public String KeepColor;
    public String KeepShape;
    int keepNum =0;
    public void UseKeep()//29�� ������ Ŭ������ shape�� ������ ��������
    {
        GameObject ItemControllerObj = GameObject.FindGameObjectWithTag("ItemController");//��Ʈ�ѷ����� ������ �ε����� ���� ��ġ ����
        if (ItemControllerObj != null)
        {
            keepSquareIndex = ItemControllerObj.GetComponent<ItemController>().keepItemIndex;
        }

        if (keepSquareIndex != 30)
        {           
            var comp = _gridSquares[keepSquareIndex].GetComponent<GridSquare>();//index�� ģ��
            if (comp.SquareOccupied == true)//index�� ������̾�
            {
                KeepImg = comp.transform.GetChild(2).gameObject.GetComponent<Image>().sprite;
                KeepColor = colors[keepSquareIndex];//ŵ ������ �ڸ��� ������ �����س��´�
                KeepShape = shapes[keepSquareIndex];

                if(keepNum < 1)//�ϳ��� �����ո� �����Ѵ�
                {
                    GameObject keepInstance = Instantiate(KeepShapeObj) as GameObject;
                    keepInstance.transform.SetParent(_gridSquares[keepSquareIndex].transform, false);
                    Vector3 pos = new Vector3(0, 0, 0);
                    keepInstance.transform.localPosition = pos;
                    keepNum++;
                }
                
                GameObject KeepObj = GameObject.FindGameObjectWithTag("KeepShape");//�����Ե� ������ �����տ� ����
                if (KeepObj != null)
                {
                    KeepObj.GetComponent<CreateKeepShape>().keepColor  = KeepColor; 
                    KeepObj.GetComponent<CreateKeepShape>().keepShape  = KeepShape;
                }
                comp.Deactivate();//�������� �����ϰ� ������ ������ �� �ڸ��� ��Ƽ�� �̹������� ����
                comp.ClearOccupied();
            }
            keepNum = 0;
        }
    }

    public void UseTrashCan()
    {
        GameObject ItemControllerObj = GameObject.FindGameObjectWithTag("ItemController");
        if (ItemControllerObj != null)
        {
            trashCanIndex = ItemControllerObj.GetComponent<ItemController>().trashCanItemIndex;
        }

        if (trashCanIndex != 30)
        {
            var comp = _gridSquares[trashCanIndex].GetComponent<GridSquare>();
            if (comp.SquareOccupied == true)
            {
                comp.TrashCan();
                comp.Deactivate();
                comp.ClearOccupied();
            }
        }
    }

    public void SettingKeep()//LineIndicator�� ���� �ϳ� �� ������µ� �츰 keep�ڸ��� can�ڸ��� �ʿ��ϴ� �װ� �ƴ϶�� ����
    {      
        for (int i = 25; i < 30; i++)//ItemController���� ���ù������� �ֵ��� ����
        {
            if(trashCanIndex != i && keepSquareIndex != i)
            {
                var comp = _gridSquares[i].GetComponent<GridSquare>();
                comp.NonKeep();//GridSquare�� �ڱ��ڽ��� ���� �Լ� ȣ��
            }
        }
    }
}
