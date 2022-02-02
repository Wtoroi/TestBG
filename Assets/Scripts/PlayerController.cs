using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MazeMaster mazeMaster;
    [SerializeField] private GameObject particleEmitter;
    [SerializeField] private GameObject particleEmitterWin;
    [SerializeField] private GameObject blackSkreen;

    private Vector2 StartPosition;
    private Vector2 EndPosition;

    private int Iterator = 0;
    private float sheeldTimer = 2;

    private float Progress = 0;
    private float speed = 0.03f;

    private bool run = false;
    private bool isDefence = false;

    void Start()
    {
        MazeMaster.PlayerStart += SetStart;
        Trap.TrapTuched += Death;
    }

    private void FixedUpdate()
    {
        if (!run)
            return;
        transform.position = Vector2.Lerp(StartPosition, EndPosition, Progress);
        Progress += speed;
        if(transform.position.x == EndPosition.x && transform.position.y == EndPosition.y)
        {
            if (Iterator == mazeMaster.BestPath.Count - 1)
                Win();
            StartPosition = EndPosition;
            Progress = 0;
            Iterator++;
            EndPosition = new Vector2(mazeMaster.BestPath[Iterator].x, mazeMaster.BestPath[Iterator].y);
        }
    }

    private void Win()
    {
        particleEmitterWin.transform.position = transform.position;
        particleEmitterWin.GetComponent<ParticleSystem>().Play();
        blackSkreen.active = true;
        StartCoroutine(winWaiter());
    }

    IEnumerator winWaiter()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (isDefence)
            sheeldTimer -= Time.deltaTime;
        if (sheeldTimer <= 0)
            SheeldOff();
    }

    private void Death()
    {
        if (isDefence)
            return;

        particleEmitter.transform.position = transform.position;
        particleEmitter.GetComponent<ParticleSystem>().Play();

        Iterator = 0;
        transform.position = new Vector3(1, 1, 0);
        SetStart();
    }

    private void SetStart()
    {
        StartPosition = transform.position;
        EndPosition = new Vector2(mazeMaster.BestPath[Iterator].x, mazeMaster.BestPath[Iterator].y);
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(2);
        run = true;
    }

    public void SheeldOn()
    {
        isDefence = true;
        Color color;
        if (ColorUtility.TryParseHtmlString("#ADFF2F", out color))
        {
            transform.GetComponent<SpriteRenderer>().color = color;
        }
    }
    public void SheeldOff()
    {
        isDefence = false;
        transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0);
        sheeldTimer = 2;
    }
}
