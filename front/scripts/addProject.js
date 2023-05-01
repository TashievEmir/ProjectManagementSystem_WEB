window.onload = () => {
    let members = document.getElementById("members");
    fetch('https://localhost:44345/api/adding/get')
        .then(response => response.json())
        .then(users => {
            users.forEach(user => {
                let member = document.createElement("option");
                member.text = `${user.name}`
                member.value = `${user.id}`
                members.add(member);
            });
        })
        .catch(error => console.error(error));

    let managers = document.getElementById("managers");
    fetch('https://localhost:44345/api/adding/getmanagers')
        .then(response => response.json())
        .then(users => {
            users.forEach(user => {
                let manager = document.createElement("option");
                manager.text = `${user.name}`
                manager.value = `${user.id}`
                managers.add(manager);
            });
        })
        .catch(error => console.error(error));

};
async function openProjects(){
    window.location.href="projects.html";
}

async function addTask(){
    let roleuser=JSON.parse(window.localStorage.getItem('tokenKey')).role;
    console.log(roleuser);
    if(roleuser=="employee"){
        alert("you don't have permission");
    }
    else
        window.location.href="addTask.html";
}
async function Save(){

    let data={
        "name":`${document.getElementById("name").value}`,
        "manager":`${document.getElementById("managers").value}`,
        "status":`${document.getElementById("status").value}`,
        "startdate":`${document.getElementById("startdate").value}`,
        "enddate":`${document.getElementById("enddate").value}`
    }
    const selectElement = document.getElementById("members");

    const selectedOptions = selectElement.selectedOptions;
    const selectedValues = [];
    for (let i = 0; i < selectedOptions.length; i++) {
        selectedValues.push(selectedOptions[i].value);
    }
    selectedValues.push(data.manager);
    const newData = {
        ...data,
        members: selectedValues.length ? selectedValues : null,
    };
    const response= await fetch(`https://localhost:44345/api/adding/saveproject`,{
        method:"POST",
        headers:{"Content-Type": "application/json"},
        body: JSON.stringify(newData)
    }).catch(function (error) {
        console.log(error);
    });
    if(response.ok){
        alert("The project has saved succesfully");
        document.getElementById("name").textContent="";
        document.getElementById("startdate").textContent="";
        document.getElementById("enddate").textContent="";
    }
    else alert("something went wrong, please try again")
}
async function LogOut(){
    window.localStorage.removeItem("tokenKey");
    window.location.href="authentification.html"
}
async function openTasks(){
    window.location.href="tasks.html";
}