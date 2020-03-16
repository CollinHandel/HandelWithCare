using UnityEngine;
using System.Collections;

public class ClickManager : MonoBehaviour {

    public MenuManager MenuManager;

    Ray ray;
    RaycastHit2D hit;

    void Start()
    {
        MenuManager = GameObject.Find("MainMenuManager").GetComponent<MenuManager>();
    }

    // Update is called once per frame
    void Update ()
    {

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

            hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "LevelOption")
                {
                    MenuManager.LoadLevel(System.Array.IndexOf(MenuManager.LevelOptions, hit.collider.name));
                }
            }
        }
#endif
    }
}
