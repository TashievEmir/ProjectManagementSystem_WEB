async function authentificate(){

    try{
        let userEmail=document.getElementById("emailInput").value;
        let userpassword=document.getElementById("passwordInput").value;
        let data=
            {
                "Email": `${userEmail}`,
                "Password": `${userpassword}`
            }
        const response= await fetch("https://localhost:44345/api/auth/Token",{
            method:"POST",
            headers:{
                "Content-Type":"application/json"
            },
            body: JSON.stringify(data)
        });
        const data2 = await response.json();
        if(response.ok){
            
            localStorage.setItem("tokenKey", JSON.stringify(data2));

            window.location.href="welcome.html"
        }
        else{
            alert("Incorrect name or password, please try again")
        }
    }
    catch (error){
        console.log(error)
    }
}
async function forgot(){
    let mail=prompt("input your email","");
    let data=
        {
            "Email":`${mail}`
        }
    const response= await fetch(`https://localhost:44345/api/auth/checksuchemailexist`,{
        method:"POST",
        headers:{"Content-Type":"application/json"},
        body: JSON.stringify(data)
    });
    //await response.json();
    if(response.ok){
        confirm('The code for reset password was sent to your email, if you you get it press the submit button and input your code')
        let code=prompt("input code","");
        let submitCode={
            "code": `${code}`,
            "email": `${mail}`
        }

        const request= await fetch(`https://localhost:44345/api/auth/checkcodeemail`,{
            method:"POST",
            headers:{"Content-Type":"application/json"},
            body: JSON.stringify(submitCode)
        });
        if(request.ok) window.location.href="forgotPassword.html"
        else alert("code hasn't matched");
    }
    else{
        alert("There is no such email")
    }
}
function signup(){
    window.location.href="SignUp.html"
}
