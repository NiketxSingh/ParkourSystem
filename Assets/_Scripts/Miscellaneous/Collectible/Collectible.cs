using UnityEngine;

public class Collectible : MonoBehaviour {
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float amplitude = 0.5f;   
    [SerializeField] private float frequency = 1f;     
    private Vector3 startPos;

    void Start() {
        startPos = transform.position;
    }

    void Update() {
        Hover();
    }

    void Hover() {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }

    private void OnTriggerEnter(Collider other) {
        if (((1 << other.gameObject.layer) & playerLayer) != 0) {
            Destroy(gameObject);
        }
    }
}
