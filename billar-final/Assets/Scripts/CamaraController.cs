using UnityEngine;

public class CamaraController : MonoBehaviour
{

    [SerializeField] float rotationSpeed;
    [SerializeField] Vector3 offset;
    [SerializeField] float downAngle;
    [SerializeField] float power;
    private float horizontalInput;

    Transform cueBall;

    void Start(){
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball")){
            if(ball.GetComponent<Ball>().IsCueBall()){
                cueBall = ball.transform;
                break;
            }
        }
        ResetCamara();
    }

    void Update(){
        if(cueBall != null){
            horizontalInput = Input.GetAxis("Mouse X")*rotationSpeed*Time.deltaTime;
            transform.RotateAround(cueBall.position,Vector3.up, horizontalInput);
        }

        //Temporary

        if(Input.GetKeyDown(KeyCode.Space)){
            ResetCamara();
        }

        //End Temporary

        if(Input.GetButtonDown("Fire1")){
            
            Vector3 hitDirection = transform.forward;
            hitDirection = new Vector3(hitDirection.x, 0, hitDirection.z).normalized;

            cueBall.gameObject.GetComponent<Rigidbody>().AddForce(hitDirection*power,ForceMode.Impulse);
        }
    }

    public void ResetCamara(){
        transform.position = cueBall.position+offset;
        transform.LookAt(cueBall.position);
        transform.localEulerAngles = new Vector3(downAngle, transform.localEulerAngles.y, 0);
    }
}
