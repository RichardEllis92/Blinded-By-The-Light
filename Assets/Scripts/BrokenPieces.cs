using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 _moveDirection;

    public float deceleration = 5f;

    public float lifetime = 3f;

    public SpriteRenderer theSr;
    public float fadeSpeed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        _moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        _moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _moveDirection * Time.deltaTime;

        _moveDirection = Vector3.Lerp(_moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifetime -= Time.deltaTime;

        if(lifetime < 0)
        {
            var color = theSr.color;
            theSr.color = new Color(color.r, color.g, color.b, Mathf.MoveTowards(color.a, 0f, fadeSpeed * Time.deltaTime));

            if(theSr.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
