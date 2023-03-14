async function Update(){
    let email=document.getElementById("email");
    let password=document.getElementById("password");
    let passwordRepeat=document.getElementById("passwordRepeat");
    if(password!=="" && password==passwordRepeat){
        let data={
            "email":`${email}`,
            "password":`${password}`
        }
        const response= await fetch(`https://localhost:44345/auth/forgot`,{
            method:"POST",
            headers:{"Content-Type":"application/json"},
            body: JSON.stringify(data)
        });
        const user=await response.json();
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