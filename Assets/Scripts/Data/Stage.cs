using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Game/Stage")]
public class Stage : SerializedScriptableObject
{
    
    public int Id;
    

    public Sprite Icon;
    public int Width;
    public int Height;
    public int MinRuleToShow = 0;
    public int MaxRuleToShow = 0;
    public int MinVisibleElements = 0;
    public int MaxVisibleElements = 0;
    public int MaxVisibleCells = 0;
    public int timeToComplite = 600;
    public Transform stagePrefab;




    public GameData GetData(int seed)
    {
        var state = Random.state;
        Random.InitState(seed);

        List<int> board = GenerateBoard(seed);
        List<int> rules = GenerateClues(board);
        (List<int>, List<int>) difficulty = SetDifficulty(board, rules);

#if UNITY_EDITOR
        Debug.Log("[" + string.Join(',', board.ToArray()) + "]");
        Debug.Log("[" + string.Join(',', rules.ToArray()) + "]");
        Debug.Log("[" + string.Join(',', difficulty.Item1.ToArray()) + "]");
        Debug.Log("[" + string.Join(',', difficulty.Item2.ToArray()) + "]");
#endif
        Random.state = state;
        return new GameData(Width, Height, difficulty.Item1.Select(i => i == 0 ? new List<int>() : new List<int>(){i}).ToList(), difficulty.Item2);
    }

    private (List<int>, List<int>) SetDifficulty(List<int> board, List<int> rules)
    {
        if (MaxVisibleElements == 0) return (new List<int>(new int[Width*Height]), rules);

        List<int> visibleCells = new List<int>(new int[Width*Height]);
        List<int> visibleRules = new List<int>(new int[rules.Count]);

        List<int> relevantPos = new List<int>();

        for (int i = 0; i < rules.Count; i++)
        {
            if (rules[i] >= MinRuleToShow && rules[i] <= MaxRuleToShow)
                relevantPos.Add(i);
        }

        int varCount = Mathf.Min(Random.Range(MinVisibleElements, MaxVisibleElements + 1), relevantPos.Count);
        var toHide = relevantPos.Count - varCount;
        for (int i = 0; i < toHide; i++)
        {
            relevantPos.RemoveAt(Random.Range(0, relevantPos.Count));
        }

        foreach (var pos in relevantPos)
        {
            visibleRules[pos] = rules[pos];
        }
    
        int visibleElementsCount = Random.Range(MinVisibleElements, MaxVisibleElements + 1);
        int visibleCellsCount = Random.Range(0, MaxVisibleCells + 1);
        List<int> xBoard = new List<int>();
        List<int> yBoard = new List<int>();
    
        for (int i = 0; i < visibleCellsCount; i++)
        {
            int curPosition = Random.Range(0, board.Count);
            while (yBoard.Contains(curPosition / Width) || xBoard.Contains(curPosition - Width * (curPosition / Width)))
                curPosition = Random.Range(0, board.Count);
            int y = curPosition / Width;
            int x = curPosition - Width * y;
            xBoard.Add(x);
            yBoard.Add(y);
            visibleCells[curPosition] = board[curPosition];
        }
    
        return (visibleCells, visibleRules);
    }


    private List<int> GenerateClues(List<int> board)
    {
        var result = new List<int>(new int[(Width + Height) * 2]);
        int curRule;
        int highestHouse;
        for (var i = 0; i < Width; i++)
        {
            curRule = 0;
            highestHouse = 0;
            for (var j = i; j < Width * Height; j += Width)
            {
                if (board[j] <= highestHouse) continue;
                highestHouse = board[j];
                curRule++;
            }
            result[i] = curRule;
        
            curRule = 0;
            highestHouse = 0;
            for (var j = Width * (Height - 1) + i; j >= i; j -= Width)
            {
                if (board[j] <= highestHouse) continue;
                highestHouse = board[j];
                curRule++;
            }
            result[Width * 2 + Height - 1 - i] = curRule;
        }

        for (var i = 0; i < Height; i++)
        {
            curRule = 0;
            highestHouse = 0;
            for (var j = i * Width; j < i * Width + Width; j++)
            {
                if (board[j] <= highestHouse) continue;
                highestHouse = board[j];
                curRule++;
            }
            result[(Width + Height) * 2 - 1 - i] = curRule;
            curRule = 0;
            highestHouse = 0;
            for (var j = i * Width + Width - 1; j >= i * Width; j--)
            {
                if (board[j] <= highestHouse) continue;
                highestHouse = board[j];
                curRule++;
            }
            result[Width + i] = curRule;
        }
    
        return result;
    }


    
    private List<int> GenerateBoard(int seed)
    {
        List<int> board = Enumerable.Repeat(0, Height * Width).ToList();

        List<List<int>> rowPossibleNumbers = new List<List<int>>();
        List<List<int>> colPossibleNumbers = new List<List<int>>();
        for (int i = 0; i < Height; i++)
            rowPossibleNumbers.Add(Enumerable.Range(1, Height).ToList());

        for (int i = 0; i < Width; i++)
            colPossibleNumbers.Add(Enumerable.Range(1, Height).ToList());

        if (FillCell(0, board, rowPossibleNumbers, colPossibleNumbers))
        {
            return board;
        }
        
        Debug.LogError("Failed to generate board");
        return null;
        
    }

    private bool FillCell(int index, List<int> board, List<List<int>> rowPossibleNumbers,
        List<List<int>> colPossibleNumbers)
    {
        if (index >= board.Count)
        {
            return true;
        }

        int row = index / Width;
        int col = index % Width;

        List<int> possibleNumbers = rowPossibleNumbers[row].Intersect(colPossibleNumbers[col]).ToList();
        while (possibleNumbers.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, possibleNumbers.Count);
            int randomNumber = possibleNumbers[randomIndex];
            board[index] = randomNumber;

            rowPossibleNumbers[row].Remove(randomNumber);
            colPossibleNumbers[col].Remove(randomNumber);

            if (FillCell(index + 1, board, rowPossibleNumbers, colPossibleNumbers))
            {
                return true;
            }

            rowPossibleNumbers[row].Add(randomNumber);
            colPossibleNumbers[col].Add(randomNumber);
            possibleNumbers.Remove(randomNumber);
        }

        board[index] = 0;
        return false;
    }
}

