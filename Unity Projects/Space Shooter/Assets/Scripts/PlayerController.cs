using UnityEngine;
using System.Collections;
using Thalmic.Myo;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using Quaternion = UnityEngine.Quaternion;
using UnlockType = Thalmic.Myo.UnlockType;
using Vector3 = UnityEngine.Vector3;
using VibrationType = Thalmic.Myo.VibrationType;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;

}

public class PlayerController : MonoBehaviour 
{
    public GameObject myo = null;


    public float speed;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn; // shotSpawn.transform.position
	public float fireRate = 0.5f;
	private float nextFire = 0.0f;


	void Update()
	{
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        
        if (thalmicMyo.pose == Pose.FingersSpread && (Time.time > nextFire))
		{

            GetComponent<AudioSource>().Play();
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            ExtendUnlockAndNotifyUserAction(thalmicMyo);

        }
    }

	void FixedUpdate() 
	{
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        //new Vector3(myo.transform.forward.x, 0, myo.transform.forward.z),

	    float moveHorizontal = -myo.transform.forward.x;
		float moveVertical = Input.acceleration.y;

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;

		GetComponent<Rigidbody> ().position = new Vector3 
		(
			Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);


		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * - tilt);
	}

    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo.Unlock(UnlockType.Timed);
        }

        myo.NotifyUserAction();
    }
}
