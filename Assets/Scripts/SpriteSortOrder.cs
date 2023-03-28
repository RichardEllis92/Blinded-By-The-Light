using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    private SpriteRenderer _theSr;

    // Start is called before the first frame update
    void Start()
    {
        _theSr = GetComponent<SpriteRenderer>();

        _theSr.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }
}
