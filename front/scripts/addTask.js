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

    let projects = document.getElementById("projects");
    fetch('https://localhost:44345/api/adding/getprojects')
        .then(response => response.json())
        .then(prs => {
            prs.forEach(pr => {
                let project = document.createElement("option");
                project.text = `${pr.name}`
                project.value = `${pr.id}`
                projects.add(project);
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

async function openTasks(){
    window.location.href="tasks.html";
}

async function Save(){
    let data = {
        "name":`${document.getElementById("name").value}`,
        "manager":`${document.getElementById("managers").value}`,
        "status":`1`,
        "startdate":`${document.getElementById("startdate").value}`,
        "enddate":`${document.getElementById("enddate").value}`,
        "userid":`${document.getElementById("members").value}`,
        "projectid":`${document.getElementById("projects").value}`
    }
    const response= await fetch(`https://localhost:44345/api/adding/savetask`,{
        method:"POST",
        headers:{"Content-Type":"application/json"},
        body: JSON.stringify(data)
    }).catch(function (error) {
        console.log(error);
    });
    if(response.ok){ alert("The task has saved succesfully"); }
    else alert("something went wrong, please try again")
}

async function LogOut(){
    window.localStorage.removeItem("tokenKey");
    window.location.href="authentification.html"
}