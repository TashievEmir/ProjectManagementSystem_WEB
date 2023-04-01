window.onload = () => {
    let members = document.getElementById("members");
    debugger
    fetch('https://localhost:44345/api/adding/get')
        .then(response => response.json())
        .then(users => {
            users.forEach(user => {
                let member = document.createElement("option");
                member.text = `${user.name}`
                member.value = `${user.name}`
                debugger
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
                manager.value = `${user.name}`
                debugger
                managers.add(manager);
            });
        })
        .catch(error => console.error(error));

};
async function openProjects(){
    window.location.href="projects.html";
}
async function addProject(){
    window.location.href="addProject.html";

}
async function addTask(){
    window.location.href="addTask.html";
}
async function Save(){
    let data = {
            "name":`${document.getElementById("name").value}`,
            "manager":`${document.getElementById("managers").value}`,
            "startdate":`${document.getElementById("startdate").value}`,
            "enddate":`${document.getElementById("enddate").value}`,
            "status":`${document.getElementById("status").value}`,
            "members":`${document.getElementById("members").value}`
    }
    const response= await fetch(`https://localhost:44345/api/adding/saveproject`,{
        method:"POST",
        headers:{"Content-Type":"application/json"},
        body: JSON.stringify(data)
    });
    if(response.ok){alert("The project has saved succesfully");}
    else alert("something went wrong, please try again")
}
async function LogOut(){
    window.location.href="authentification.html"
}