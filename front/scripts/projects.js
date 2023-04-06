window.onload=()=>{
    fetch(`https://localhost:44345/api/data/GetProjectsAsManager/${JSON.parse(window.localStorage.getItem('tokenKey')).username}`)
        .then(response => response.json())
        .then(data => {
            let projects = document.getElementById("projects");
            data.forEach(pr => {
                let tr=document.createElement("tr");

                let projectName = document.createElement("td");
                projectName.textContent = `${pr.name}`
                tr.appendChild(projectName)

                let projectManager = document.createElement("td");
                projectManager.textContent = `${pr.name}`
                tr.appendChild(projectManager)

                let projectStatus = document.createElement("td");
                projectStatus.textContent = `${pr.name}`
                tr.appendChild(projectStatus)

                let projectStartDate = document.createElement("td");
                projectStartDate.textContent = `${pr.name}`
                tr.appendChild(projectStartDate)

                let projectEndDate = document.createElement("td");
                projectEndDate.textContent = `${pr.name}`
                tr.appendChild(projectEndDate)

                let projectMembers = document.createElement("td");
                projectMembers.textContent = `${pr.name}`
                tr.appendChild(projectMembers)

                projects.appendChild(tr);
            });
        })
        .catch(error => console.error(error));

    fetch(`https://localhost:44345/api/data/GetProjectsAsEmployee/${JSON.parse(window.localStorage.getItem('tokenKey')).username}`)
        .then(response => response.json())
        .then(data => {
            let projects = document.querySelector("tbody");
            data.forEach(pr => {
                let tr = document.createElement("tr");

                let projectName = document.createElement("td");
                projectName.textContent = `${pr.name}`
                tr.appendChild(projectName)

                let projectManager = document.createElement("td");
                projectManager.textContent = `${pr.manager}`
                tr.appendChild(projectManager)

                let projectStatus = document.createElement("td");
                projectStatus.textContent = `${pr.status}`
                tr.appendChild(projectStatus)

                let projectStartDate = document.createElement("td");
                projectStartDate.textContent = `${pr.startdate}`
                tr.appendChild(projectStartDate)

                let projectEndDate = document.createElement("td");
                projectEndDate.textContent = `${pr.enddate}`
                tr.appendChild(projectEndDate)

                let projectMembers = document.createElement("td");
                projectMembers.textContent = `${pr.name}`
                tr.appendChild(projectMembers)
                debugger
                projects.appendChild(tr);
            });
        })
        .catch(error => console.error(error));
}
async function addProject(){
    window.location.href="addProject.html";
}
async function addTask(){
    window.location.href="addTask.html";
}
async function openTasks(){
    window.location.href="tasks.html";
}