using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int height = 5; // Board height
    public int width = 5; // Board width

    public int minMatch = 3; // Minimum items in line to be a valid match

    public float itemSwapTime = 0.1f;

    // Reference for loading our tile prefabs
    private GameObject[] _tiles;

    // Reference to store which item is on each board position
    private Item[,] _items;

    // Store a reference to the transform of our Board object.
    private Transform boardHolder;

    private Item _selectedItem; // Player's current selected item

    void BoardSetup()
    {

        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;

        _items = new Item[width, height];

        //Loop along x axis.
        for (int x = 0; x < width; x++)
        {
            //Loop along y axis
            for (int y = 0; y < height; y++)
            {
                _items[x, y] = InstantiateDoddle(x, y);
            }
        }
    }

    Item InstantiateDoddle(int x, int y)
    {
        //Choose a random tile from our array of tile prefabs and prepare to instantiate it.
        GameObject toInstantiate = _tiles[Random.Range(0, _tiles.Length)];

        //Instantiate the GameObject instance using the prefab chosen for to Instantiate at the Vector3 corresponding to current grid position in loop.
        Item newDoddle = ((GameObject)Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity)).GetComponent<Item>();

        //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
        newDoddle.transform.SetParent(boardHolder);

        // Set current item position in board
        newDoddle.SetPosition(x, y);

        return newDoddle;
    }

    void LoadTiles()
    {
        _tiles = Resources.LoadAll<GameObject>("Prefabs");
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i].GetComponent<Item>().id = i; // Set an UID for the tile
            Debug.Log(string.Format("Loaded game object {0} with id {1}", _tiles[i].name, i));
        }
        Debug.Log(string.Format("Loaded {0} objects", _tiles.Length));
    }

    void SweepBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                MatchInfo match = GetMatch(_items[i, j]);
                if (match.valid) // We have a match, change this item
                {
                    Debug.Log("Found a match on game init, changing tile");
                    Destroy(_items[i, j].gameObject);
                    _items[i, j] = InstantiateDoddle(i, j);
                    j--; // Recheck this position
                }
            }
        }
    }

    public void Init()
    {
        LoadTiles(); // TODO make this a global referenced Scriptable Object
        BoardSetup();
        SweepBoard();

        // Register a callback to be called everytime player selects an item
        Item.OnMouseOverItemEventHandler += OnMouseOverItem;
    }

    MatchInfo GetMatch(Item item)
    {
        MatchInfo m = null;
        List<Item> horizontalMatch = GetMatchHorizontal(item);
        List<Item> verticalMatch = GetMatchVertical(item);

        bool shouldPreferenceHorizontal = horizontalMatch.Count >= verticalMatch.Count; // Is the horizontal match higher than the vertical?

        if (shouldPreferenceHorizontal)
        {
            m = new MatchInfo(horizontalMatch);
        }
        else
        {
            m = new MatchInfo(verticalMatch);
        }
        return m;
    }

    List<Item> GetMatchHorizontal(Item item)
    {
        List<Item> matched = new List<Item> { item };
        int leftItem = item.x - 1;  // Imediatelly left item
        int rightItem = item.y + 1; // Imediatelly right item
        while (leftItem >= 0 && _items[leftItem, item.y].id == item.id)
        {
            matched.Add(_items[leftItem, item.x]);
            leftItem--;
        }
        while (rightItem < width && _items[rightItem, item.y].id == item.id)
        {
            matched.Add(_items[rightItem, item.x]);
            rightItem++;
        }
        return matched;
    }

    List<Item> GetMatchVertical(Item item)
    {
        List<Item> matched = new List<Item> { item };
        int lowerItem = item.y - 1; // Imediatelly lower item
        int upperItem = item.y + 1; // Imediatelly upper item
        while (lowerItem >= 0 && _items[item.x, lowerItem].id == item.id)
        {
            matched.Add(_items[item.y, lowerItem]);
            lowerItem--;
        }
        while (upperItem < height && _items[item.x, upperItem].id == item.id)
        {
            matched.Add(_items[item.y, upperItem]);
            upperItem++;
        }
        return matched;
    }

    void OnMouseOverItem(Item item)
    {
        if (_selectedItem == item)
        {
            _selectedItem = null;
            return;
        }
        if (_selectedItem == null)
        {
            _selectedItem = item;
        }
        else
        {
            float xDiff = Mathf.Abs(item.x - _selectedItem.x);
            float yDiff = Mathf.Abs(item.y - _selectedItem.y);
            if (Mathf.Abs(xDiff - yDiff) == 1)
            {
                // Try to swap items
                StartCoroutine(TryMatch(_selectedItem, item));
            }
            else
            {
                Debug.Log("This move is forbidden.");
            }
            _selectedItem = null;
        }
    }

    IEnumerator TryMatch(Item a, Item b)
    {
        yield return StartCoroutine(Swap(a, b)); // We do the swappingz

        MatchInfo matchA = GetMatch(a);
        MatchInfo matchB = GetMatch(b);

        Debug.Log(matchA.valid + " " + matchB.valid);
        if (!matchA.valid && !matchB.valid)
        {
            // Swap not resulted in a valid match, undo swap
            Debug.Log("Swap not valid");
            yield return StartCoroutine(Swap(a, b));
            yield break;
        }

        // if (matchA.valid)
        // {
        //     yield return StartCoroutine(DestroyItems(matchA.match));
        //     yield return new WaitForSeconds(delayBetweenMatches);
        //     yield return StartCoroutine(UpdateBoardIndices(matchA));
        // }
        // else if (matchB.valid)
        // {
        //     yield return StartCoroutine(DestroyItems(matchB.match));
        //     yield return new WaitForSeconds(delayBetweenMatches);
        //     yield return StartCoroutine(UpdateBoardIndices(matchB));
        // }
    }

    IEnumerator Swap(Item a, Item b)
    {
        StartCoroutine(a.transform.Move(b.transform.position, itemSwapTime));
        StartCoroutine(b.transform.Move(a.transform.position, itemSwapTime));

        ItemBase temp = a.Copy();
        UpdateItemPositions(a, b.x, b.y);
        UpdateItemPositions(b, temp.x, temp.y);
        yield return new WaitForSeconds(itemSwapTime);
    }

    void UpdateItemPositions(Item item, int x, int y)
    {
        _items[x, y] = item;
        item.SetPosition(x, y);
    }

    void OnDisable()
    {
        Item.OnMouseOverItemEventHandler -= OnMouseOverItem;
    }

}
