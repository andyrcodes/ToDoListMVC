// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    PrepareLocalStorage();

    let allTasks = GetLocalStorage();
    //Set task count
    SetTaskCountLabel(`ALL TASKS (${allTasks.length})`);

    //This is my first change where I am trying to pass the entire array into ListTasks...
    //Not even sure if it works yet...However if it does I am in business!
    ListTasks(allTasks);

    //Trigger tooltips on hover
    $('[data-toggle="tooltip"]').tooltip({
        trigger: 'hover'
    })

    $("#btnSearch").on("click", function () {
        let searchString = $("#txtSearch").val();
        SearchTasks(searchString);
    })
});


function PrepareLocalStorage() {
    if (GetLocalStorage() == null)
        SetLocalStorage([]);
}

function CreateTask(formData) {

    let dueDate = formData[2].value == "" ?
        new Date() :
        new Date(`${formData[2].value} 00:00`);

    let task = {
        id: GenerateSimpleId(),
        created: new Date(),
        completed: false,
        title: formData[1].value,
        dueDate: dueDate
    }

    let tasks = GetLocalStorage();
    tasks.push(task);

    SetLocalStorage(tasks);
    SetTaskCountLabel(`ALL TASKS (${tasks.length})`);
    ListTasks(tasks);
}


function SaveTask(task) {
    let taskData = GetLocalStorage();
    taskData.tasks.push(task);
    SetLocalStorage(taskData);
}

function ListTasks(tasks) {
    const template = document.getElementById("taskItem-template");
    const eventBody = document.getElementById("taskBody");

    eventBody.innerHTML = "";
    for (var row = 0; row < tasks.length; row++) {
        const taskRow = document.importNode(template.content, true);

        if (tasks[row].completed) {
            taskRow.getElementById("TaskRow").setAttribute("class", "complete");
        }

        taskRow.getElementById("id").textContent = tasks[row].id;
        taskRow.getElementById("title").textContent = tasks[row].title;
        taskRow.getElementById("created").textContent = RenderDate(tasks[row].created);
        taskRow.getElementById("dueDate").textContent = RenderDate(tasks[row].dueDate);
        taskRow.getElementById("tdCrud").setAttribute("data-id", tasks[row].id)

        taskBody.appendChild(taskRow);
    }
}


function DeleteTask(element) {
    ClearTooltip();

    let index = GetIndex(element);
    let tasks = GetLocalStorage();
    tasks.splice(index, 1);
    SetLocalStorage(tasks);

    SetTaskCountLabel(`ALL TASKS (${tasks.length})`)
    ListTasks(GetLocalStorage());
}

function CompleteTask(element) {
    ClearTooltip();

    let taskId = GetTaskId(element);
    let tasks = GetLocalStorage();
    let task = tasks.find(t => t.id == taskId);
    task.completed = true;

    SetLocalStorage(tasks);
    ListTasks(GetLocalStorage());
}

//The element in this function is most likely a button
function GetIndex(element) {
    //I am passing a piece of structure (i.e a button) and trying to get some data out    
    let taskId = GetTaskId(element);

    //Get a reference to local storage
    let tasks = GetLocalStorage();

    //findIndex is a built-in JS function that returns the 0 based position in the array
    return tasks.findIndex(t => t.id == taskId);
}


//This is overcoded to prove it could be done
//The use of GUIDs is typically handled by the code/database
function GenerateId() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0,
            v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function GenerateSimpleId() {
    let tasks = GetLocalStorage();
    let testId = tasks.length;
    //indexOf() returns the index of what you are testing if found
    //if it does not find a match it returns -1
    //knowing that a -1 means the id is not being used
    //we can test against that number
    //while loops run until the condition inside the () is no longer true
    while (tasks.indexOf(t => t.id == testId) >= 0) {
        //if the id was found in the array increase the number by one
        //then check again
        testId++;
    }

    return testId;
}

function GetTaskCount(tasks) {
    return tasks.length;
}

function RenderDate(dateString) {
    let date = new Date(dateString);
    let options = {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    };
    return date.toLocaleDateString("en-US", options)
}

function GetTaskId(element) {
    // let taskId = $(element).parent().attr("data-id");
    let taskId = element.parentNode.getAttribute("data-id");
    return taskId;
}

function GetTask(element) {
    let taskId = GetTaskId(element);
    return tasks.find(t => t.id == taskId);
}

function GetLocalStorage() {
    return JSON.parse(localStorage.getItem("TaskData"));
}

function SetLocalStorage(data) {
    localStorage.setItem("TaskData", JSON.stringify(data));
}

function ClearTooltip() {
    $("div.tooltip").hide();
}

function TriggerCustomAlert(title, message) {

    const swalWithDarkButton = Swal.mixin({
        showConfirmButton: false,
        imageUrl: '/img/tasks.jpg',
        imageHeight: 150,
        closeOnConfirm: false,
        closeOnCancel: false,
        allowOutsideClick: false
        // timer: 1500,
        // timerProgressBar: true,            
        // didOpen: () => {
        //     Swal.showLoading()
        //     timerInterval = setInterval(() => {
        //         const content = Swal.getContent()
        //     }, 100)
        // },
        // willClose: () => {
        //     clearInterval(timerInterval)
        // }
    });

    swalWithDarkButton.fire({
        title: `<span class="cool-text">${title}</span>`,
        html: `<span class="font-weight-bold">${message}</span>`
    });
}

function PopEditModal(element) {
    let tasks = GetLocalStorage();
    let taskId = GetTaskId(element);
    let task = tasks.find(t => t.id == taskId);

    // $("#TaskId").val(task.id);
    document.getElementById("TaskId").value = task.id

    $("#NewTitle").val(task.title);

    //Sweet works!
    let modalDueDate = BuildModalDueDate(task.dueDate);
    $("#NewDueDate").val(modalDueDate);
    $("#editTaskItem").modal("show");

    alert("Let's see");

}

function ClearTasks() {
    SetLocalStorage(new Array());
    ListTasks();
}

function BuildModalDueDate(dueDate) {
    let shortDate = dueDate.split("T")[0];
    return shortDate;
}

function SetTaskCountLabel(message) {
    document.getElementById("taskCount").innerText = message;
}

function SearchTasks(searchString) {
    searchString = searchString.toLowerCase();
    let allTasks = GetLocalStorage();
    let tasks = allTasks.filter(t => t.title.toLowerCase().includes(searchString));
    SetTaskCountLabel(`TASKS FOUND (${tasks.length})`);
    ListTasks(tasks);
}