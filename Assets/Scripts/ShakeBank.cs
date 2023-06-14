using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBank : MonoBehaviour
{
    public float timeBtwShakes = 0.05f;
    public float shakeShift = 0.1f;
    // Start is called before the first frame update
    public IEnumerator ShakeMe()
    {
        var shift = Vector3.right * shakeShift;
        while (true)
        {
            transform.position += shift;
            shift *= -1;
            yield return new WaitForSeconds(timeBtwShakes);
        }
    }
}
