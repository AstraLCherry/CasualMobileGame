using UnityEngine;

public class BackgroundParalax : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform prefab;

    [SerializeField] private int chunkCount;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector2 cell;

    private Chunk[] chunks;

    private Vector3 destinedPoint;

    private void Start()
    {
        chunks = new Chunk[chunkCount];

        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].chunkTransform = Instantiate(prefab);
        }

        chunks[0].isPrimary = true;
        chunks[1].chunkTransform.position = new(cell.x, 0, 10);
        chunks[2].chunkTransform.position = new(0, cell.y, 10);
        chunks[3].chunkTransform.position = new(cell.x, cell.y, 10);
    }

    private void Update()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].distance = Vector2.Distance(target.position, chunks[i].chunkTransform.position);

            //If the target is within range of each chunk
            if (chunks[i].distance < maxDistance && chunks[i].distance > -maxDistance)
            {
                chunks[i].isPrimary = true;
            }
            else //If the target is outside of the range
            {
                chunks[i].isPrimary = false;
            }

            if (chunks[0].isPrimary)
            {
                destinedPoint = chunks[i].chunkTransform.position;

                if (chunks[i].distance < maxDistance)
                {
                    RepositionChunksHorizontally(i);
                    RepositionChunksVetically(i);
                    RepositionChunksDiagonally(i);
                }
            }

            //If chunks are not the main one
            if (chunks[i].chunkTransform != chunks[0].chunkTransform)
            {
                // If the player is on the wrong chunk then moves the main to that wrong
                if (chunks[i].isPrimary)
                    chunks[0].chunkTransform.position = chunks[i].chunkTransform.position;
            }
        }
    }

    private void RepositionChunksHorizontally(int index)
    {
        // RIGHT
        if (target.position.x > chunks[index].chunkTransform.position.x)
        {
            Vector3 dir = new(destinedPoint.x + cell.x, destinedPoint.y, 10);
            chunks[1].chunkTransform.position = dir;
        }
        // LEFT
        else if (target.position.x < chunks[index].chunkTransform.position.x)
        {
            Vector3 dir = new(destinedPoint.x - cell.x, destinedPoint.y, 10);
            chunks[1].chunkTransform.position = dir;
        }
    }

    private void RepositionChunksVetically(int index)
    {
        // UP
        if (target.position.y > chunks[index].chunkTransform.position.y)
        {
            Vector3 dir = new(destinedPoint.x, destinedPoint.y + cell.y, 10);
            chunks[2].chunkTransform.position = dir;
        }
        // DOWN
        else if (target.position.y < chunks[index].chunkTransform.position.y)
        {
            Vector3 dir = new(destinedPoint.x, destinedPoint.y - cell.y, 10);
            chunks[2].chunkTransform.position = dir;
        }
    }

    private void RepositionChunksDiagonally(int index)
    {
        // RIGHT AND UP
        if (target.position.x > chunks[index].chunkTransform.position.x && target.position.y > chunks[index].chunkTransform.position.y)
        {
            Vector3 dir = new(destinedPoint.x + cell.x, destinedPoint.y + cell.y, 10);
            chunks[3].chunkTransform.position = dir;
        }
        // LEFT AND DOWN
        else if (target.position.x < chunks[index].chunkTransform.position.x && target.position.y < chunks[index].chunkTransform.position.y)
        {
            Vector3 dir = new(destinedPoint.x - cell.x, destinedPoint.y - cell.y, 10);
            chunks[3].chunkTransform.position = dir;
        }
        // LEFT AND UP
        else if (target.position.x < chunks[index].chunkTransform.position.x && target.position.y > chunks[index].chunkTransform.position.y)
        {
            Vector3 dir = new(destinedPoint.x - cell.x, destinedPoint.y + cell.y, 10);
            chunks[3].chunkTransform.position = dir;
        }
        // RIGHT AND DOWN
        else if (target.position.x > chunks[index].chunkTransform.position.x && target.position.y < chunks[index].chunkTransform.position.y)
        {
            Vector3 dir = new(destinedPoint.x + cell.x, destinedPoint.y - cell.y, 10);
            chunks[3].chunkTransform.position = dir;
        }
    }
}

[System.Serializable]
struct Chunk
{
    public bool isPrimary;
    public float distance;
    public Transform chunkTransform;
}
