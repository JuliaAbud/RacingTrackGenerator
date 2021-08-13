using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/

public class IntersectionDetection : MonoBehaviour
{
    /*void Start(){
        Vector3[] pts = {new Vector3(0,0,0),
                        new Vector3(1,0,1),
                        new Vector3(2,0,4)
                        };
        int i = PathIntersections(pts);
        print(i);
    }*/

    public int PathIntersections(Vector3[] pathPoints){
        int intersectNum = 0;
        for (int i=0;i<pathPoints.Length-1;i++){
            Vector3 segment1_p1 = pathPoints[i];
            Vector3 segment1_p2 = pathPoints[i+1];
            for (int j=i+1;j<pathPoints.Length;j++){
                Vector3 segment2_p1 = pathPoints[j];
                Vector3 segment2_p2;
                if(j!=pathPoints.Length-1)
                    segment2_p2 = pathPoints[j+1];
                else
                    segment2_p2 = pathPoints[0];

                //Segments intersection
                Point p1 = new Point(segment1_p1.x, segment1_p1.z);
                Point q1 = new Point(segment1_p2.x, segment1_p2.z);
                Point p2 = new Point(segment2_p1.x, segment2_p1.z);
                Point q2 = new Point(segment2_p2.x, segment2_p2.z);

                if((q1.x==p2.x && q1.y==p2.y) || (p1.x==q2.x && p1.y==q2.y)){
                }
                else{
                    if(doIntersect(p1, q1, p2, q2)){
                        //Debug.Log("Yes");
                        intersectNum +=1;
                    }
                    else{
                        //Debug.Log("No");
                    }
                }
            }
        }
        return intersectNum;
    }        

    public class Point
    {
        public float x;
        public float y;
    
        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        
    };
    
    // Given three colinear points p, q, r, the function checks if
    // point q lies on line segment 'pr'
    static bool onSegment(Point p, Point q, Point r)
    {
        if (q.x <= Mathf.Max(p.x, r.x) && q.x >= Mathf.Min(p.x, r.x) &&
            q.y <= Mathf.Max(p.y, r.y) && q.y >= Mathf.Min(p.y, r.y))
        return true;
    
        return false;
    }
    
    // To find orientation of ordered triplet (p, q, r).
    // The function returns following values
    // 0 --> p, q and r are colinear
    // 1 --> Clockwise
    // 2 --> Counterclockwise
    static int orientation(Point p, Point q, Point r)
    {
        // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
        // for details of below formula.
        float val = (q.y - p.y) * (r.x - q.x) -
                (q.x - p.x) * (r.y - q.y);
    
        if (val == 0) return 0; // colinear
    
        return (val > 0)? 1: 2; // clock or counterclock wise
    }
    
    // The main function that returns true if line segment 'p1q1'
    // and 'p2q2' intersect.
    static bool doIntersect(Point p1, Point q1, Point p2, Point q2)
    {
        // Find the four orientations needed for general and
        // special cases
        int o1 = orientation(p1, q1, p2);
        int o2 = orientation(p1, q1, q2);
        int o3 = orientation(p2, q2, p1);
        int o4 = orientation(p2, q2, q1);
    
        // General case
        if (o1 != o2 && o3 != o4)
            return true;
    
        // Special Cases
        // p1, q1 and p2 are colinear and p2 lies on segment p1q1
        if (o1 == 0 && onSegment(p1, p2, q1)) return true;
    
        // p1, q1 and q2 are colinear and q2 lies on segment p1q1
        if (o2 == 0 && onSegment(p1, q2, q1)) return true;
    
        // p2, q2 and p1 are colinear and p1 lies on segment p2q2
        if (o3 == 0 && onSegment(p2, p1, q2)) return true;
    
        // p2, q2 and q1 are colinear and q1 lies on segment p2q2
        if (o4 == 0 && onSegment(p2, q1, q2)) return true;
    
        return false; // Doesn't fall in any of the above cases
    }
}
