using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICar : MonoBehaviour {

    public float maxSteerAngle = 45f;
    public float maxSpeed = 90f;
    public float maxTorque = 2000f;
    public float raycastMaxFar = 10f;
    public bool coll;

    public Vector3 centerOfMass;
    public Transform target;
    public WheelCollider[] wheelColliders;
    public Transform[] wheelMeshes;
    public Transform[] raycastPoints;
    public UIScript uiScript;

    private Rigidbody m_rigidbody;
    private bool stop = false;
    private float bumpDelay;
    private float forceBumpDelay = 0f;
    [SerializeField] private float raycastFar;

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        m_rigidbody = GetComponent<Rigidbody>();
        //float rnd = Random.Range(200, 300);
        //bumpDelay = rnd;
    }

    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypoint();
        UpdateWheelMeshes();
    }

    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(target.position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelColliders[0].steerAngle = newSteer;
        wheelColliders[1].steerAngle = newSteer;
        for (int i = 0; i < raycastPoints.Length; i++)
        {
            raycastPoints[i].transform.rotation = new Quaternion(0,0,0,0);
            raycastPoints[i].transform.Rotate(0, newSteer, 0);
        }
    }

    void Drive()
    {
        float speed = m_rigidbody.velocity.magnitude;

        raycastFar = (raycastMaxFar - 5) * speed / 22 + 5;
        RaycastCheck();
        for (int i = 0; i < raycastPoints.Length; i++)
        {
            Vector3 scl = raycastPoints[i].transform.localScale;
            scl.z = 1 * speed / 21;
            raycastPoints[i].transform.localScale = scl;
        }

        if (stop && forceBumpDelay <= 0)
        {
            wheelColliders[0].motorTorque = 0;
            wheelColliders[1].motorTorque = 0;
            if (speed > 0.5f)
            {
                wheelColliders[0].motorTorque = -maxTorque * 3;
                wheelColliders[1].motorTorque = -maxTorque * 3;
            }
            else if(speed < -0.5f)
            {
                wheelColliders[0].motorTorque = maxTorque * 3;
                wheelColliders[1].motorTorque = maxTorque * 3;
            }
            /*
            if (bumpDelay > 0)
                bumpDelay -= Time.deltaTime;
            else
            {
                stop = false;
                float rnd = Random.Range(200, 300);
                bumpDelay = rnd;
                forceBumpDelay = 3f;
            }*/
        }
        else if (speed < maxSpeed)
        {
            wheelColliders[0].motorTorque = maxTorque;
            wheelColliders[1].motorTorque = maxTorque;
        }
        else
        {
            wheelColliders[0].motorTorque = 0;
            wheelColliders[1].motorTorque = 0;
        }
        /*
        if (forceBumpDelay > 0)
            forceBumpDelay -= Time.deltaTime;*/
    }

    void RaycastCheck()
    {
        bool change = false;
        for (int i = 0; i < raycastPoints.Length; i++)
        {
            Vector3 fwd = raycastPoints[i].TransformDirection(Vector3.forward);
            RaycastHit hit;
            Debug.DrawRay(raycastPoints[i].position, raycastPoints[i].TransformDirection(Vector3.forward) * raycastFar, Color.red, 0.1f);
            if (Physics.Raycast(raycastPoints[i].position, raycastPoints[i].TransformDirection(Vector3.forward), out hit, raycastFar))
            {
                stop = true;
                change = true;
            }
        }
        if(!change)
        {
            if(stop)
            {
                stop = false;
                float rnd = Random.Range(7, 13);
                bumpDelay = rnd;
                forceBumpDelay = 0f;
            }
        }
    }

    void CheckWaypoint()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 2.5f)
        {
            Transform[] nextTarget = target.GetComponent<AIMap>().nextPoint;
            if (nextTarget.Length == 0)
            {
                uiScript.addCar();
                Destroy(gameObject);
            }
            else
            {
                int rnd = Random.Range(0, nextTarget.Length);
                target = nextTarget[rnd];
            }
        }
    }

    void UpdateWheelMeshes()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            Vector3 wheelPosition;
            Quaternion wheelRotartion;
            wheelColliders[i].GetWorldPose(out wheelPosition, out wheelRotartion);
            wheelMeshes[i].transform.position = wheelPosition;
            wheelMeshes[i].transform.rotation = wheelRotartion;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        coll = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        coll = false;
    }
}
