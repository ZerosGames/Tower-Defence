using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

    public float panSpeed = 30f;
    public float mousePanBorder = 100f;
    public float scrollSpeed = 5f;

    public int minY = 0;
    public int maxY = 0;

    public int minDistanceFromMap = 30;
    public int MaxDistanceFromMap = 80;

    private int mapWidth;
    private int mapHeight;

    private Vector3 CameraPosOnMap;

    Vector3 lastPosition;

    void Start()
    {
        mapHeight = GameManager.gameManager.GetMapHeight();
        mapWidth = GameManager.gameManager.GetMapWidth();
    }

	// Update is called once per frame
	void Update ()
    {
        Vector3 camPos = transform.position;

        Vector3 NewCamPos = camPos;

        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

        if (!screenRect.Contains(Input.mousePosition))
            return;

        if (Input.GetKey("w") || Input.mousePosition.y >= (Screen.height - mousePanBorder))
        {
            NewCamPos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= mousePanBorder)
        {
            NewCamPos.z -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= mousePanBorder)
        {
            NewCamPos.x -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= (Screen.width - mousePanBorder))
        {
            NewCamPos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(2))
        {
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            delta.Normalize();

            NewCamPos.x += -delta.x * panSpeed * Time.deltaTime;
            NewCamPos.z += -delta.y * panSpeed * Time.deltaTime;
            lastPosition = Input.mousePosition;
        }

        CameraPosOnMap = GetCameraPointOnMap();

        minY = (int)CameraPosOnMap.y + minDistanceFromMap;
        maxY = (int)CameraPosOnMap.y + MaxDistanceFromMap;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            NewCamPos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        }

        NewCamPos.y = Mathf.Clamp(NewCamPos.y, minY, maxY);
        NewCamPos.x = Mathf.Clamp(NewCamPos.x, -mapWidth, (mapWidth) - Camera.main.fieldOfView - 2);
        NewCamPos.z = Mathf.Clamp(NewCamPos.z, -mapHeight, (mapHeight) - Camera.main.fieldOfView - 2);

        Vector3 LerpedMovement = Vector3.Lerp(camPos, NewCamPos, 0.5f);

        transform.position = LerpedMovement;
    }

    public Vector3 GetCameraPointOnMap()
    {
        Vector3 CameraPointOnMap = new Vector3();

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(ray, out hit, 1000, 1 << 9))
        {
            CameraPointOnMap = hit.point;
        }

        return CameraPointOnMap;
    }
}
