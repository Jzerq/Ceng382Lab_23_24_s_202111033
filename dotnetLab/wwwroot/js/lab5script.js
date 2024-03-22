function hidelements(){
    let elt1 = document.getElementById("id1");
    let elt2 = document.getElementById("id2");

    if (elt1.style.display == "none" && elt2.style.display == "none") {
        elt1.style.display = "block";
        elt2.style.display = "block";
        

    } 
    else {
        elt1.style.display = "none";
        elt2.style.display = "none";
       
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






