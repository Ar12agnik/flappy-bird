//using UnityEngine;

//public class pipeSpawnerScript : MonoBehaviour
//{

//    public GameObject pipe;
//    public float spawnrate = 2;
//    private float timer = 0;
//    public float heightoffset =10;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        spawn_pipe();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (timer < spawnrate)
//        {
//            timer += Time.deltaTime;
//        }
//        else {
//            spawn_pipe();

//        }

//    }
//    void spawn_pipe()
//    {
//        float lowest_point = transform.position.y - heightoffset;
//        float heighest_point = transform.position.y + heightoffset;
//        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowest_point, heighest_point), 0), transform.rotation);
//        timer = 0;
//    }
//}

using UnityEngine;

public class PipeSpawnerScript : MonoBehaviour
{
    [Header("Pipe / spawn")]
    public GameObject pipe;
    public float spawnRate = 2f;                 // average seconds between spawns
    public float spawnRateJitter = 0.3f;         // +/- jitter around spawnRate
    public float maxYChange = 2f;                // max vertical change between successive pipes
    public float perlinSpeed = 0.6f;             // speed of Perlin noise for smooth variation
    public float pipeHalfHeightFallback = 0.5f;  // used if prefab doesn't have a SpriteRenderer

    private float timer = 0f;
    private float currentSpawnInterval;
    private float lastY;
    private float perlinSeed;

    void Start()
    {
        perlinSeed = Random.Range(0f, 1000f);
        // initialize spawn interval and lastY
        currentSpawnInterval = spawnRate + Random.Range(-spawnRateJitter, spawnRateJitter);
        lastY = GetClampedStartY();
        SpawnPipe(); // first immediate spawn (like your original Start)
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentSpawnInterval)
        {
            SpawnPipe();
        }
    }

    // Computes camera-safe min/max Y for z = pipe spawn plane (this.transform.position.z)
    void GetCameraYBounds(out float minY, out float maxY)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            // fallback to some reasonable range around spawner if no camera found
            minY = transform.position.y - 10f;
            maxY = transform.position.y + 10f;
            return;
        }

        // distance from camera to the z plane where pipes will be spawned
        float zDistance = Mathf.Abs(cam.transform.position.z - transform.position.z);

        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, zDistance));

        minY = bottomLeft.y;
        maxY = topRight.y;
    }

    float GetPipeHalfHeight()
    {
        if (pipe == null) return pipeHalfHeightFallback;

        SpriteRenderer sr = pipe.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // bounds are in world units and already account for sprite scale on prefab
            return sr.bounds.size.y * 0.5f;
        }

        // fallback if no SpriteRenderer found
        return pipeHalfHeightFallback;
    }

    // initial Y clamped to screen so first pipe is inside view
    float GetClampedStartY()
    {
        GetCameraYBounds(out float minY, out float maxY);
        float half = GetPipeHalfHeight();
        minY += half;
        maxY -= half;
        return Mathf.Clamp(transform.position.y, minY, maxY);
    }

    void SpawnPipe()
    {
        // recompute bounds (handles moving camera or camera zoom)
        GetCameraYBounds(out float minY, out float maxY);
        float half = GetPipeHalfHeight();
        minY += half;
        maxY -= half;

        // Perlin noise gives a smooth target between minY and maxY
        float perlin = Mathf.PerlinNoise(Time.time * perlinSpeed, perlinSeed);
        float targetY = Mathf.Lerp(minY, maxY, perlin);

        // limit the vertical jump from previous spawned pipe so it's playable
        float newY = Mathf.Clamp(targetY, lastY - maxYChange, lastY + maxYChange);

        // final clamp to make absolutely sure pipe is on screen
        newY = Mathf.Clamp(newY, minY, maxY);

        Vector3 spawnPos = new Vector3(transform.position.x, newY, transform.position.z);
        Instantiate(pipe, spawnPos, transform.rotation);

        // prepare next spawn
        lastY = newY;
        timer = 0f;
        currentSpawnInterval = Mathf.Max(0.05f, spawnRate + Random.Range(-spawnRateJitter, spawnRateJitter));
    }
}
