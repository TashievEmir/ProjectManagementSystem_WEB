window.onload=()=>{
    fetch(`https://localhost:44345/api/data/GetTasks/${JSON.parse(window.localStorage.getItem('tokenKey')).username}`)
        .then(response => response.json())
        .then(data => {
            let tasks = document.querySelector("tbody");
            data.forEach(pr => {
                let tr = document.createElement("tr");
                let taskName = document.createElement("td");
                taskName.textContent = `${pr.name}`
                tr.appendChild(taskName)

                let taskManager = document.createElement("td");
                taskManager.textContent = `${pr.manager}`
                tr.appendChild(taskManager)

                let taskStatus = document.createElement("td");
                taskStatus.textContent = `${pr.status}`
                tr.appendChild(taskStatus)

                let taskStartDate = document.createElement("td");
                taskStartDate.textContent = `${pr.startDate}`
                tr.appendChild(taskStartDate)

                let taskEndDate = document.createElement("td");
                taskEndDate.textContent = `${pr.endDate}`
                tr.appendChild(taskEndDate)

                let taskProject = document.createElement("td");
                taskProject.textContent = `${pr.projectId}`
                tr.appendChild(taskProject)

                tasks.appendChild(tr);
            });
        })
        .catch(error => console.error(error));
}
async function addProject(){
    let roleuser=JSON.parse(window.localStorage.getItem('tokenKey')).role;
    console.log(roleuser);
    if(roleuser=="employee"){
        alert("you don't have permission");
    }
    else
        window.location.href="addProject.html";
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
async function openProjects(){
    window.location.href="projects.html";
}
async function LogOut(){
    window.localStorage.removeItem("tokenKey");
    window.location.href="authentification.html";
}

