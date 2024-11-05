using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireShell : MonoBehaviour
{
    public GameObject bullet;
    public GameObject turret;
    public GameObject enemy;
    public Transform turretBase;
    float speed = 15.0f;
    float rotSpeed = 2.0f;
    void CreateBullet()
    {
        GameObject shell = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        shell.GetComponent<Rigidbody>().velocity = speed * turretBase.forward;
    }

    void RotateTurret()
    {
        float? angle = CalculateAngle(false);
        if (angle != null)
        {
            turretBase.localEulerAngles = new Vector3(360f - (float)angle,0f,0f);
        }
    }

    float? CalculateAngle(bool low)
    {
        Vector3 targetDir = enemy.transform.position - this.transform.position;
        float y = targetDir.y;
        targetDir.y = 0.0f;
        float x = targetDir.magnitude -1.0f;
        float gravity = 9.8f;
        float sSqr = speed * speed;
        float underTheSqrRoot = (sSqr*sSqr) - gravity * (gravity*x*x+2*y*sSqr);

        if (underTheSqrRoot >= 0.0f)
        {
            float root= Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;

            if (low)
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            else
                return (Mathf.Atan2(highAngle,gravity*x) * Mathf.Rad2Deg);
        }
        else
        {
            return null;
        }
    }
    void Update()
    {

        Vector3 direction = (enemy.transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
        RotateTurret();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateBullet();
        }

    }
}

