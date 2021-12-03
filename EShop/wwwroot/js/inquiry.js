
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
            }]
    });
}