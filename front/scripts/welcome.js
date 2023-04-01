window.onload=()=>{

}
async function openProjects(){
    window.location.href="projects.html";
}
async function addProject(){
    window.location.href="addProject.html";
    window.history.forward();
    function preventBack() {
        window.history.forward();
    }
    setTimeout("preventBack()", 0);
    window.onunload = function () { null };
}
async function addTask(){
    window.location.href="addTask.html";
}
async function LogOut(){
    window.location.href="authentification.html";
}