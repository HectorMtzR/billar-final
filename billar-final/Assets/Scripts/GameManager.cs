using UnityEngine;
using TMPro;

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

    [SerializeField] TextMeshProUGUI player1BallsText;
    [SerializeField] TextMeshProUGUI player2BallsText;
    [SerializeField] TextMeshProUGUI currentTurnText;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] GameObject restartButton;
    [SerializeField] Transform headPosition;

    void start(){
        currentPlayer = CurrentPlayer.Player1;
    }

    bool Scratch(){
        if(currentPlayer == CurrentPlayer.Player1){
            if(isWinningShotForPlayer1){
                ScratchOnWinningShot("Player 1");
                return true;
            }
        }else{
            if(isWinningShotForPlayer2){
                ScratchOnWinningShot("Player 2");
                return true;
            }
        }
        NextPlayerTurn();
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

    void NoMoreBalls(CurrentPlayer player){
        if(player == CurrentPlayer.Player1){
            isWinningShotForPlayer1 = true;
        }else{
            isWinningShotForPlayer2 = true;
        }
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
                    Win("Player 1");
                    return true;
                }
            }else{
                if(isWinningShotForPlayer2){
                    Win("Player 2");
                    return true;
                }
            }
            EarlyEightBall();
        }else{
            //All other logic when not eigth ball or cue ball
            if(ball.IsBallRed()){
                player1BallsRemaining--;
                if(player1BallsRemaining<=0){
                    isWinningShotForPlayer1 = true;
                }
                if(currentPlayer != CurrentPlayer.Player1){
                    NextPlayerTurn();
                }
            }else{
                player2BallsRemaining--;
                if(player2BallsRemaining<=0){
                    isWinningShotForPlayer2 = true;
                }
                if(currentPlayer != CurrentPlayer.Player2){
                    NextPlayerTurn();
                }
            }
        }
        return true;
    }

    void Lose(string message){
        messageText.GameObject.SetActive(true);
        messageText.text = message;
        restartButton.SetActive(true);
    }

    void Win(string player){
        messageText.GameObject.SetActive(true);
        messageText.text = player + " Has Won!";
        restartButton.SetActive(true);
    }

    void NextPlayerTurn(){
        if(currentPlayer == CurrentPlayer.Player1){
            currentPlayer = CurrentPlayer.Player2;
            currentTurnText.text = "Current Turn: Player 2";
        }else{
            currentPlayer = CurrentPlayer.Player2;
            currentTurnText.text = "Current Turn: Player1";
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "ball"){
            if(CheckBall(other.gameObject.GetComponent<Ball>())){
                Destroy(other.gameObject);
            }else{
                other.gameObject.transform.position = headPosition.position;
                other.gameobject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameobject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }
}
