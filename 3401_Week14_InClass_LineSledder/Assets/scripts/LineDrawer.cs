using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    public float minimumSegmentLength = 0.5f;

    private Vector3 previousMousePosition;
    private LineRenderer _currentLine;
    private List <GameObject> _undoBuffer;

    //Component - varibale type - name
    //public LineRenderer Line;
    //public EdgeCollider2D Edge; 

    void Start()
    {
        //Initializes a new list, weird af syntax but too bad
        _undoBuffer = new List<GameObject>();
    }

   
    void Update()
    {
        //Spawn line prefab 
        if (Input.GetMouseButtonDown(0))
        {
            _currentLine = Instantiate(linePrefab).GetComponent <LineRenderer> ();
            previousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        }
        //if current line exists, update it
        if(_currentLine != null)
        {
          //  Gets world position of mouse - has to start by saying which camera 
                Vector3 clickpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickpoint.z = 0;
            if (Vector3.Distance(previousMousePosition, clickpoint) > minimumSegmentLength)
            {
                //"how mnay points does this line have" adds 1 exponentially 
                    _currentLine.positionCount += 1;
                    _currentLine.SetPosition(_currentLine.positionCount - 1, clickpoint);

                //
                EdgeCollider2D currentEdge = _currentLine.gameObject.GetComponent<EdgeCollider2D>();
                Vector2[] edgePoints = new Vector2[_currentLine.positionCount];
                for (int i = 0; i < edgePoints.Length; i ++)
                {
                    edgePoints[i] = _currentLine.GetPosition(i);
                }
                //Assign collider points 
                currentEdge.points = edgePoints;


                previousMousePosition = clickpoint;

            }
        }

        //Stop drawing this shit 
        if (Input.GetMouseButtonUp (0))
        {
          
            if (_currentLine.positionCount <2 )
            {
                Destroy(_currentLine.gameObject);
            }
            else
            {
                _undoBuffer.Add(_currentLine.gameObject);
            }
            _currentLine = null;

        }

        //undo button
        if (Input.GetKeyDown (KeyCode.U))
        {
            Destroy(_undoBuffer[_undoBuffer.Count - 1]);
            _undoBuffer.RemoveAt(_undoBuffer.Count - 1); 
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //Gets world position of mouse - has to start by saying which camera 
        //    Vector3 clickpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    clickpoint.z = 0;

        //    //"how mnay points does this line have" adds 1 exponentially 
        //    Line.positionCount += 1;

        //    Line.SetPosition(Line.positionCount - 1, clickpoint);

        //    // the [] means it is an array (can store many varaibles in one unit), gpes through every point
        //    Vector2[] edgePoints = new Vector2[Line.positionCount];
        //    for (int i = 0; i < edgePoints.Length; i++)
        //    {
        //        edgePoints[i] = Line.GetPosition(i);
        //    }

        //    //set collider points!!! :P
        //    Edge.points = edgePoints;
        //}
    }
}
