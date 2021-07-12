using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform mainPlayer;
    Vector3 correctPosition;
    public bool isFollow;
    public float stopX = 7;
    public float stopY = 3;
    public float Speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        mainPlayer = GameManager.Instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //correctPosition = mainPlayer.position;
        correctPosition.z = transform.position.z;
        if (mainPlayer.position.x < stopX && mainPlayer.position.x > -stopX)
        {
            correctPosition.x = mainPlayer.position.x;

        }
        if (mainPlayer.position.y < stopY && mainPlayer.position.y > -stopY)
        {
            correctPosition.y = mainPlayer.position.y;

        }
        //transform.position = Vector3.MoveTowards(transform.position, correctPosition, 4.0f * Time.deltaTime);
    }
    private void LateUpdate()
    {
        if (isFollow)
        {
            if (Vector3.Distance(transform.position, correctPosition) > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, correctPosition, Speed * Time.deltaTime);
            }
        }

    }
}
