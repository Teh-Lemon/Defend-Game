using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    #region Inspector Variables
    // How much the camera shakes when randomly shaking
    [SerializeField]
    float SHAKE_MAGNITUDE;
    // How long the random shake lasts
    [SerializeField]
    float SHAKE_DURATION;

    // How smoothly the camera resets to the original position
    [SerializeField]
    float SMOOTHNESS;
    #endregion

    // Used to reset the camera at the end
    Vector3 origPosition;

    void Start()
    {
        origPosition = transform.position;
    }

    // How far the camera currently is from the original positon
    //Vector3 distance = Vector3.zero;
    void FixedUpdate()
    {
        // constantly bring the camera back to the original position
        transform.position = Vector3.Lerp(transform.position, origPosition,
            SMOOTHNESS * Time.deltaTime);
    }

    public IEnumerator RandomShake()
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

        //ResetPosition();
    }

    // Bump the camera towards the direction
    public void ShakeDirection(Vector3 direction, float magnitude)
    {
        transform.position += (direction.normalized * magnitude);
    }

    // Deprecated
    /*
    public void ResetPosition()
    {
        transform.position = origPosition;
    }*/
}
