



var productDataTable;
$(document).ready(function () {

    loadData();
});
function loadData() {
    productDataTable = $("#productTable").DataTable(
        {
            "ajax": { "url": '/Admin/Product/GetData' }
            ,
            "columns": [
                { "data": "name" },
                { "data": "description" },
                { "data": "price" },
                { "data": "category.name" },

                {
                    "data": "id",
                    "render": function (data) {
                        return `
        <a class="btn btn-success" href="/Admin/Product/Edit/${data}">Edit</a>
        <a class="btn btn-danger" onClick="deleteProduct('/Admin/Product/Delete/${data}')">Delete</a>
    `;
                    }




                }
            ]

        });
}

function deleteProduct(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            // click sweat alert ok first 
            Swal.fire({
                title: "Deleted!",
                text: "Product has been deleted.",
                icon: "success"
            });
            // then tostr alert

            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        // refesh datatable
                        productDataTable.ajax.reload(null, false);
                        toastr.success(data.message);
                    }
                    else {
                        toaster.error(data.message);
                    }
                }
            })

        }
    });

}