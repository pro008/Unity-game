#pragma strict

var level: String ;

function OnTriggerEnter2D(Col : Collider2D){
    if(Col.CompareTag("Player")){
        Application.LoadLevel(level);
    }
}
