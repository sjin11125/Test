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
    int keepSquareIndex;
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

    private void CheckIfShapeCanBePlaced()
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

    void CheckIfAnyLineIsCompleted()
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
        }
    }

    Sprite[] sprites = new Sprite[30];
    int sameColorColum;
    int sameColorRow;
    int[] sameColorColumLine = new int[5];
    int[] sameColorRowLine = new int[5];
    int[] completeIndexArray = new int[5];
    int[] sameColorZeroLine = new int[5];
    int[] sameColorOneLine = new int[5];
    private int CheckIfSquaresAreCompleted(List<int[]> data)//�����������_data.Count==10;
    {
        List<int[]> completedLines = new List<int[]>();
        var linesCompleted = 0;

        foreach (var line in data)//data�ȿ� 10���� ������ ���� �ݺ� 012345 012345
        {
            //line[0] 0 1 2 3 4        0 5 10 15 20     data[0]
            //line[1] 5 6 7 8 9        1 6 11 16 21     data[1]
            //line[2] 10 11 12 13 14   2 7 12 17 22     data[2]
            //line[3] 15 16 17 18 19   3 8 13 18 23     data[3]
            //line[4] 20 21 22 23 24   4 9 14 19 24     data[4]

            foreach (var squareIndex in line)//���ξȿ� �ε�����ȣ
            {
                sprites[squareIndex] = _gridSquares[squareIndex].transform.GetChild(2).GetComponent<Image>().sprite;
            }
        }
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
                    MainTimerObj.GetComponent<Timer>().timeLeft += 10;
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
    public bool CheckDiaOneColor()
    {
        var sameColorTrueDia = false;

        if (sprites[4] != null && sprites[8] != null && sprites[12] != null && sprites[16] != null && sprites[20] != null)
        {
            if (sprites[4] == sprites[8] && sprites[4] == sprites[12] && sprites[4] == sprites[16] && sprites[4] == sprites[20])
            {
                sameColorTrueDia = true;
                int j = 0;
                for (int i = 4; i < 21; i += 4)
                {
                    sameColorOneLine[j] = i;
                    j++;
                }
            }
        }
        return sameColorTrueDia;
    }
    public bool CheckDiaZeroColor()
    {
        var sameColorTrueDiaz = false;

        if (sprites[0] != null && sprites[6] != null && sprites[12] != null && sprites[18] != null && sprites[24] != null)
        {
            if (sprites[0] == sprites[6] && sprites[0] == sprites[12] && sprites[0] == sprites[18] && sprites[0] == sprites[24])
            {
                sameColorTrueDiaz = true;
                int j = 0;
                for (int i = 0; i < 25; i += 6)
                {
                    sameColorZeroLine[j] = i;
                    j++;
                }
            }
        }
       
        return sameColorTrueDiaz;
    }

    public bool CheckColumColor()
    {
        var sameColorCompCol = 0;
        var sameColorTrueCol = false;

        for (int i = 0; i < 21; i += 5)
        {
            if (sprites[i] != null && sprites[i + 1] != null && sprites[i + 2] != null && sprites[i + 3] != null && sprites[i + 4] != null)
            {
                if (sprites[i] == sprites[i + 1] && sprites[i] == sprites[i + 2] && sprites[i] == sprites[i + 3] && sprites[i] == sprites[i + 4])
                {
                    sameColorCompCol = i;
                    sameColorTrueCol = true;
                }
            }
        }

        if(sameColorTrueCol)
        {
            for (int i = 0; i < 5; i++)
            {
                sameColorColumLine[i] = sameColorCompCol + i;
            }
        }
        return sameColorTrueCol;
    }

    public bool CheckRowColor() 
    {
        var sameColorCompRow = 0;
        var sameColorTrueRow = false;

        for (int i = 0; i < 5; i++)
        {
            if (sprites[i] != null && sprites[i + 5] != null && sprites[i + 10] != null && sprites[i + 15] != null && sprites[i + 20] != null)
            {
                if (sprites[i] == sprites[i + 5] && sprites[i] == sprites[i + 10] && sprites[i] == sprites[i + 15] && sprites[i] == sprites[i + 20])
                {
                    sameColorCompRow = i;
                    sameColorTrueRow = true;
                }
            }
        }

        if (sameColorTrueRow)
        {
            int j = 0;
            for (int i = 0; i < 21; i += 5)
            {
                sameColorRowLine[j] = sameColorCompRow + i;
                j++;
            }
        }
        return sameColorTrueRow;
    }

    public bool GameOver()
    {
        bool isGameover = true;

        for (int i = 0; i < 25; i++)
        {
            var comp = _gridSquares[i].GetComponent<GridSquare>();
            if (comp.SquareOccupied == false)
            {
                isGameover = false;
            }
        }
        return isGameover;
    }

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
                comp.ColorTransfer();//�׸��� ������ �Լ� ȣ��

                var KeepCount = _gridSquares[keepSquareIndex].GetComponent<GridSquare>().keepCount;
                var KeepTimerObj = _gridSquares[keepSquareIndex].GetComponent<GridSquare>().keepTimer;
                if (KeepCount > 0 && KeepTimerObj.activeSelf == false)
                {
                    comp.Deactivate();
                    comp.ClearOccupied();
                }
            }
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

                var TrashCount = _gridSquares[trashCanIndex].GetComponent<GridSquare>().trashCount;
                var TrashTimerObj = _gridSquares[trashCanIndex].GetComponent<GridSquare>().keepTimer;
                if (TrashCount > 0 && TrashTimerObj.activeSelf == false)
                {
                    comp.Deactivate();
                    comp.ClearOccupied();
                }
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
