using UnityEngine;

namespace UI
{
    public class MenuParallax : MonoBehaviour
    {
        private const float OffsetMultiplier = 1f;
        private const float SmoothTime = 0.3f;
        
        private Vector2 _startPosition;
        private Vector3 _velocity;
        void Start()
        {
            _startPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            transform.position = Vector3.SmoothDamp(transform.position, _startPosition + offset * OffsetMultiplier, ref _velocity, SmoothTime);
        }
    }

}
