using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

    public Transform[] background;  //array list of all the back and foregorunds to be parallaxed
    private float[] paralaxScales;  //the proportion of the camera's movement to move the backgrounds by.
    public float smoothing = 1f;     //how smooth the parallax is going to be. Make sure to set this above 0.

    private Transform cam;          //reference to the main cameras transform;
    private Vector3 previousCamPos; //the camera's position of the previous frame

    void Awake() //Great for references                   
    {
        //ser up the camera the references
        cam = Camera.main.transform;
    }
	// Use this for initialization
	void Start () {
        //The previous frame had the current frame's camera position
        previousCamPos = cam.position;

        //asigning coresponding parallaxScales
        paralaxScales = new float[background.Length];
        for (int i = 0; i < background.Length; i++)
        {
            paralaxScales[i] = background[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    //for each background
        for (int i = 0; i < background.Length; i++)
        {
            //the parallax is the opposite of the camera movement, beacuse the previous frame muliplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * paralaxScales[i];
            //set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = background[i].position.x + parallax;
            //create a target position which is the backround's current postion with it's target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, background[i].position.y, background[i].position.z);
            //fade between current position and the target postion using lerp
            background[i].position = Vector3.Lerp(background[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        //set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
	}
}
