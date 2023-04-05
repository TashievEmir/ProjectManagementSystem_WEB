window.onload=()=>{

}
async function openProjects(){
    window.location.href="projects.html";
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
async function LogOut(){
    window.localStorage.removeItem("tokenKey");
    window.location.href="authentification.html";
}
async function openTasks(){
    window.location.href="tasks.html";
}