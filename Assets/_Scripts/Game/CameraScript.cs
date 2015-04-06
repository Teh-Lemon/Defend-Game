using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    #region Inspector Variables
    // How much the camera shakes when shaking
    [SerializeField]
    float SHAKE_MAGNITUDE;

    [SerializeField]
    float SHAKE_DURATION;
    #endregion

    // Used to reset the camera at the end
    Vector3 origPosition;

    void Start()
    {
        origPosition = transform.position;
    }

    public IEnumerator Shake()
    {
        // How long the camera has been shaking
        float elapsed = 0.0f;

        while (elapsed < SHAKE_DURATION)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / SHAKE_DURATION;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= SHAKE_MAGNITUDE * damper;
            y *= SHAKE_MAGNITUDE * damper;

            // Shake the camera
            transform.position = new Vector3(x + origPosition.x
                , y + origPosition.y
                , origPosition.z);

            yield return null;
        }
        
        transform.position = origPosition;
    }
}
