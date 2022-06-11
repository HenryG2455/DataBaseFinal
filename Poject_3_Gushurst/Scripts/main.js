var Sortable = {
    baseUrl: '',
    sortBy: 0,
    searchTerm: '',
    sort(sortBy) {
        window.location.href = Sortable.baseUrl + "?sortBy=" + sortBy;
    },
    search() {
        var searchKey = $('#txtSearch').val();
        window.location.href = Sortable.baseUrl + "?id=" + searchKey;
    }
};
var apiHandler = {
    Get(url) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (res) {
                debugger
            }
        })
    }, POST(url, object) {
        object = {
            Id: 5,
            Name: "asd"
        }
        $.ajax({
            url: url,
            type: 'GET',
            success: function (res) {
                debugger
            }
        })
    }, DELETE(url) {
        if (confirm("Are you sure you want to Delete?")) {
            $.ajax({
                url: url,
                type: "GET",
                success: function (res) {
                    debugger
                }
            })
        } else {
            alert("Lucky we asked!")
        }
        
    }
}