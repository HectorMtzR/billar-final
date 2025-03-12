using UnityEngine;

public class GameSetup : MonoBehaviour
{

    int redBallsRemaining = 7;
    int blueBallsRemaining = 7;
    float ballRadius;
    float ballDiameter;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform cueBallPosition;
    [SerializeField] Transform headBallPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballRadius = ballPrefab.GetComponent<SphereCollider>().radius*100f;
        ballDiameter = ballRadius*2f;
        
        //Esta funcion ayuda a parar todo
        //Debug.Break();

        PlaceAllBalls();
    }

    
    void PlaceAllBalls(){
        PlaceCueBall();
        PlaceRandomBalls();
    }

    void PlaceCueBall(){
        GameObject  ball = Instantiate(ballPrefab, cueBallPosition.position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeCueBall();
    }

    void PlaceEightBall(Vector3 position){
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.GetComponent <Ball>().MakeEightBall();
    }

    void PlaceRandomBalls(){
        int NumInThisRow = 1;
        int rand;
        Vector3 firstInRowPosition = headBallPosition.position;
        Vector3 currentPosition = firstInRowPosition;

        void PlaceRedBall(Vector3 position){
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetup(true);
            redBallsRemaining--;
        }

        void PlaceBlueBall(Vector3 position){
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetup(false);
            blueBallsRemaining--;
        }

        //Outer loop is the 5 rows
        for (int i=0; i<5; i++){

            //Inner loop are the balls in each row
            for(int j=0; j<NumInThisRow; j++ ){

                //Check to see if its the middle spot where the 8 ball goes
                if(i==2 && j==1){
                    PlaceEightBall(currentPosition);

                // If there are red and blue balls remainig, randomly choose one and place it
                }else if (redBallsRemaining>0 && blueBallsRemaining>0){
                    rand = Random.Range(0,2);
                    if(rand == 0){
                        PlaceRedBall(currentPosition);
                    }else{
                        PlaceBlueBall(currentPosition);
                    }
                // If only red balls are left, place one
                }else if(redBallsRemaining>0){
                    PlaceRedBall(currentPosition);

                // Otherwise, place a blue ball
                }else{
                    PlaceBlueBall(currentPosition);
                }

                //Move the current position for the next ball in this row to the right
                currentPosition += new Vector3(1,0,0).normalized*ballDiameter;
            }

            //Once all the balls in the row have been placed, move to the next row
            firstInRowPosition += Vector3.back * (Mathf.Sqrt(3)*ballRadius) + Vector3.left*ballRadius;
            currentPosition = firstInRowPosition;
            NumInThisRow++;
        }

    }

}
