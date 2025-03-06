using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool isRed;
    private bool is8ball = false;
    private bool isCueBall = false;

    public bool isBallRed(){
        return isRed;
    }

    public bool IsCueBall(){
        return isCueBall;
    }

    public bool IsEigthBall(){
        return is8ball;
    }

    public void BallSetup(bool red){
        isRed = true;
        if(isRed){
            GetComponent<Renderer>().material.color = Color.red;
        }else{
            GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    public void MakeCueBall(){
        isCueBall = true;
    }

    public void MakeEightBall(){
        is8ball = true;
        GetComponent<Renderer>().material.color = Color.black;
    }

}
