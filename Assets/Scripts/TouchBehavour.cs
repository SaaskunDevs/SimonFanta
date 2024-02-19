using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchBehavour : MonoBehaviour
{
    public Gameplay gameplay;
    private Vector2 mousePos;
    private RaycastHit hit;
    private Ray ray;
    public bool canTouch = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject(0))
            {
                Touch touch = Input.GetTouch(0);

                // Move the cube if the screen has the finger moving.
                if (touch.phase == TouchPhase.Began)
                {
                    ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out hit) && canTouch)
                    {
                        if (hit.collider.CompareTag("Toucher"))
                        {
                            gameplay.CheckTouch(hit.collider.GetComponent<Toucher>().Touch());

                        }
                    }
                }
            }
        }
#endif

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && canTouch)
                {
                    if (hit.collider.CompareTag("Toucher"))
                    {
                        gameplay.CheckTouch(hit.collider.GetComponent<Toucher>().Touch());

                    }
                }
            }
        }
#endif
    }
}
