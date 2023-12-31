using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCol;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Image image;
    [SerializeField] private int tileType;
    private Action<bool> onChangeCanTouch;
    private Vector3 startPos;
    private Quaternion startQuaternion;
    public int TileType { get => tileType; }
    private void FixedUpdate()
    {
        Vector3 objectDirection = transform.forward;
        if (objectDirection.y < 0.5f)
        {
            objectDirection.y = 0.6f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(objectDirection), 10f * Time.fixedDeltaTime);
        }

    }
    public void OnInit(int tileType, Sprite sprite)
    {
        this.tileType = tileType;
        image.sprite = sprite;
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
        onChangeCanTouch?.Invoke(false);
        if (transform == null) return;
        transform.DOMove(temp, 0.3f);
        transform.DORotate(new(-90f, -90f, 0), 0.3f);
        transform.DOScale(CalculatorScaleX(70f), 0.3f).OnComplete(() => onChangeCanTouch?.Invoke(true));
    }
    private Vector3 CalculatorScaleX(float scale)
    {
        Vector3 initialScale = transform.localScale;
        float scaleXRatio = scale / initialScale.x;
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
    public void AddForce(float force, Vector3 direction)
    {
        rb.AddForce(force * direction, ForceMode.Impulse);
    }
    public void OnDespawn()
    {
        SimplePool.Despawn(gameObject);
    }
}
