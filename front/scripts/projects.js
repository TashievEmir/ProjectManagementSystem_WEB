async function openProjects(){
    window.location.href="projects.html";
}
async function addProject(){
    window.location.href="addProject.html";
}
async function addTask(){
    window.location.href="addTask.html";
}
/*
* <table>
  <caption>Projects</caption>
  <thead>
    <tr>
      <th>Name</th>
      <th>Status</th>
      <th>Manager</th>
      <th>Start Date</th>
      <th>End Date</th>
      <th>Members</th>
    </tr>
  </thead>
  <tbody>
    <!-- Table rows will be dynamically populated with data from the backend -->
  </tbody>
</table>
CSS code:

css
Copy code
table {
  border-collapse: collapse;
  width: 100%;
  margin-bottom: 1rem;
}

caption {
  font-size: 1.5rem;
  font-weight: bold;
  margin-bottom: 1rem;
}

th, td {
  border: 1px solid #ccc;
  padding: 0.5rem;
  text-align: left;
}

th {
  background-color: #f2f2f2;
  font-weight: bold;
}
To populate the table with data from the backend, you can use JavaScript to fetch the data and dynamically create the table rows and cells. Here is an example using jQuery:

JavaScript code:

javascript
Copy code
$(document).ready(function() {
  $.ajax({
    url: 'backend_url_here',
    success: function(data) {
      // Loop through the data and create a new table row for each project
      $.each(data, function(index, project) {
        var row = $('<tr>');
        // Add the project data to the table cells
        row.append($('<td>').text(project.name));
        row.append($('<td>').text(project.status));
        row.append($('<td>').text(project.manager));
        row.append($('<td>').text(project.startDate));
        row.append($('<td>').text(project.endDate));
        row.append($('<td>').text(project.members));
        // Add the completed row to the table body
        $('tbody').append(row);
      });
    }
  });
});
In this example, we use the jQuery library to make an AJAX request to the backend URL and fetch the data. We then loop through the data and create a new table row for each project, using the append() function to add the project data to the table cells. Finally, we add the completed row to the table body using the append() function again.

Note that you will need to modify the JavaScript code to match the format of the data returned by your backend API.

*
* */