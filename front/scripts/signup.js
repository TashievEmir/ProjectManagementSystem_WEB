async function SignUp(){
    let name=document.getElementById("name").value;
    let surname=document.getElementById("surname").value;
    let role=document.getElementById("status").value;
    let email=document.getElementById("email").value;
    let password=document.getElementById("password").value;
    let passwordRepeat=document.getElementById("passwordRepeat").value;

    if(name!="" && surname!="" && role!="" && email!="" && password!=="" && password==passwordRepeat){
        let data={
            "email":`${email}`,
            "password":`${password}`,
            "name":`${name}`,
            "surname":`${surname}`,
            "role": Number(role)
        }

        const response= await fetch(`https://localhost:44345/api/auth/signup`,{
            method:"POST",
            headers:{"Content-Type":"application/json"},
            body: JSON.stringify(data)
        });

        response.json();
        
        if(response.ok){
            window.location.href="authentification.html"
        }
        else{
            alert(`${response.statusText}`)
        }
    }
    else{
        alert("Please fill all blankes or check passwords")
    }
}
