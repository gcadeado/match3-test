using System.Collections;
using UnityEngine;
using UnityEditor;

public class Item : MonoBehaviour
{
    public int x; // Horizontal position

    public int y; // Vertical position

    private Vector2 finalTouchPosition;

    public int id;

    public bool isFalling; // Referece to check if object is falling

    public Color defaultColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color selectedColor = new Color(0.7f, 0.7f, 0.7f, 0.9f);

    SpriteRenderer sp;

    void Start()
    {
        //Fetch the SpriteRenderer from the GameObject
        sp = GetComponent<SpriteRenderer>();

        //Set the GameObject's Color quickly to a set Color (blue)
        sp.color = defaultColor;
    }


    public void SetPosition(int newX, int newY)
    {
        // Set new position
        x = newX;
        y = newY;

        // Set object name
        gameObject.name = string.Format("[{0}][{1}]", x, y);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            sp.color = selectedColor;
        }
        else
        {
            sp.color = defaultColor;
        }
    }

    public IEnumerator Fall(Vector3 target, float gravity)
    {
        if (transform == null) { yield break; }; // Fix null reference when object is already destroyed
        isFalling = true;
        Vector3 velocity = Vector3.zero;

        while (transform.position.y > target.y)
        {
            yield return null;
            if (transform == null) { yield break; }; // Fix null reference when object is already destroyed
            velocity = velocity + new Vector3(0f, -gravity, 0f) * Time.deltaTime;
            transform.position = transform.position + velocity * Time.deltaTime;
        }

        // Make sure that it snaps to the end position.
        transform.position = target;
        isFalling = false;
    }

    void OnMouseUp()
    {
        // We get the camera to cancel the Z vector and ScreenToWorldPoint returns distances in the same Z value
        float cameraZPos = Camera.main.transform.position.z;
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, -cameraZPos));

        if (OnMouseOverItemEventHandler != null)
        {
            SwipeDirection direction = InputHelpers.GetSwipeDirection(new Vector2(x, y), finalTouchPosition, 0.5f);
            OnMouseOverItemEventHandler(this, direction);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Draw object position and ID on screen
        Handles.Label(transform.position, string.Format("[{0}][{1}]\nID: {2}", x, y, id));
    }
#endif

    public delegate void OnMouseOverItem(Item item, SwipeDirection direction);
    public static event OnMouseOverItem OnMouseOverItemEventHandler;
}
