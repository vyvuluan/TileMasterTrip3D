using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCol;
    [SerializeField] private Rigidbody rb;
    public void Collect(Transform tf)
    {
        Vector3 temp = tf.position;
        temp.y = 0.3f;
        boxCol.enabled = false;
        rb.isKinematic = true;
        transform.DOMove(temp, 1f);
        transform.DORotate(new(-90f, -90f, 0), 0.5f);

        //calculator the scale ratio
        Vector3 initialScale = transform.localScale;
        float scaleXRatio = 70f / initialScale.x;
        float newScaleY = initialScale.y * scaleXRatio;
        float newScaleZ = initialScale.z * scaleXRatio;

        transform.DOScale(new Vector3(70f, newScaleY, newScaleZ), 0.5f);
    }
    public void Back(Vector3 position)
    {
        boxCol.enabled = true;
        rb.isKinematic = false;
        float tempY = position.y;
        //boxCol.isTrigger = true;
        transform.DOMoveY(position.y + 3f, 0.3f).OnComplete(() =>
        {
            transform.DOMove(new(position.x, transform.position.y, position.z), 0.5f).OnComplete(() =>
            {
                transform.DOMoveY(tempY, 0.3f);
            });
        });

        //transform.DORotate(new(startTf.rotation.x, startTf.rotation.y, startTf.rotation.z), 0.5f);

        //calculator the scale ratio
        Vector3 initialScale = transform.localScale;
        float scaleXRatio = 90f / initialScale.x;
        float newScaleY = initialScale.y * scaleXRatio;
        float newScaleZ = initialScale.z * scaleXRatio;

        transform.DOScale(new Vector3(90f, newScaleY, newScaleZ), 0.5f);
    }
}
