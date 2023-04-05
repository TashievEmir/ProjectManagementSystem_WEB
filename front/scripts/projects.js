window.onload=()=>{
    fetch(`https://localhost:44345/api/data/GetProjectsAsManager/${JSON.parse(window.localStorage.getItem('tokenKey')).username}`)
        .then(response => response.json())
        .then(data => {
            let projects = document.getElementById("projects");
            data.forEach(pr => {
                let tr=document.createElement("tr");
                let projectName = document.createElement("td");
                projectName.text = `${pr.name}`
                projectName.value = `${pr.id}`
                tr.appendChild(projectName)
                let projectManager = document.createElement("td");
                projectManager.text = `${pr.name}`
                projectManager.value = `${pr.id}`
                tr.appendChild(projectManager)
                let projectStatus = document.createElement("td");
                projectStatus.text = `${pr.name}`
                projectStatus.value = `${pr.id}`
                tr.appendChild(projectStatus)
                let projectStartDate = document.createElement("td");
                projectStartDate.text = `${pr.name}`
                projectStartDate.value = `${pr.id}`
                tr.appendChild(projectStartDate)
                let projectEndDate = document.createElement("td");
                projectEndDate.text = `${pr.name}`
                projectEndDate.value = `${pr.id}`
                tr.appendChild(projectEndDate)
                let projectMembers = document.createElement("td");
                projectMembers.text = `${pr.name}`
                projectMembers.value = `${pr.id}`
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
                //projectName.value = `${pr.id}`
                tr.appendChild(projectName)
                let projectManager = document.createElement("td");
                projectManager.textContent = `${pr.manager}`
                //projectManager.value = `${pr.manager}`
                tr.appendChild(projectManager)
                let projectStatus = document.createElement("td");
                projectStatus.textContent = `${pr.status}`
                //projectStatus.value = `${pr.status}`
                tr.appendChild(projectStatus)
                let projectStartDate = document.createElement("td");
                projectStartDate.textContent = `${pr.startdate}`
                //projectStartDate.value = `${pr.startdate}`
                tr.appendChild(projectStartDate)
                let projectEndDate = document.createElement("td");
                projectEndDate.textContent = `${pr.enddate}`
                //projectEndDate.value = `${pr.enddate}`
                tr.appendChild(projectEndDate)
                let projectMembers = document.createElement("td");
                projectMembers.textContent = `${pr.name}`
                //projectMembers.value = `${pr.id}`
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