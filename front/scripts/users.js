let password;
window.onload=()=>{

    fetch(`https://localhost:44345/api/data/GetUsers`)
        .then(response => response.json())
        .then(data => {
            let tasks = document.querySelector("tbody");
            data.forEach(pr => {

                password=pr.password;
                let tr = document.createElement("tr");

                let taskId = document.createElement("td");
                taskId.textContent = `${pr.id}`;
                tr.appendChild(taskId);

                let taskName = document.createElement("td");
                taskName.textContent = `${pr.name}`
                tr.appendChild(taskName)

                let userSurname = document.createElement("td");
                userSurname.textContent = `${pr.surname}`
                tr.appendChild(userSurname)

                let taskStatus = document.createElement("td");
                taskStatus.textContent = mapToPage.get(`${pr.role}`);
                taskStatus.setAttribute('id','status');
                taskStatus.addEventListener("click", function() {
                    taskStatus.contentEditable = true;
                  });
                tr.appendChild(taskStatus)

                let userEmail = document.createElement("td");
                userEmail.textContent = `${pr.email}`;
                tr.appendChild(userEmail);
                
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

                tr.children[0].style.backgroundColor = 'white'; tr.children[1].style.backgroundColor = 'white';
                tr.children[2].style.backgroundColor = 'white'; tr.children[3].style.backgroundColor = 'white';
                tr.children[4].style.backgroundColor = 'white'; tr.children[5].style.backgroundColor = 'white';
                tr.children[6].style.backgroundColor = 'white';

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

const mapToDb = new Map();
mapToDb.set('manager', 1);
mapToDb.set('employee', 2);

const mapToPage = new Map();
mapToPage.set('1', 'manager');
mapToPage.set('2', 'employee');

async function Update(button)
{
      var row = button.parentNode.parentNode;
      var cells = row.getElementsByTagName("td");
      var rowData = [];
      for (var i = 0; i < cells.length; i++) {
      rowData.push(cells[i].textContent);
      }

fetch(`https://localhost:44345/api/data/UpdateUser`, {
method: 'PUT',
headers: {
  'Content-Type': 'application/json'
},
body: JSON.stringify({
  id: rowData[0],
  name: `${rowData[1]}`,
  surname: `${rowData[2]}`,
  role: mapToDb.get(`${rowData[3]}`),
  email: `${rowData[4]}`,
  password: password
})
})
.then(response => {
if (!response.ok) {
  throw new Error('Network response was not ok');
}
alert("the data updated succesfully");
location.reload();
})
.catch(error => {
console.error('There was a problem with the fetch request:', error);
});    
  
}

async function Delete(button){

    var row = button.parentNode.parentNode;
      var cells = row.getElementsByTagName("td");
      var rowData = [];
      for (var i = 0; i < cells.length; i++) {
      rowData.push(cells[i].textContent);
      }
      
      fetch(`https://localhost:44345/api/data/DeleteUser/${rowData[0]}`, {
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

