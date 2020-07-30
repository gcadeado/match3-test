﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    // public int height = 11;
    // public int width = 7;

    // public float tileSwapTime = 0.1f;
    // public float delayBetweenMatches = 0.1f;

    // public int mininumMatchSize = 3;

    // private GameObject[] _tiles; // Our reference to prefabs
    // private Item[,] _items; // Board items

    // private Item _selectedItem;

    // //Sets up the outer walls and floor (background) of the game board.
    // void BoardSetup()
    // {

    //     _items = new Item[width, height];

    //     //Loop along x axis.
    //     for (int x = 0; x < width; x++)
    //     {
    //         //Loop along y axis
    //         for (int y = 0; y < height; y++)
    //         {
    //             _items[x, y] = InstantiateDoddle(x, y);
    //         }
    //     }
    // }

    // void ClearBoard()
    // {
    //     for (int i = 0; i < width; i++)
    //     {
    //         for (int j = 0; j < height; j++)
    //         {
    //             MatchInfo matchInfo = GetMatchInfo(_items[i, j]);
    //             if (matchInfo.valid)
    //             {
    //                 Destroy(_items[i, j].gameObject);
    //                 _items[i, j] = InstantiateDoddle(i, j);
    //                 j--;
    //             }
    //         }
    //     }
    // }

    // Item InstantiateDoddle(int x, int y)
    // {
    //     //Choose a random tile from our array of tile prefabs and prepare to instantiate it.
    //     GameObject toInstantiate = _tiles[Random.Range(0, _tiles.Length)];

    //     //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop.
    //     Item newDoddle = ((GameObject)Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity)).GetComponent<Item>();

    //     //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
    //     newDoddle.transform.SetParent(this.transform);

    //     //Send change event to new doddle
    //     newDoddle.OnItemPositionChanged(x, y);

    //     return newDoddle;
    // }

    // void OnMouseOverItem(Item item)
    // {
    //     if (_selectedItem == item)
    //     {
    //         Debug.Log("Selected same item");
    //         _selectedItem = null;
    //         return;
    //     }
    //     if (_selectedItem == null)
    //     {
    //         _selectedItem = item;
    //     }
    //     else
    //     {
    //         float xDiff = Mathf.Abs(item.x - _selectedItem.x);
    //         float yDiff = Mathf.Abs(item.y - _selectedItem.y);
    //         if (Mathf.Abs(xDiff - yDiff) == 1)
    //         {
    //             // We can try to swap items
    //             StartCoroutine(TryMatch(_selectedItem, item));
    //         }
    //         else
    //         {
    //             Debug.Log("LOL cannot swap these.");
    //         }
    //         _selectedItem = null;
    //     }
    // }

    // IEnumerator TryMatch(Item a, Item b)
    // {
    //     yield return StartCoroutine(Swap(a, b)); // We do the swappingz

    //     MatchInfo matchA = GetMatchInfo(a);
    //     MatchInfo matchB = GetMatchInfo(b);

    //     if (!matchA.valid && !matchB.valid) // Swap not resulting in a valid match
    //     {
    //         yield return StartCoroutine(Swap(a, b)); // Undo the swap
    //         yield break; // Return
    //     }

    //     if (matchA.valid)
    //     {
    //         yield return StartCoroutine(DestroyItems(matchA.match));
    //         yield return new WaitForSeconds(delayBetweenMatches);
    //         yield return StartCoroutine(UpdateBoardIndices(matchA));
    //     }
    //     else if (matchB.valid)
    //     {
    //         yield return StartCoroutine(DestroyItems(matchB.match));
    //         yield return new WaitForSeconds(delayBetweenMatches);
    //         yield return StartCoroutine(UpdateBoardIndices(matchB));
    //     }
    // }

    // IEnumerator UpdateBoardIndices(MatchInfo match)
    // {
    //     if (match.matchStartY == match.matchEndY)
    //     {
    //         for (int i = match.matchStartX; i < match.matchEndX; i++)
    //         {
    //             for (int j = match.matchStartY; j < match.matchEndY; j++)
    //             {
    //                 Item upperIndex = _items[i, j + 1];
    //                 Item current = _items[i, j];
    //                 _items[i, j] = upperIndex;
    //                 _items[i, j + 1] = current;
    //                 _items[i, j].OnItemPositionChanged(_items[i, j].x, _items[i, j].y - 1);
    //                 _items[i, height - 1] = InstantiateDoddle(i, height - 1);
    //             }
    //         }
    //     }
    //     else if (match.matchStartX == match.matchEndX)
    //     {
    //         int matchHeight = (match.matchEndY - match.matchStartY) + 1;
    //         int currentX = match.matchStartX;
    //         for (int y = match.matchStartY + matchHeight; y < height - 1; y++)
    //         {

    //             Item lowerIndex = _items[currentX, y - matchHeight];
    //             Item current = _items[currentX, y];
    //             _items[currentX, y - matchHeight] = current;
    //             _items[currentX, y] = lowerIndex;
    //         }

    //         for (int y = 0; y < height - matchHeight; y++)
    //         {
    //             _items[currentX, y].OnItemPositionChanged(currentX, y);
    //         }

    //         for (int i = 0; i < match.match.Count; i++)
    //         {
    //             _items[currentX, height - 1 - i] = InstantiateDoddle(currentX, height - 1 - i);
    //         }
    //     }
    //     for (int x = 0; x < width; x++)
    //     {
    //         //Loop along y axis
    //         for (int y = 0; y < height; y++)
    //         {
    //             MatchInfo matchInfo = GetMatchInfo(_items[x, y]);
    //             if (match.valid)
    //             {
    //                 yield return StartCoroutine(DestroyItems(matchInfo.match));
    //                 yield return new WaitForSeconds(delayBetweenMatches);
    //                 yield return StartCoroutine(UpdateBoardIndices(matchInfo));
    //             }
    //         }
    //     }
    //     yield return null;
    // }

    // IEnumerator DestroyItems(List<Item> items)
    // {
    //     foreach (var item in items)
    //     {
    //         yield return StartCoroutine(item.transform.Scale(Vector3.zero, 0.1f));
    //         // Destroy(item.gameObject);
    //     }
    // }

    // IEnumerator Swap(Item a, Item b)
    // {
    //     SetPhysicsStatus(false);

    //     Vector3 aPos = a.transform.position; // We have to save this in order to move both simultaneously
    //     StartCoroutine(a.transform.Move(b.transform.position, 0.1f));
    //     StartCoroutine(b.transform.Move(aPos, 0.1f));

    //     yield return new WaitForSeconds(tileSwapTime);

    //     SwapIndices(a, b);
    //     SetPhysicsStatus(true);
    // }

    // void SwapIndices(Item a, Item b)
    // {
    //     Item tempA = _items[a.x, a.y];
    //     _items[a.x, a.y] = b;
    //     _items[b.x, b.y] = tempA;
    //     int bOldX = b.x; int bOldY = b.y;
    //     b.OnItemPositionChanged(a.x, a.y);
    //     a.OnItemPositionChanged(bOldX, bOldY);
    // }

    // public void SetPhysicsStatus(bool status)
    // {
    //     foreach (Item item in _items)
    //     {
    //         Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
    //         rb.isKinematic = !status;
    //     }
    // }

    // MatchInfo GetMatchInfo(Item item)
    // {
    //     MatchInfo m = new MatchInfo();
    //     List<Item> horizontalMatch = GetMatchHorizontally(item);
    //     List<Item> verticalMatch = GetMatchVertically(item);
    //     if (horizontalMatch.Count >= mininumMatchSize && horizontalMatch.Count >= verticalMatch.Count)
    //     {
    //         Debug.Log("Got a horizontal match");
    //         m.matchStartX = GetMinX(horizontalMatch);
    //         m.matchEndX = GetMaxX(horizontalMatch);
    //         m.matchStartY = m.matchEndY = horizontalMatch[0].y;
    //         m.match = horizontalMatch;
    //     }
    //     else if (verticalMatch.Count >= mininumMatchSize)
    //     {
    //         Debug.Log("Got a vertical match");
    //         m.matchStartY = GetMinY(horizontalMatch);
    //         m.matchEndY = GetMaxY(horizontalMatch);
    //         m.matchStartX = m.matchEndX = verticalMatch[0].x;
    //         m.match = verticalMatch;
    //     }

    //     return m;
    // }

    // int GetMinX(List<Item> items)
    // {
    //     float[] indices = new float[items.Count];
    //     for (int i = 0; i < indices.Length; i++)
    //     {
    //         indices[i] = items[i].x;
    //     }
    //     return (int)Mathf.Min(indices);
    // }

    // int GetMaxX(List<Item> items)
    // {
    //     float[] indices = new float[items.Count];
    //     for (int i = 0; i < indices.Length; i++)
    //     {
    //         indices[i] = items[i].x;
    //     }
    //     return (int)Mathf.Max(indices);
    // }

    // int GetMinY(List<Item> items)
    // {
    //     float[] indices = new float[items.Count];
    //     for (int i = 0; i < indices.Length; i++)
    //     {
    //         indices[i] = items[i].y;
    //     }
    //     return (int)Mathf.Min(indices);
    // }

    // int GetMaxY(List<Item> items)
    // {
    //     float[] indices = new float[items.Count];
    //     for (int i = 0; i < indices.Length; i++)
    //     {
    //         indices[i] = items[i].y;
    //     }
    //     return (int)Mathf.Max(indices);
    // }

    // List<Item> GetMatchHorizontally(Item item)
    // {
    //     Debug.Log("GetMatchHorizontally");
    //     List<Item> matched = new List<Item> { item };
    //     int leftItem = item.x - 1; // Imediatelly left item
    //     int rightItem = item.x + 1; // Imediatelly right item
    //     Debug.Log(string.Format("Current item id {0} with X position {1}", item.id, item.x));
    //     while (leftItem >= 0 && _items[leftItem, item.y].id == item.id)
    //     {
    //         matched.Add(_items[leftItem, item.y]);
    //         leftItem--;
    //     }
    //     while (rightItem < width && _items[rightItem, item.y].id == item.id)
    //     {
    //         matched.Add(_items[rightItem, item.y]);
    //         rightItem++;
    //     }
    //     Debug.Log(string.Format("Found {0} matched items", matched.Count));
    //     return matched;
    // }

    // List<Item> GetMatchVertically(Item item)
    // {
    //     List<Item> matched = new List<Item> { item };
    //     int lowerItem = item.y - 1; // Imediatelly lower item
    //     int upperItem = item.y + 1; // Imediatelly upper item
    //     Debug.Log(string.Format("Current item id {0} with Y position {1}", item.id, item.y));
    //     while (lowerItem >= 0 && _items[item.x, lowerItem].id == item.id)
    //     {
    //         matched.Add(_items[item.x, lowerItem]);
    //         lowerItem--;
    //     }
    //     while (upperItem < height && _items[item.x, upperItem].id == item.id)
    //     {
    //         matched.Add(_items[item.x, upperItem]);
    //         upperItem++;
    //     }
    //     Debug.Log(string.Format("Found {0} matched items", matched.Count));
    //     return matched;
    // }

    // void LoadDoddles()
    // {
    //     _tiles = Resources.LoadAll<GameObject>("Prefabs");
    //     for (int i = 0; i < _tiles.Length; i++)
    //     {
    //         _tiles[i].GetComponent<Item>().id = i;
    //         Debug.Log(string.Format("Loaded game object {0} with id {1}", _tiles[i].name, i));
    //     }
    //     Debug.Log(string.Format("Loaded {0} objects", _tiles.Length));
    // }

    // public void SetupScene()
    // {
    //     LoadDoddles();
    //     BoardSetup();
    //     ClearBoard();
    //     Item.OnMouseOverItemEventHandler += OnMouseOverItem;
    // }

    // void Start()
    // {
    //     SetupScene();
    // }
    // void OnDisable()
    // {
    //     // Unsubscribe events
    //     Item.OnMouseOverItemEventHandler -= OnMouseOverItem;
    // }
}
