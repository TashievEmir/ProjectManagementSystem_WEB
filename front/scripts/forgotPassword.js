async function Update(){
    let email=document.getElementById("email").value;
    let password=document.getElementById("password").value;
    let passwordRepeat=document.getElementById("passwordRepeat").value;

    if(password!=="" && password==passwordRepeat){
        let data={
            "Email":`${email}`,
            "Password":`${password}`
        }
        const response= await fetch(`https://localhost:44345/api/auth/CreateNewPassword`,{
            method:"POST",
            headers:{"Content-Type":"application/json"},
            body: JSON.stringify(data)
        });
        //await response.json();
        if(response.ok){
            window.location.href="authentification.html"
        }
        else{
            alert(`${response.statusText}`)
        }
    }
    else{
        alert("Incorrect password")
    }
}