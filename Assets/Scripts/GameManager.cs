using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool MoveByTouch, StartGame;
    [SerializeField] private float roadSpeed, swipeSpeed, distance;
    [SerializeField] private GameObject road;
    private Camera _cam;
    private Vector3 mouseStartPos, playerStartPos;
    public List<Transform> Balls = new List<Transform>();
    public static GameManager Instance;
    public GameObject newBall;
    //public ParticleSystem explosionParticle;
    
    void Start()
    {
        Instance = this;
        _cam = Camera.main;
        Balls.Add(gameObject.transform);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGame = MoveByTouch = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            MoveByTouch = false;
        }

        if (MoveByTouch)
        {
            var plane = new Plane(Vector3.up, 0f);
            float distance;
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out distance))
            {
                 Vector3 mousePos = ray.GetPoint(distance);
                 Vector3 desiredPos = mousePos - mouseStartPos;
                 Vector3 move = playerStartPos + desiredPos;
                 
                 move.x = Mathf.Clamp(move.x, 0.2f, 4.4f);
                 move.z = -7f;
                 
                 var player = transform.position;
                 player = new Vector3(Mathf.Lerp(player.x, move.x, Time.deltaTime * (swipeSpeed + 10f)), player.y, player.z);
                 transform.position = player;
                 
            }
        }

        if (StartGame)
        {
            road.transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        }

        if (Balls.Count > 1)
        {
            for (int i = 1; i < Balls.Count; i++)
            {
                var FirstBall = Balls.ElementAt(i - 1);
                var SectBall = Balls.ElementAt(i);
                
                SectBall.position = new Vector3(Mathf.Lerp(SectBall.position.x, FirstBall.position.x, swipeSpeed * Time.deltaTime), 
                    SectBall.position.y, Mathf.Lerp(SectBall.position.z, FirstBall.position.z + 0.5f, swipeSpeed * Time.deltaTime));
            }
        }
    }

    private void LateUpdate()
    {
        if (StartGame)
        {
            _cam.transform.position = new Vector3(Mathf.Lerp(_cam.transform.position.x, transform.position.x,
                (swipeSpeed - 5f) * Time.deltaTime), _cam.transform.position.y, _cam.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blue"))
        {
            other.transform.parent = null;
            other.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            other.gameObject.GetComponent<Collider>().isTrigger = true;
            other.gameObject.AddComponent<StackManager>();
            other.tag = gameObject.tag;
            other.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
            Balls.Add(other.transform);
        }

        if (other.CompareTag("add"))
        {
            var Add = Int16.Parse(other.transform.GetChild(0).name);

            for (int i = 0; i < Add; i++)
            {
                GameObject Ball = Instantiate(newBall, Balls.ElementAt(Balls.Count - 1).position + 
                    new Vector3(0f, 0f, 0.5f), Quaternion.identity);
                Balls.Add(Ball.transform);
            }
        }

        if (other.CompareTag("sub") && Balls.Count > 0)
        {
            //Instantiate(explosionParticle, Balls.ElementAt(Balls.Count - 1).position, Quaternion.identity);
            Balls.ElementAt(Balls.Count -1).gameObject.SetActive(false);
            Balls.RemoveAt(Balls.Count -1);
        }

        if (Balls.Count == 0)
        {
            StartGame = false;
        }
    }
}