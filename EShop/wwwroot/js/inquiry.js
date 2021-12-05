
var dataTable;

$(document).ready(function () {
    //$('#myTable').DataTable();
    loadDataTable("GetInquiryList");
});

function loadDataTable(url) {
    $('#tblData').DataTable({
        "ajax": {
            "url":"/inquiry/"+url
        },
        "columns": [
            {
                "data":"id","width":"10%"
            },
            {
                "data": "fullName", "width": "15%"
            },
            {
                "data": "phoneNumber", "width": "15%"
            },
            {
                "data": "email", "width": "15%"
            },

            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Inquiry/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                            </div>
                           `;
                },
                "width":"5%"

            },
        ]
    });
}