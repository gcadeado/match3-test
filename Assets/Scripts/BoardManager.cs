using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    private GameObject[] _tiles; // Reference for loading our tile prefabs
    private Item[,] _items; // Reference to store which item is on each board position
    private Transform boardHolder; // Reference to the transform of our Board object
    private Item _selectedItem; // Player's current selected item

    [Header("Board settings")]
    public int height = 5; // Board height
    public int width = 5; // Board width

    [Tooltip("Minimum items for a valid match")]
    public int minMatch = 3;

    [Tooltip("Score value for each item in a match")]
    public int itemScore = 60;

    public float itemSwapTime = 0.1f; // Time to swap items animation

    public float delayBetweenMatches = 0.2f; // Wait time after a match

    public float gravity = 9.8f;

    [Header("Scriptable Objects Architecture")]

    [SerializeField]
    private IntVariable scoreObject = null; // Reference to score Scriptable Object

    [Header("Audio")]
    [SerializeField]
    private AudioManager audioManager = null;

    [SerializeField]
    private Sound clearSFX = null;

    [SerializeField]
    private Sound selectSFX = null;

    [SerializeField]
    private Sound swapSFX = null;

    private AudioSourcePlayer audioPlayer = null;

    [Header("Info")]

    [ReadOnly]
    public bool canPlay = true; // Setup board and instantiate items

    // Setup board and instantiate items
    void BoardSetup()
    {

        audioPlayer = AudioSourcePlayer.AddAsComponent(gameObject, audioManager);

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

    void RepositionItems()
    {
        for (int x = 0; x < width; x++)
        {
            //Loop along y axis
            for (int y = 0; y < height; y++)
            {
                Item current = _items[x, y];
                current.transform.position = new Vector3(current.x, current.y, 0f);
            }
        }
    }


    // Shuffle board using Fisher-Yates algorithm
    public void ShuffleBoard(System.Random random)
    {
        // Get 2d array dimensions
        int num_rows = _items.GetUpperBound(0) + 1;
        int num_cols = _items.GetUpperBound(1) + 1;
        int num_cells = num_rows * num_cols;

        // Randomize the array.
        for (int i = 0; i < num_cells - 1; i++)
        {
            // Pick a random cell between i and the end of the array
            int j = random.Next(i, num_cells);

            // Convert to row/column indexes
            int row_i = i / num_cols;
            int col_i = i % num_cols;
            int row_j = j / num_cols;
            int col_j = j % num_cols;

            // Swap cells i and j
            Item temp = _items[row_i, col_i];
            _items[row_i, col_i] = _items[row_j, col_j];
            _items[row_j, col_j] = temp;
            SwapIndices(_items[row_i, col_i], _items[row_j, col_j]);
            RepositionItems(); // Reposition all items gameObject positions with the new ones
        }
    }

    Item InstantiateDoddle(int x, int y, int offsetX = 0, int offsetY = 0)
    {
        //Choose a random tile from our _items of tile prefabs and prepare to instantiate it.
        GameObject toInstantiate = _tiles[Random.Range(0, _tiles.Length)];

        //Instantiate the GameObject instance using the prefab chosen for to Instantiate at the Vector3 corresponding to current grid position in loop.
        Item newDoddle = ((GameObject)Instantiate(toInstantiate, new Vector3(x + offsetX, y + offsetY, 0f), Quaternion.identity)).GetComponent<Item>();

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


    // Clean initial matches as we don't want matches on round start
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
        LoadTiles(); // Load tile resources prefabs
        BoardSetup(); // Setup item array and intantiate doddles
        SweepBoard(); // Remove possible matches at round start

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

    // Get horizontal matched items
    List<Item> GetMatchHorizontal(Item item)
    {
        List<Item> matched = new List<Item> { item }; // List for matched items
        int leftItem = item.x - 1;  // Imediatelly left item
        int rightItem = item.x + 1; // Imediatelly right item

        // Starting from the swapped item, we search recursively for the left items with same id until we found a different item
        while (leftItem >= 0 && _items[leftItem, item.y].id == item.id)
        {
            matched.Add(_items[leftItem, item.y]); // Add matched item to the list
            leftItem--;
        }
        // Same logic for right items
        while (rightItem < width && _items[rightItem, item.y].id == item.id)
        {
            matched.Add(_items[rightItem, item.y]);
            rightItem++;
        }
        return matched;
    }

    // Get horizontal matched items
    List<Item> GetMatchVertical(Item item)
    {
        List<Item> matched = new List<Item> { item }; // List for matched items
        int lowerItem = item.y - 1; // Imediatelly lower item
        int upperItem = item.y + 1; // Imediatelly upper item

        // Starting from the swapped item, we search recursively for the lower items with same id until we found a different item
        while (lowerItem >= 0 && _items[item.x, lowerItem].id == item.id)
        {
            matched.Add(_items[item.x, lowerItem]);
            lowerItem--;
        }
        // Same logic for upper items
        while (upperItem < height && _items[item.x, upperItem].id == item.id)
        {
            matched.Add(_items[item.x, upperItem]);
            upperItem++;
        }
        return matched;
    }

    void OnMouseOverItem(Item item)
    {
        // If selected item is already select or player cannot play during animations, game setup, etc
        if (_selectedItem == item || !canPlay)
        {
            _selectedItem = null; // Unselect item
            SetSelectItems(false, item); // Remove select status from item
            return;
        }
        if (_selectedItem == null)
        {
            _selectedItem = item; // There is no other item selected, select this one
            SetSelectItems(true, item); // Update item select status
            audioPlayer.PlaySound(selectSFX); // Play sound
        }
        else
        {
            // We have 2 selected items
            Vector3 itemPos = new Vector3(item.x, item.y, 0);
            Vector3 selectedPos = new Vector3(_selectedItem.x, _selectedItem.y, 0); // Previously selected item

            // Check if selected is in permited radius (the neighbors always has distance 1)
            if ((itemPos - selectedPos).magnitude == 1)
            {
                // Try to swap items
                StartCoroutine(TryMatch(_selectedItem, item));
            }
            else
            {
                Debug.Log("This move is forbidden.");
            }
            SetSelectItems(false, item, _selectedItem); // Set items select status to false
            _selectedItem = null;
        }
    }

    IEnumerator TryMatch(Item a, Item b)
    {
        canPlay = false;
        audioPlayer.PlaySound(swapSFX);

        SwapIndices(a, b);
        yield return StartCoroutine(Swap(a, b));

        MatchInfo matchA = GetMatch(a);
        MatchInfo matchB = GetMatch(b);

        if (!matchA.valid && !matchB.valid)
        {
            // Swap not resulted in a valid match, undo swap
            Debug.Log("This swap is not a valid match.");
            SetSelectItems(false, a, b);
            SwapIndices(a, b);
            yield return StartCoroutine(Swap(a, b));
            canPlay = true;
            yield break;
        }

        if (matchA.valid)
        {
            AddScore(matchA.match.Count);
            SetSelectItems(false, a, b);
            StartCoroutine(DestroyMatch(matchA.match));
            yield return StartCoroutine(UpdateBoardIndices(matchA));
            DestroyMatchObjects(matchA.match);
            yield return new WaitForSeconds(delayBetweenMatches);
        }
        else if (matchB.valid)
        {
            AddScore(matchB.match.Count);
            SetSelectItems(false, a, b);
            StartCoroutine(DestroyMatch(matchB.match));
            yield return StartCoroutine(UpdateBoardIndices(matchB));
            DestroyMatchObjects(matchB.match);
            yield return new WaitForSeconds(delayBetweenMatches);
        }

        yield return StartCoroutine(CheckForMatches());

        canPlay = true;
    }


    // Swap item indices
    void SwapIndices(Item a, Item b)
    {
        int tempX = a.x;
        int tempY = a.y;
        UpdateItemPositions(a, b.x, b.y);
        UpdateItemPositions(b, tempX, tempY);
    }

    // Actual swap animation
    IEnumerator Swap(Item a, Item b)
    {
        StartCoroutine(a.transform.Move(b.transform.position, itemSwapTime));
        StartCoroutine(b.transform.Move(a.transform.position, itemSwapTime));
        yield return new WaitForSeconds(itemSwapTime);
    }

    // Set items select status
    void SetSelectItems(bool selected = true, params Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetSelected(selected);
        }
    }

    // Animation to vanish items
    IEnumerator DestroyMatch(List<Item> items)
    {
        foreach (var item in items)
        {
            StartCoroutine(item.transform.Scale(Vector3.zero, 0.1f));
        }
        audioPlayer.PlaySound(clearSFX);
        yield return null;
    }

    // Actually destroy items' gameObject
    void DestroyMatchObjects(List<Item> items)
    {
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
    }

    void AddScore(int matchSize)
    {
        // The match score is given by individual item score multiplied by match size
        scoreObject.Value += matchSize * itemScore;
    }

    void UpdateItemPositions(Item item, int x, int y)
    {
        _items[x, y] = item;
        item.SetPosition(x, y);
    }

    IEnumerator UpdateBoardIndices(MatchInfo match)
    {
        int minX = match.GetMinX(); // Minimum match horizontal position
        int maxX = match.GetMaxX(); // Maximum match horizontal position
        int minY = match.GetMinY(); // Minimum match vertical position
        int maxY = match.GetMaxY(); // Maximum match vertical position

        List<Item> fallingItems = new List<Item> { }; // List to hold items that must fall

        if (minY == maxY) // Horizontal match, we have to update several columns (for the remaining upper items)
        {
            for (int i = minX; i <= maxX; i++) //Loop through horizontal positions
            {
                for (int j = minY; j < height - 1; j++) //Loop through vertical positions above match
                {
                    Item upperIndex = _items[i, j + 1];
                    Item current = _items[i, j];

                    // Do the swappingz
                    _items[i, j] = upperIndex;
                    _items[i, j + 1] = current;
                    _items[i, j].SetPosition(_items[i, j].x, _items[i, j].y - 1);
                    fallingItems.Add(_items[i, j]); // Set the item to fall
                }

                // Fill empty space with new random item
                _items[i, height - 1] = InstantiateDoddle(i, height - 1, 0, 1);
                Item newItem = _items[i, height - 1];
                fallingItems.Add(newItem); // Fall this new item
            }
        }
        else if (minX == maxX) // Vertical match, we have to update just one column
        {
            int matchHeight = (maxY - minY) + 1;
            int currentX = minX;
            for (int j = minY + matchHeight; j <= height - 1; j++) // Loop throught vertical positions above match
            {
                Item lowerIndex = _items[currentX, j - matchHeight];
                Item current = _items[currentX, j];

                //Do the swappingz
                _items[currentX, j - matchHeight] = current;
                _items[currentX, j] = lowerIndex;
            }

            for (int y = 0; y < height - matchHeight; y++) // Update all items on column
            {
                _items[currentX, y].SetPosition(currentX, y);
                fallingItems.Add(_items[currentX, y]);
            }
            for (int i = 0; i < match.Count; i++) // Loop through match count
            {
                int newYPos = (height - 1) - i; // Vertical position for the new item
                _items[currentX, newYPos] = InstantiateDoddle(currentX, newYPos, 0, match.Count); // Instantiate new item
                fallingItems.Add(_items[currentX, newYPos]); // Set new item to fall
            }
        }

        yield return StartCoroutine(FallItems(fallingItems)); // Fall all items setted to fall and waits for finish

        CheckForMatches(); // Check for more matches after update

        bool hasPossibleMoves = CheckForPossibleMoves();
        if (!hasPossibleMoves)
        {
            Debug.Log("No more possible moves. Shuffling board.");
            ShuffleBoard(new System.Random());
        };

        yield return null;
    }

    IEnumerator FallItems(List<Item> items)
    {
        foreach (var item in items)
        {
            StartCoroutine(item.Fall(new Vector3(item.x, item.y, 0.0f), gravity)); // Fall item to position
        }

        // Keep on this coroutine until all items has finished falling
        bool hasFallingItems = true;
        while (hasFallingItems)
        {
            yield return null;
            hasFallingItems = false;
            foreach (var item in items)
            {
                hasFallingItems = hasFallingItems || item.isFalling;
            }
        }
        yield return null;
    }

    // Check for matches on entire board
    IEnumerator CheckForMatches()
    {
        //Loop along x axis
        for (int x = 0; x < width; x++)
        {
            //Loop along y axis
            for (int y = 0; y < height; y++)
            {
                MatchInfo matchInfo = GetMatch(_items[x, y]);
                if (matchInfo.valid)
                {
                    AddScore(matchInfo.match.Count);
                    StartCoroutine(DestroyMatch(matchInfo.match)); // Destroy animation
                    yield return StartCoroutine(UpdateBoardIndices(matchInfo)); // Update
                    DestroyMatchObjects(matchInfo.match); // Destroy gameObjects

                    yield return new WaitForSeconds(delayBetweenMatches); // Wait before next check
                }
            }
        }
    }

    // Check for possible moves on entire board
    bool CheckForPossibleMoves()
    {

        MatchInfo matchA;
        MatchInfo matchB;
        List<List<Item>> possibleSwaps = new List<List<Item>>(); // TODO: Saving and return the first swap that gives a match for later reuse (eg. hint for moves)
        for (int x = 0; x < width - 1; x++)
        {
            //Loop along y axis
            for (int y = 0; y < height - 1; y++)
            {
                Item current = _items[x, y];
                Item upperItem = _items[x, y + 1];
                Item rightItem = _items[x + 1, y];

                // We only have to check for righty items (as for the left items was already checked before)
                SwapIndices(current, rightItem); // As we dont want to make the actual swap, we only swap board positions for checking
                matchA = GetMatch(current);
                matchB = GetMatch(rightItem);
                if (matchA.valid || matchB.valid)
                {
                    possibleSwaps.Add(new List<Item> { current, rightItem });
                }
                SwapIndices(current, rightItem); // Swap back as we dont want the actual swap

                // We only have to check for upper items (as for the lower items was already checked before)
                SwapIndices(current, upperItem);
                matchA = GetMatch(current);
                matchB = GetMatch(upperItem);
                if (matchA.valid || matchB.valid)
                {
                    possibleSwaps.Add(new List<Item> { current, upperItem });
                }
                SwapIndices(current, upperItem); // Swap back as we dont want the actual swap
            }
        }
        Debug.Log("Possible moves left: " + possibleSwaps.Count);
        return possibleSwaps.Count > 0;
    }
    void OnDisable()
    {
        Item.OnMouseOverItemEventHandler -= OnMouseOverItem; // Remove subscription callback
    }

}
