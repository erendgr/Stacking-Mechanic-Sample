using System;
using System.Linq;
using UnityEngine;

public class StackManager : MonoBehaviour
{
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
            GameManager.Instance.Balls.Add(other.transform);
        }
        
        if (other.CompareTag("add"))
        {
            if (!other.GetComponent<Collider>().enabled) return;
            
            var Add = Int16.Parse(other.transform.GetChild(0).name);
            Debug.Log(Add);
            for (int i = 0; i < Add; i++)
            {
                GameObject Ball = Instantiate(GameManager.Instance.newBall, GameManager.Instance.Balls.
                    ElementAt(GameManager.Instance.Balls.Count - 1).position + 
                        new Vector3(0f, 0f, 0.5f), Quaternion.identity);
                GameManager.Instance.Balls.Add(Ball.transform);
            }
            other.GetComponent<Collider>().enabled = false;
            other.transform.parent.GetChild(1).GetComponent<Collider>().enabled = false;
            
        }

        if (other.CompareTag("sub"))
        {
            var Sub = Int16.Parse(other.transform.GetChild(0).name);
            
            if (GameManager.Instance.Balls.Count > Sub)
            {
                for (int i = 0; i < Sub; i++)
                {
                    GameManager.Instance.Balls.ElementAt(GameManager.Instance.Balls.Count - 1).gameObject.SetActive(false);
                    GameManager.Instance.Balls.RemoveAt(GameManager.Instance.Balls.Count - 1);
                }
                //Instantiate(  GameManager.Instance.ex,   GameManager.Instance.
                   // Balls.ElementAt(  GameManager.Instance.Balls.Count - 1).position, Quaternion.identity);
            }

            if (GameManager.Instance.Balls.Count <= Sub)
            {
                for (int i = 0; i < Sub; i++)
                {
                    if (GameManager.Instance.Balls.Count != 0)
                    {
                        GameManager.Instance.Balls.ElementAt(GameManager.Instance.Balls.Count - 1).gameObject
                            .SetActive(false);
                        GameManager.Instance.Balls.RemoveAt(GameManager.Instance.Balls.Count - 1);
                    }
                    else
                    {
                        GameManager.Instance.StartGame = false;
                        return;
                    }
                }
            }
            other.GetComponent<Collider>().enabled = false;
            other.transform.parent.GetChild(0).GetComponent<Collider>().enabled = false;
        }
    }
}