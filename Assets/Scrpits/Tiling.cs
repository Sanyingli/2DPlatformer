using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

    public int offsetX = 2;   // offset make sure carmer border dooesnt across the foreground border

    public bool hasARightBuddy = false;//check if we need instantiate buddy
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;//used if the object it not tileable

    private float spriteWidth = 0f;// the width of element
    private Camera cam;
    private Transform mtransform;

    void Awake()
    {
        cam = Camera.main;
        mtransform = transform;
    }
	// Use this for initialization
	void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {

        //check if need buddy, if not do nothing
        if (hasALeftBuddy == false || hasARightBuddy == false)
        {   
            //calculate the camras extand(half the width) of what the camera can see in world coordinates
            float camHorziontalExtand = cam.orthographicSize * Screen.width / Screen.height;

            //calcuate the x postition where the camera can see the edge of the sprite(element)
            float edgeVisiblePositionRight = (mtransform.position.x + spriteWidth / 2) - camHorziontalExtand;
            float edgeVisiblePositionLeft = (mtransform.position.x - spriteWidth / 2) + camHorziontalExtand;

            //check if we can see the edge of the element and then calling MakeNewBuddy if we can
            if(cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
	
	}

    //function that create a buddy on the side required
    void MakeNewBuddy(int rightOrLeft)
    {
        //the new position for new buddy
        Vector3 newPosition = new Vector3(mtransform.position.x + spriteWidth * rightOrLeft, mtransform.position.y, mtransform.position.z);
        //instantiating new buddy and storing it in a variable
        Transform newBuddy = Instantiate(mtransform,newPosition,mtransform.rotation) as Transform;

        //if not tilable, reverse the x size 
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }
        newBuddy.parent = mtransform.parent;

        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
