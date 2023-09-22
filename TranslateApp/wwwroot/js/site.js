var dataTable;

$(document).ready(function () {
    loadDataTable();

    // Check if it's an AJAX request
    if ($.fn.dataTable && Request.Headers["X-Requested-With"] == "XMLHttpRequest") {
        loadDataTable();
    }
});

function loadDataTable() {
    dataTable = $('#loadHistoryLog').DataTable({
        "ajax": {
            "url": "/Translation/Index/",
            "type": "GET",
            "datatype": "json",
        },
        "columns": [
            {
                "data": "timestamp", "width": "20%",
                "render": function (data) {
                    // Format the date to display only the date part
                    return moment(data).format('MMMM D, YYYY');
                }
            },
            { "data": "text", "width": "30%" },
            { "data": "translated", "width": "50%" }
        ],
        "language": {
            "emptyTable": "no data available"
        },
        "width": "100%"
    });


    dataTable = $('#loadUserData').DataTable({
        "ajax": {
            "url": "/Users/Index/",
            "type": "GET",
            "datatype": "json",
        },
        "columns": [
            { "data": "firstName", "width": "25%" },
            { "data": "lastName", "width": "25%" },
            { "data": "email", "width": "25%" },
            { "data": "userRole", "width": "25%" }
        ],
        "language": {
            "emptyTable": "no data available"
        },
        "width": "100%"
    });
}