window.onload=()=>{
    fetch(`https://localhost:44345/api/data/GetTasks/${JSON.parse(window.localStorage.getItem('tokenKey')).username}`)
        .then(response => response.json())
        .then(data => {
            let tasks = document.querySelector("tbody");
            data.forEach(pr => {
                let tr = document.createElement("tr");

                let taskId = document.createElement("td");
                taskId.textContent = `${pr.id}`;
                tr.appendChild(taskId);

                let taskName = document.createElement("td");
                taskName.textContent = `${pr.name}`
                tr.appendChild(taskName)

                let taskManager = document.createElement("td");
                taskManager.textContent = `${pr.manager}`
                tr.appendChild(taskManager)

                let taskStatus = document.createElement("td");
                taskStatus.textContent = `${pr.status}`;
                taskStatus.setAttribute('id','status');
                taskStatus.addEventListener("click", function() {
                    taskStatus.contentEditable = true;
                  });
                tr.appendChild(taskStatus)

                let taskStartDate = document.createElement("td");
                taskStartDate.textContent = `${pr.startDate}`;
                tr.appendChild(taskStartDate);

                let taskEndDate = document.createElement("td");
                taskEndDate.textContent = `${pr.endDate}`;
                tr.appendChild(taskEndDate);

                let taskProject = document.createElement("td");
                taskProject.textContent = `${pr.projectId}`;
                tr.appendChild(taskProject);
                
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

const myMap = new Map();
myMap.set('notStarted', 1);
myMap.set('inProcess', 2);
myMap.set('finished', 3);

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

  let response= await fetch(`https://localhost:44345/api/data/GetUserByEmail/${JSON.parse(window.localStorage.getItem('tokenKey')).username}`,{
      method:"Get",
      headers:{
          "Content-Type":"application/json"
      }
  });
  const user = await response.json();

  response= await fetch(`https://localhost:44345/api/data/GetUserId/${rowData[2]}`,{
      method:"Get",
      headers:{
          "Content-Type":"application/json"
      }
  });
  const manager = await response.json();

fetch(`https://localhost:44345/api/data/UpdateTask`, {
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
  manager: manager.id,
  userId: user.id,
  projectId: rowData[6]
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
      
      fetch(`https://localhost:44345/api/data/DeleteTask/${rowData[0]}`, {
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

