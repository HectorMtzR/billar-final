using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum CurrentPlayer{
        Player1,
        Player2
    }

    CurrentPlayer currentPlayer;
    bool isWinningShotForPlayer1 = false;
    bool isWinningShotForPlayer2 = false;
    int player1BallsRemaining = 7;
    int player2BallsRemaining = 7;

    bool isWaitingForBallMovementToStop = false;
    bool willSwapPlayers = false;
    bool isGameOver = false;
    bool ballPocketed = false;

    [SerializeField] float shotTimer = 3f;
    private float currentTimer;
    [SerializeField] float movementThereshold;

    [SerializeField] TextMeshProUGUI player1BallsText;
    [SerializeField] TextMeshProUGUI player2BallsText;
    [SerializeField] TextMeshProUGUI currentTurnText;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] GameObject restartButton;
    [SerializeField] Transform headPosition;

    [SerializeField] Camera cueStickCamera;
    [SerializeField] Camera overheadCamera;
    Camera currentCamera;

    void Start(){
        currentPlayer = CurrentPlayer.Player1;
        currentCamera = cueStickCamera;
        currentTimer = shotTimer;
    }

    void Update(){
        if(isWaitingForBallMovementToStop && !isGameOver){

            // Logic here to check if balls are moving
            // When all balls have stopped moving, it can move to the next players turns

            currentTimer -= Time.deltaTime;
            if(currentTimer>0){
                return;
            }

            bool allStopped = true;
            foreach(GameObject ball in GameObject.FindGameObjectsWithTag("Ball")){
                if(ball.GetComponent<Rigidbody>().linearVelocity.magnitude >= movementThereshold){
                    Debug.Log(ball.GetComponent<Rigidbody>().linearVelocity.magnitude);
                    allStopped = false;
                    break;
                }
            }
            if (allStopped){
                isWaitingForBallMovementToStop = false;
                if(willSwapPlayers || !ballPocketed){
                    NextPlayerTurn();
                }else{
                    SwitchCameras();
                }
                currentTimer = shotTimer;
                ballPocketed = false;
            }
        }
    }

    public void SwitchCameras(){
        if(currentCamera == cueStickCamera){
            cueStickCamera.enabled = false;
            overheadCamera.enabled = true;
            currentCamera = overheadCamera;
            isWaitingForBallMovementToStop = true;
        }else{
            cueStickCamera.enabled = true;
            overheadCamera.enabled = false;
            currentCamera = cueStickCamera;
            currentCamera.gameObject.GetComponent<CameraController>().ResetCamera();
        }
    }

    public void RestartTheGame(){
        SceneManager.LoadScene(0);
    }

    bool Scratch(){
        if(currentPlayer == CurrentPlayer.Player1){
            if(isWinningShotForPlayer1){
                ScratchOnWinningShot("Jugador 1");
                return true;
            }
        }else{
            if(isWinningShotForPlayer2){
                ScratchOnWinningShot("Jugador 2");
                return true;
            }
        }
        
        willSwapPlayers = true;
        return false;
    }

    void EarlyEightBall(){
        if(currentPlayer == CurrentPlayer.Player1){
            Lose("Player 1 Hit in the Eight Ball Too Early and Has Lost!");
        }else{
            Lose("Player 2 Hit in the Eight Ball Too Early and Has Lost!");
        }
    }

    void ScratchOnWinningShot(string player){
        Lose(player + " Scratched on Their Final Shot and Has Lost!");
    }

    bool CheckBall(Ball ball){
        if(ball.IsCueBall()){
            if(Scratch()){
                return true;
            }else{
                return false;
            }
        }else if(ball.IsEightBall()){
            if(currentPlayer == CurrentPlayer.Player1){
                if(isWinningShotForPlayer1){
                    Win("Jugador 1");
                    return true;
                }
            }else{
                if(isWinningShotForPlayer2){
                    Win("Jugador 2");
                    return true;
                }
            }
            EarlyEightBall();
        }else{
            //All other logic when not eigth ball or cue ball
            if(ball.IsBallRed()){
                player1BallsRemaining--;
                player1BallsText.text = "Pelotas Restantes del Jugador 1: " + player1BallsRemaining;
                if(player1BallsRemaining<=0){
                    isWinningShotForPlayer1 = true;
                }
                if(currentPlayer != CurrentPlayer.Player1){
                    //NextPlayerTurn();
                    //isWaitingForBallMovementToStop = true;
                    willSwapPlayers = true;
                }
                
            }else{
                player2BallsRemaining--;
                player2BallsText.text = "Pelotas Restantes del Jugador 2: " + player2BallsRemaining;
                if(player2BallsRemaining<=0){
                    isWinningShotForPlayer2 = true;
                }
                if(currentPlayer != CurrentPlayer.Player2){
                    //NextPlayerTurn();
                    //isWaitingForBallMovementToStop = true;
                    willSwapPlayers = true;
                }
            }
        }
        return true;
    }

    void Lose(string message) {
        isGameOver = true;
        messageText.gameObject.SetActive(true);
        messageText.text = message;
        restartButton.SetActive(true);
    }

    void Win(string player) {
        isGameOver = true;
        messageText.gameObject.SetActive(true);
        messageText.text = player + " Has Ganando!";
        restartButton.SetActive(true);
    }


    void NextPlayerTurn() {
        if (currentPlayer == CurrentPlayer.Player1) {
            currentPlayer = CurrentPlayer.Player2;
            currentTurnText.text = "Turno Actual: Jugador 2";
        } else {
            currentPlayer = CurrentPlayer.Player1;
            currentTurnText.text = "Turno Actual: Jugador 1";
        }
        willSwapPlayers = false;
        SwitchCameras();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball") {
            ballPocketed = true;
            if (CheckBall(other.gameObject.GetComponent<Ball>())) {
                Destroy(other.gameObject);
            }else{
                other.gameObject.transform.position = headPosition.position;
                other.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }


}
