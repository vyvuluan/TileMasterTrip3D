using DG.Tweening;
using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCol;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TileType tileType;
    private Action<bool> onChangeCanTouch;
    private Vector3 startPos;
    private Quaternion startQuaternion;
    public TileType TileType { get => tileType; }

    public void OnInit(TileType tileType)
    {
        this.tileType = tileType;
    }
    public void SetRollback(Vector3 startPos, Quaternion startQuaternion, Action<bool> onChangeCanTouch)
    {
        this.startPos = startPos;
        this.startQuaternion = startQuaternion;
        this.onChangeCanTouch = onChangeCanTouch;
    }

    public void Collect(Transform tf)
    {
        Vector3 temp = tf.position;
        temp.y = 0.3f;
        boxCol.enabled = false;
        rb.isKinematic = true;
        transform.DOMove(temp, 0.5f).OnComplete(() => onChangeCanTouch?.Invoke(true));
        transform.DORotate(new(-90f, -90f, 0), 0.5f);
        transform.DOScale(CalculatorScaleX(70f), 0.5f);
    }
    private Vector3 CalculatorScaleX(float scale)
    {
        Vector3 initialScale = transform.localScale;
        float scaleXRatio = 70f / initialScale.x;
        float newScaleY = initialScale.y * scaleXRatio;
        float newScaleZ = initialScale.z * scaleXRatio;
        return new Vector3(scale, newScaleY, newScaleZ);
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
        transform.DOScale(CalculatorScaleX(90f), 0.5f);
    }
    public void AddForce(int force, Vector3 direction)
    {
        rb.AddForce(force * direction, ForceMode.Impulse);
    }
    public void OnDespawn()
    {
        SimplePool.Despawn(gameObject);
    }
}
