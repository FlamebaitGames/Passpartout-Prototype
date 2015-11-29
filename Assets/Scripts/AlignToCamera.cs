using UnityEngine;
using System.Collections;

public class AlignToCamera : MonoBehaviour {
    private Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = transform.localPosition;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

        Vector3 planeOrigin = Camera.main.transform.position + Camera.main.transform.forward * 3.0f;

        Vector3 intersect;

        Vector3 lineOrigin = transform.parent.position + offset;
        Vector3 lineDir = (Camera.main.transform.position - lineOrigin).normalized;
        Vector3 planeNormal = Camera.main.transform.forward;

        Debug.DrawRay(lineOrigin, lineDir * 10.0f, Color.red, 50.0f);
        if (LinePlaneIntersection(out intersect, lineOrigin, lineDir, planeNormal, planeOrigin))
        {
            transform.position = intersect;
        }
	}


    public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec, Vector3 planeNormal, Vector3 planePoint)
    {

        float length;
        float dotNumerator;
        float dotDenominator;
        Vector3 vector;
        intersection = Vector3.zero;

        //calculate the distance between the linePoint and the line-plane intersection point
        dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
        dotDenominator = Vector3.Dot(lineVec, planeNormal);

        //line and plane are not parallel
        if (dotDenominator != 0.0f)
        {
            length = dotNumerator / dotDenominator;

            //create a vector from the linePoint to the intersection point
            vector = lineVec.normalized * length;

            //get the coordinates of the line-plane intersection point
            intersection = linePoint + vector;

            return true;
        }

        //output not valid
        else
        {
            return false;
        }
    }
}
