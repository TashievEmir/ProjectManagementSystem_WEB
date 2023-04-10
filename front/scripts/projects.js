window.onload=()=>{
    /*fetch(`https://localhost:44345/api/data/GetProjectsAsManager/${JSON.parse(window.localStorage.getItem('tokenKey')).username}`)
        .then(response => response.json())
        .then(data => {
            let projects = document.getElementById("projects");
            data.forEach(pr => {
                let tr=document.createElement("tr");

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
                projectStartDate.textContent = `${pr.startDate}`
                tr.appendChild(projectStartDate)

                let projectEndDate = document.createElement("td");
                projectEndDate.textContent = `${pr.endDate}`
                tr.appendChild(projectEndDate)

                let projectMembers = document.createElement("td");
                let projectMembersSelect=document.createElement("select");
                if(pr.members){
                    pr.members.forEach(x=>{
                        let projectMembersOption=document.createElement("option");
                        projectMembersOption.text=`${x.name}`
                        projectMembersOption.value = `${x.id}`
                        projectMembersSelect.appendChild(projectMembersOption)
                    })
                }

                projectMembers.appendChild(projectMembersSelect)
                tr.appendChild(projectMembers)
                projects.appendChild(tr);
            });
        })
        .catch(error => console.error(error));*/

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
                projectStartDate.textContent = `${pr.startDate}`
                tr.appendChild(projectStartDate)

                let projectEndDate = document.createElement("td");
                projectEndDate.textContent = `${pr.endDate}`
                tr.appendChild(projectEndDate)

                let projectMembers = document.createElement("td");
                let projectMembersSelect=document.createElement("select");

                if(pr.members){
                    pr.members.forEach(x=>{
                        let projectMembersOption=document.createElement("option");
                        projectMembersOption.text=`${x.name}`
                        projectMembersOption.value = `${x.id}`
                        projectMembersSelect.appendChild(projectMembersOption)
                    })
                }

                projectMembers.appendChild(projectMembersSelect)
                tr.appendChild(projectMembers)

                projects.appendChild(tr);
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
async function openTasks(){
    window.location.href="tasks.html";
}
async function LogOut(){
    window.localStorage.removeItem("tokenKey");
    window.location.href="authentification.html";
}