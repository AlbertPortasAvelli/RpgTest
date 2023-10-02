using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonGame : MonoBehaviour
{
    public bool is_playing;
    public List<GameObject> rocks;
    private List<GameObject> rock_pattern;
    private int rounds;
    private int currentRound;

    public GameObject portal;
    public Camera portalCamera;
    public float animationDuration = 2.0f;
    public float transitionDuration = 2.0f;
    public float returnDuration = 2.0f;
    private bool showPortal = false;
    private bool gameEnded = false; // New variable to track game ending

    // Start is called before the first frame update
    void Start()
    {
        is_playing = false;
        rock_pattern = new List<GameObject>();
        rounds = 1; // Start with 1 round
        currentRound = 0;
        portal.SetActive(false);
        portalCamera.gameObject.SetActive(false);
    }

    public void GeneratePattern()
    {
        rock_pattern.Clear();
        for (int i = 0; i < rounds; i++)
        {
            int randomInt = UnityEngine.Random.Range(0, rocks.Count);
            rock_pattern.Add(rocks[randomInt]);
        }
    }

    IEnumerator ShowPattern()
    {
        if (rounds <= 4)
        {
            List<GameObject> patternCopy = new List<GameObject>(rock_pattern); // Create a copy of the pattern
            foreach (GameObject rock in patternCopy)
            {
                Renderer rockRenderer = rock.GetComponent<Renderer>();
                rockRenderer.material.EnableKeyword("_EMISSION");
                yield return new WaitForSeconds(2.0f);
                rockRenderer.material.DisableKeyword("_EMISSION");
                yield return new WaitForSeconds(2.0f);
            }
        }
        is_playing = true; // Allow player input after showing the pattern
    }

    private void ShowPortalAndCameraTransition()
    {
        // Disable player input during the transition
        is_playing = false;

        // Code to activate portal and portal camera
        portal.SetActive(true);
        portalCamera.gameObject.SetActive(true);

        // Implement portal animation here if needed
        StartCoroutine(PortalAnimation());

        // Implement camera transition (switch to portal camera)
        StartCoroutine(CameraTransition());

        // Implement a delay if needed before returning to the main camera
        StartCoroutine(ReturnToMainCamera());

        // Set gameEnded to true
        gameEnded = true;
    }

    private IEnumerator PortalAnimation()
    {
        // Implement portal opening animation here
        // You can use Unity's animation system or transform manipulation

        yield return new WaitForSeconds(animationDuration);
    }

    private IEnumerator CameraTransition()
    {
        // Smoothly transition to the portal camera
        // You can use Vector3.Lerp or other techniques for camera movement

        yield return new WaitForSeconds(transitionDuration);
    }

    private IEnumerator ReturnToMainCamera()
    {
        // Implement any delay or animation needed before returning to the main camera

        yield return new WaitForSeconds(returnDuration);

        // Disable portal camera
        
        portalCamera.gameObject.SetActive(false);

        // Switch back to the main camera
        Camera.main.gameObject.SetActive(true);

        // Continue gameplay
        is_playing = true;
    }

    private IEnumerator CheckPlayerInput(GameObject rockClicked)
    {
        Renderer rockRenderer = rockClicked.GetComponent<Renderer>();
        rockRenderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(2.0f);
        rockRenderer.material.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(2.0f);

        if (rockClicked == rock_pattern[currentRound])
        {
            currentRound++;
            if (currentRound == rock_pattern.Count)
            {
                Debug.Log("Round Win!");
                currentRound = 0;
                rounds++; // Increase the number of rounds for the next level
                Restart();
            }
        }
        else
        {
            Debug.Log("Round Lost!");
            Restart();
        }
    }

    public void Restart()
    {
        is_playing = false;
        GeneratePattern();
        StartCoroutine(ShowPattern());
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnded)
        {
            if (is_playing)
            {
                if (rounds == 5 && !showPortal)
                {
                    showPortal = true;
                    ShowPortalAndCameraTransition();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        GameObject rockClicked = hit.collider.gameObject;
                        StartCoroutine(CheckPlayerInput(rockClicked));
                    }
                }
            }
        }
    }

    // Add a new method to start the game
    public void StartGame()
    {
        if (!is_playing)
        {
            Restart();
            is_playing = true;
        }
    }
}
