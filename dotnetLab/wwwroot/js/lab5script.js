function hidelements(){
    let elt=document.getElementById("PRicce");
    var newText=document.createTextNode("toggling");

    if(elt.style.display=="none"){
        elt.style.display="block";
    }
    else{
        elt.style.display="none";
    }
   
}
function hidecal(){

    let elt=document.getElementById("CalculateForm");
    elt.style.visibility="visible";

    if(elt.style.display=="block"){
        elt.style.display="none";
    }
    else{
        elt.style.display="block";
    } 
}


function calculateSum() {

    const num1=Number(document.getElementById("num1").value);
    const num2=Number(document.getElementById("num2").value);
    const result=document.getElementById("result");
    let sum=num1+num2;

    result.innerHTML=`The sum of ${num1} and ${num2} is ${sum}`;
}






