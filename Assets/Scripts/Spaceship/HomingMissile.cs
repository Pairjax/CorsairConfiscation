using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private CopShipController parentShip;

    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private PlayerShipController _target;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Movement")]
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _rotateSpeed = 95;

    [Header("Prediction")]
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;

    private void Start()
    {
        _target = FindObjectOfType<PlayerShipController>();
    }

    public void SetParent(CopShipController copShip)
    {
        parentShip = copShip;
    }
    private void OnDestroy()
    {
        if(parentShip != null)
            parentShip.spawnedBullets.Remove(gameObject);
    }
    private void FixedUpdate()
    {
        _rb.velocity = transform.right * _speed;

        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector2.Distance(transform.position, _target.transform.position));

        PredictMovement(leadTimePercentage);

        AddDeviation(leadTimePercentage);

        RotateRocket();
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = _target.rb2d.position + _target.rb2d.velocity * predictionTime; 
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector2(Mathf.Cos(Time.time * _deviationSpeed), 0);

        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_explosionPrefab) Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        Player playerShip = collision.gameObject.GetComponent<Player>();
        if (playerShip != null)
            playerShip.Damage();
        
        Destroy(gameObject);
    }
}
