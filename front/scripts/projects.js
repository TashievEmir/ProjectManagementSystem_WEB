window.onload=()=>{
    fetch(`https://localhost:44345/api/data/GetProjects/${JSON.parse(window.localStorage.getItem('tokenKey')).username}`)
        .then(response => response.json())
        .then(data => {
            let projects = document.querySelector("tbody");
            data.forEach(pr => {
                let tr = document.createElement("tr");
                
                let projectId = document.createElement("td");
                projectId.textContent = `${pr.id}`;
                tr.appendChild(projectId);

                let projectName = document.createElement("td");
                projectName.textContent = `${pr.name}`
                tr.appendChild(projectName)

                let projectManager = document.createElement("td");
                projectManager.textContent = `${pr.manager}`
                tr.appendChild(projectManager)

                let projectStatus = document.createElement("td");
                projectStatus.textContent = `${pr.status}`;
                projectStatus.setAttribute('id','status');
                projectStatus.addEventListener("click", function() {
                    projectStatus.contentEditable = true;
                  });
                tr.appendChild(projectStatus);

                let projectStartDate = document.createElement("td");
                projectStartDate.textContent = `${pr.startDate}`;
                projectStartDate.addEventListener("click", function() {
                    projectStatus.contentEditable = true;
                  });
                tr.appendChild(projectStartDate);

                let projectEndDate = document.createElement("td");
                projectEndDate.textContent = `${pr.endDate}`;
                projectEndDate.addEventListener("click", function() {
                    projectStatus.contentEditable = true;
                  });
                tr.appendChild(projectEndDate);

                let projectMembers = document.createElement("td");
                let projectMembersSelect=document.createElement("select");

                if(pr.members){
                    pr.members.forEach(x=>{
                        let projectMembersOption=document.createElement("option");
                        projectMembersOption.text=`${x.name}`
                        projectMembersOption.value = `${x.id}`
                        projectMembersSelect.appendChild(projectMembersOption);
                    })
                }
                projectMembers.appendChild(projectMembersSelect);
                tr.appendChild(projectMembers);

                let updateTd = document.createElement("td");
                let updateBtn=document.createElement("button");
                updateBtn.textContent="Update";
                updateBtn.classList.add("row-btn");
                updateBtn.onclick=function() {
                    Update(this);
                };
                updateTd.appendChild(updateBtn);
                tr.appendChild(updateTd);

                let deleteTd = document.createElement("td");
                let deleteBtn=document.createElement("button");
                deleteBtn.textContent="Delete";
                deleteBtn.classList.add("row-btn");
                deleteBtn.onclick=function() {
                    Delete(this);
                };
                deleteTd.appendChild(deleteBtn);
                tr.appendChild(deleteTd);

                projects.appendChild(tr);
            });
        })
        .catch(error => console.error(error));

}
document.onload=()=>{
    var table = document.getElementById("projects");
    var rows = table.getElementsByTagName("tr");
    let currentDate = new Date().toJSON().slice(0, 10);
    debugger
    for(let i = 1; i < rows.length; i++) {
        var statusValue = rows[i].cells[2].textContent;
        var startDate= rows[i].cells[3].textContent;
        var endDateValue = rows[i].cells[4].firstChild.nodeValue;
        if (statusValue == 'notStarted' && startDate>currentDate) {
            rows[i].style.backgroundColor = "red";
        }
        else {
            rows[i].style.backgroundColor = "green";
        }
    }
}
setTimeout(function() {
    console.log("set fuck")
    var table = document.getElementById("projects");
    var rows = table.getElementsByTagName("tr");
    let currentDate = new Date().toJSON().slice(0, 10);
    
    for(let i = 0; i < rows.length; i++) {
        var statusValue = rows[i].cells[2].textContent;
        var startDate= rows[i].cells[3].textContent;
        var endDateValue = rows[i].cells[4].firstChild.nodeValue;
        
        if (statusValue == 'notStarted' && startDate>currentDate) {
            rows[i].style.backgroundColor = "red";
        }
        else {
            rows[i].className="green";
        }
    }
}, 2000);
document.addEventListener('load', function() {
    console.log("load fuck")
    var table = document.getElementById("projects");
    var rows = table.getElementsByTagName("tr");
    let currentDate = new Date().toJSON().slice(0, 10);
    debugger
    for(let i = 1; i < rows.length; i++) {
        var statusValue = rows[i].cells[2].textContent;
        var startDate= rows[i].cells[3].textContent;
        var endDateValue = rows[i].cells[4].firstChild.nodeValue;
        if (statusValue == 'notStarted' && startDate>currentDate) {
            rows[i].style.backgroundColor = "red";
        }
        else {
            rows[i].style.backgroundColor = "green";
        }
    }
});

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

const myMap = new Map();
myMap.set('notStarted', '1');
myMap.set('inProcess', '2');
myMap.set('finished', '3');

async function Update(button){
    let roleuser=JSON.parse(window.localStorage.getItem('tokenKey')).role;
    console.log(roleuser);
    if(roleuser=="employee"){
        alert("you don't have permission");
    }
    else{
        var row = button.parentNode.parentNode;
        var cells = row.getElementsByTagName("td");
        var rowData = [];
        for (var i = 0; i < cells.length; i++) {
        rowData.push(cells[i].textContent);
        }

    const response= await fetch(`https://localhost:44345/api/data/GetUserId/${rowData[2]}`,{
        method:"Get",
        headers:{
            "Content-Type":"application/json"
        }
    });
    const data = await response.json();

  fetch(`https://localhost:44345/api/data/UpdateProject`, {
  method: 'PUT',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    id: rowData[0],
    name: `${rowData[1]}`,
    status: myMap.get(`${rowData[3]}`),
    startDate: `${rowData[4]}`,
    endDate: `${rowData[5]}`,
    manager: data.id
  })
})
.then(response => {
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  alert("the data updated succesfully");
})
.catch(error => {
  console.error('There was a problem with the fetch request:', error);
});    
    }
}

async function Delete(button){
    let roleuser=JSON.parse(window.localStorage.getItem('tokenKey')).role;
    console.log(roleuser);
    if(roleuser=="employee"){
        alert("you don't have permission");
    }
    else{
        var row = button.parentNode.parentNode;
        var cells = row.getElementsByTagName("td");
        var rowData = [];
        for (var i = 0; i < cells.length; i++) {
        rowData.push(cells[i].textContent);
        }
        fetch(`https://localhost:44345/api/data/DeleteProject/${rowData[0]}`, {
            method: 'Delete',
            headers: {
            'Content-Type': 'application/json'
            },
            })
            .then(response => {
              if (!response.ok) {
              throw new Error('Network response was not ok');
                }
              alert("the data deleted succesfully");
              location.reload();
            })
            .catch(error => {
              console.error('There was a problem with the fetch request:', error);
            }); 
    }
}

