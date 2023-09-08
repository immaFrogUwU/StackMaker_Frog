using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [Header("Chiều cao ban đầu của người chơi")]
    [SerializeField] float beginHeight;

    public PlayerDirection CurrentPlayerDirection;
    public float speed;
    public Vector2 beginMousePosition;
    public Vector2 lastMousePosition;
    public LayerMask layersToHit;
    public Ray ray;
    public Collider colliderToHit;

    private bool isMoving = false;
    private List<GameObject> bricks = new List<GameObject>();
    private float maxDistance = 0.1f;
    private Direction direction;

    private void Start()
    {
        isMoving = false;
        CheckForColliders();
    }


    //cho những object trước mặt vào 1 list, nếu đụng phải tường thì stop

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            CalculateSwipeDirection();
            CheckForColliders();
            return;
        }
        GetInputBySwipe();
        
    }

    Vector3 GetVectorDirectionFromEnum()
    {
        if(CurrentPlayerDirection == PlayerDirection.left)
        {
            return Vector3.left;
        }
        else if(CurrentPlayerDirection == PlayerDirection.right)
        {
            return Vector3.right;
        }
        else if(CurrentPlayerDirection == PlayerDirection.foward)
        {
            return Vector3.forward;
        }
        return Vector3.back;
    }

    Vector3 GetVectorBackDirectionFromEnum()
    {
        if (CurrentPlayerDirection == PlayerDirection.left)
        {
            return Vector3.right;
        }
        else if (CurrentPlayerDirection == PlayerDirection.right)
        {
            return Vector3.left;
        }
        else if (CurrentPlayerDirection == PlayerDirection.foward)
        {
            return Vector3.back;
        }
        return Vector3.forward;
    }

    private void CheckForColliders()
    {
        RaycastHit hit;

        Vector3 targetRayStartPosition = new Vector3(transform.position.x, beginHeight +1f, transform.position.z);
        Vector3 targetRayEndPosition = new Vector3(transform.position.x, beginHeight, transform.position.z);
        targetRayEndPosition += GetVectorDirectionFromEnum();
        targetRayStartPosition += GetVectorDirectionFromEnum();
        Debug.DrawLine(targetRayStartPosition,transform.TransformDirection(GetVectorDirectionFromEnum()), Color.red);
        Physics.Raycast(targetRayStartPosition, targetRayEndPosition - targetRayStartPosition, out hit, 1f, layersToHit);

        if (hit.collider != null)
        {       
            Vector3 targetPosition = hit.collider.transform.position;
            targetPosition.y = transform.position.y;
            StartCoroutine(WaitForFit(targetPosition + GetVectorBackDirectionFromEnum()));
        }
    }

    IEnumerator WaitForFit(Vector3 targetPosition)
    {
        while(Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            yield return null;
        }
        rb.velocity = Vector3.zero;
        transform.position = targetPosition;
        isMoving = false;
    }


    private void GetInputBySwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            beginMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMoving = true;
            lastMousePosition = Input.mousePosition;
            CalculateSwipeDirection();
        }
    }
    private void CalculateSwipeDirection() /// lệnh tính toán mỗi khi nhấc chuột trái lên
    {
        float dentaX = lastMousePosition.x - beginMousePosition.x; //tren truc Ox, lay diem cuoi khi tha chuot - diem dau tien nhan chuot
        float dentaY = lastMousePosition.y - beginMousePosition.y; //tuong tu Oy
        float angleAlpha = Mathf.Atan2(dentaY, dentaX) * Mathf.Rad2Deg; // Use Mathf.Rad2Deg to convert from radians to degrees
        if (angleAlpha > -45 && angleAlpha < 45)
        {
            //phải
            CurrentPlayerDirection = PlayerDirection.right;
            rb.velocity = Vector3.right * speed * Time.deltaTime; // Use transform.Translate to move by a direction and distance
        }
        if (angleAlpha > 135 && angleAlpha < -135)
        {
            CurrentPlayerDirection = PlayerDirection.left;
            rb.velocity = Vector3.left * speed * Time.deltaTime;
        }
        if (angleAlpha < -45 && angleAlpha > -135)
        {
            CurrentPlayerDirection = PlayerDirection.back;
            rb.velocity = Vector3.back * speed * Time.deltaTime;
        }
        if (angleAlpha > 45 && angleAlpha < 135)
        {
            CurrentPlayerDirection = PlayerDirection.foward;
            rb.velocity = Vector3.forward * speed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        { 
            other.transform.SetParent(this.transform);
            transform.position += Vector3.up * 0.5f;
            bricks.Add(other.gameObject);
            other.tag = "Player";
            other.transform.localPosition = new Vector3(0, -0.5f * bricks.Count, 0);
        }
        
        if (other.CompareTag("UnBrick"))
        {
            Destroy(bricks[bricks.Count - 1]);
            bricks.RemoveAt(bricks.Count - 1);
            other.tag = "Untagged";
            transform.position -= Vector3.up * 0.5f;

        }

    }
}

public enum PlayerDirection
{
    left = 0,
    right = 1,
    foward = 2,
    back = 3,
}



