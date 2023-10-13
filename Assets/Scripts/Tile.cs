using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCol;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TileType tileType;
    private Vector3 startPos;
    private Quaternion startQuaternion;
    public TileType TileType { get => tileType; }

    public void OnInit(TileType tileType)
    {
        this.tileType = tileType;
    }
    public void SetRollback(Vector3 startPos, Quaternion startQuaternion)
    {
        this.startPos = startPos;
        this.startQuaternion = startQuaternion;
    }

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
    public void Back()
    {
        boxCol.enabled = true;
        rb.isKinematic = false;
        float tempY = startPos.y;
        transform.DOMoveY(startPos.y + 2f, 0.3f).OnComplete(() =>
        {
            transform.DOMove(new(startPos.x, transform.position.y, startPos.z), 0.5f).OnComplete(() =>
            {
                transform.DOMoveY(tempY, 0.3f);
                transform.DORotateQuaternion(startQuaternion, 0.1f);
            });
        });

        //calculator the scale ratio
        Vector3 initialScale = transform.localScale;
        float scaleXRatio = 90f / initialScale.x;
        float newScaleY = initialScale.y * scaleXRatio;
        float newScaleZ = initialScale.z * scaleXRatio;

        transform.DOScale(new Vector3(90f, newScaleY, newScaleZ), 0.5f);
    }
}
