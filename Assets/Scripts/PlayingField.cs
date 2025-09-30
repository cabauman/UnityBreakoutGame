using UnityEngine;

namespace BreakoutGame
{
    public class PlayingField : MonoBehaviour
    {
        [SerializeField]
        private Transform _leftWall;
        [SerializeField]
        private Transform _rightWall;
        [SerializeField]
        private Transform _topWall;
        [SerializeField]
        private Transform _deadZone;

        private void Start()
        {
            var cameraDistanceZ = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
            _leftWall.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, cameraDistanceZ));
            _rightWall.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, cameraDistanceZ));
            _topWall.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, cameraDistanceZ));
            _deadZone.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, cameraDistanceZ));
        }
    }
}