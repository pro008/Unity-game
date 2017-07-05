#pragma strict

function Start () {
    
}

function Update () {
    if(Input.GetKey(KeyCode.Space))
        Application.LoadLevel("Scene1");
}
function playgame(){
    Application.LoadLevel("Scene1");
}
