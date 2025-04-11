using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Width;
    public int Height;
    public int MaxSize => Mathf.Max(Width, Height);

    public List<List<int>> GameCells = new List<List<int>>();
    public List<int> RuleCells = new List<int>();


    public GameData()
    {
        
    }
    public GameData(int width, int height, List<List<int>> preset, List<int> rules)
    {
        Width = width;
        Height = height;
        GameCells = preset;
        RuleCells = rules;
    }

    public bool CheckWin()
    {
        Dictionary<int, OrderedSet<int>> rows = new Dictionary<int, OrderedSet<int>>();
        Dictionary<int, OrderedSet<int>> cols = new Dictionary<int, OrderedSet<int>>();

        for (int y = 0; y < Height; y++)
        {
            if (!rows.ContainsKey(y))
                rows[y] = new OrderedSet<int>();
            for (int x = 0; x < Width; x++)
            {
                if (GameCells[y * Width + x].Count != 1)
                    return false;
                
                rows[y].Add(GameCells[y * Width + x][0]);
            }
        }
        
        for (int x = 0; x < Width; x++)
        {
            if (!cols.ContainsKey(x))
                cols[x] = new OrderedSet<int>();
            for (int y = 0; y < Height; y++)
            {
                
                if (GameCells[y * Width + x].Count != 1)
                    return false;

                cols[x].Add(GameCells[y * Width + x][0]);
            }
        }
        

        foreach (var keyValuePair in rows)
            if (keyValuePair.Value.Count < Width)
                return false;
        foreach (var keyValuePair in cols)
            if (keyValuePair.Value.Count < Height)
                return false;
        
        Debug.Log(rows);
        Debug.Log(cols);
        var d = RuleCells.GetRange(Height + Width * 2, Height);
        d.Reverse();
        if (!CheckRules(d, rows))
            return false;
        if (!CheckRules(RuleCells.GetRange(0, Width), cols))
            return false;

        ReverseCollections(rows);
        ReverseCollections(cols);

        d = RuleCells.GetRange(Height + Width, Width);
        d.Reverse();
        if (!CheckRules(RuleCells.GetRange(Width, Height), rows))
            return false;
        if (!CheckRules(d, cols))
            return false;

        return true;
    }
    
    public List<int> GetIncorrectCells()
    {
        Dictionary<(int, int), List<int>> str = new Dictionary<(int, int), List<int>>();
        Dictionary<(int, int), List<int>> col = new Dictionary<(int, int), List<int>>();
        List<int> indexes = new List<int>();
        
        
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if(GameCells[j + i * Width].Count == 1)
                {
                    if (str.ContainsKey((i, GameCells[j + i * Width][0])))
                        str[(i, GameCells[j + i * Width][0])].Add(j);
                    else
                        str.Add((i, GameCells[j + i * Width][0]), new List<int>(new[] { j }));
                }
                
            }
        }
        
        for (int j = 0; j < Width; j++)
        {
            for (int i = 0; i < Height; i++)
            {
                if(GameCells[j + i * Width].Count == 1)
                {
                    if (col.ContainsKey((j, GameCells[j + i * Width][0])))
                        col[(j, GameCells[j + i * Width][0])].Add(i);
                    else
                        col.Add((j, GameCells[j + i * Width][0]), new List<int>(new[] { i }));
                }
                
            }
        }
        

        foreach (var StrEl in str)
        {
            if (StrEl.Value.Count > 1)
            
                foreach (var ValEl in StrEl.Value)
                    indexes.Add(StrEl.Key.Item1 * Width + ValEl);
        }
        
        foreach (var ColEl in col)
        {
            if (ColEl.Value.Count > 1)
            
                foreach (var ValEl in ColEl.Value)
                    indexes.Add(ColEl.Key.Item1 + ValEl * Width);
        }

        return indexes;
    }


    public List<int> GetIncorrectRules()
    {
        List<int> indexes = new List<int>();

        for (var i = 0; i < Height; i++)
        {
            // Check rows from top to bottom
            if (i < Width)
            {
                int curRule = 0;
                int highestHouse = 0;
                bool isFull = true;
                for (var j = i; j < Width * Height; j += Width)
                {
                    if (GameCells[j].Count != 1)
                    {
                        
                        isFull = false;
                        break;
                    }

                    if (GameCells[j][0] <= highestHouse) continue;
                    highestHouse = GameCells[j][0];
                    curRule++;
                }
                if (RuleCells[i] != curRule && isFull)
                    indexes.Add(i);
            }
            // Check rows from bottom to top
            if (i < Width)
            {
                int curRule = 0;
                int highestHouse = 0;
                bool isFull = true;
                for (var j = Height * Width - Width + i; j >= i; j -= Width)
                {
                    if (GameCells[j].Count != 1)
                    {
                        isFull = false;
                        break;
                    }

                    if (GameCells[j][0] <= highestHouse) continue;
                    
                    highestHouse = GameCells[j][0];
                    curRule++;
                }

                if (RuleCells[Width * 2 + Height - 1 - i] != curRule && isFull)
                    indexes.Add(Width * 2 + Height - 1 - i);
            }
            
            // Check columns from left to right
            if (i < Height)
            {
                int curRule = 0;
                int highestHouse = 0;
                bool isFull = true;
                for (var j = i * Width; j < i * Width + Width; j++)
                {
                    if (GameCells[j].Count != 1)
                    {
                        isFull = false;
                        break;
                    }

                    if (GameCells[j][0] <= highestHouse) continue;
                    highestHouse = GameCells[j][0];
                    curRule++;
                }

                if (RuleCells[(Width * 2 + Height * 2) - 1 - i] != curRule && isFull)
                    indexes.Add((Width * 2 + Height * 2) - 1 - i);
            }
            

            // Check columns from right to left
            if (i < Height)
            {
                int curRule = 0;
                int highestHouse = 0;
                bool isFull = true;
                for (var j = (i + 1) * Width - 1; j >= i * Width; j--)
                {
                    if (GameCells[j].Count != 1)
                    {
                        isFull = false;
                        break;
                    }

                    if (GameCells[j][0] <= highestHouse) continue;
                    highestHouse = GameCells[j][0];
                    curRule++;
                }
                //if (RuleCells[(Width * 2 + Height * 2) - 1 - i] != curRule && isFull)
                //    indexes.Add((Width * 2 + Height * 2) - 1 - i);
                if (RuleCells[i + Width] != curRule && isFull)
                    indexes.Add(Width + i);
                
            }
        }

        return indexes;
    }


    private void ReverseCollections(Dictionary<int, OrderedSet<int>> data)
    {
        foreach (var keyValuePair in data)
        {
            var s = keyValuePair.Value.ToList();
            s.Reverse();
            keyValuePair.Value.Clear();
            foreach (var o in s)
            {
                keyValuePair.Value.Add(o);
            }
        }
    }

    private bool CheckRules(List<int> rules, Dictionary<int, OrderedSet<int>> data)
    {


        for (int i = 0; i < rules.Count; i++)
        {
            int r = rules[i];
            if(r == 0)
                continue;
            var rawData = data[i].ToList();
            var count = 1;
            var value = rawData[0];

            //if (value == expectedSize && r != 1)
            //    return false;
            
            for (int x = 1; x < rawData.Count; x++)
            {
                if (rawData[x] > value)
                {
                    value = rawData[x];
                    count++;
                }
            }

            if (count != r)
                return false;
        }
        

        return true;
    }
}

public class Dictionary<TKey1, TKey2, TValue> : Dictionary<Tuple<TKey1, TKey2>, TValue>,
    IDictionary<Tuple<TKey1, TKey2>, TValue>
{
    public TValue this[TKey1 key1, TKey2 key2]
    {
        get { return base[Tuple.Create(key1, key2)]; }
        set { base[Tuple.Create(key1, key2)] = value; }
    }

    public void Add(TKey1 key1, TKey2 key2, TValue value)
    {
        base.Add(Tuple.Create(key1, key2), value);
    }

    public bool ContainsKey(TKey1 key1, TKey2 key2)
    {
        return base.ContainsKey(Tuple.Create(key1, key2));
    }
}