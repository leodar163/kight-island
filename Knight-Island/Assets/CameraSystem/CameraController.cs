using UnityEngine;

namespace CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public float speed = 5;
        
        void LateUpdate()
        {
            if (!target) return;
            
            Vector3 position = target.position;
            position.z = transform.position.z;
            
            transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
        }
        
    }
}
