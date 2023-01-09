var items = null;

function filterItems() {
    console.log('loading items...');
    $.ajax({
        url: "../api/items/filter",
        type: "get",
        data: {
            name: $('#nameSearch').val(),
            model: $('#modelSearch').val(),
            make: $('#makeSearch').val(),
        },
        success: function (data) {
            items = data;
            let table = '<table class="table table-border table-striped" style="width:100%">';
            table += '<thead><tr><th>Image</th><th>Item Name</th><th>Model</th><th>Make</th><th>Price</th><th>City</th><th>Date created</th><th></th></tr ></thead ><tbody class="border">';
            for (element in data) {
                var mainImage;
                for (image in data[element].images) {
                    if (data[element].images[image].isMainImage)
                        mainImage = data[element].images[image];
                }
                let center = ' <td width="10%"><img src=' + mainImage.url + ' class="w-100" /></td>';
                center += '<td width="15%">' + data[element].title + '</td>';
                center += '<td width="20%">' + data[element].model + '</td>';
                center += '<td width="10%">' + data[element].make + '</td>';
                center += '<td width="10%">' + data[element].price + '.00 RSD</td>'
                center += '<td width="10%">' + data[element].city + '</td>'
                center += '<td width="10%">' + new Date(data[element].created).toISOString().split('T')[0] + '</td>'
                center += '<td>' +
                    '<div class="w-75 btn-group" role="group">' +
                    '<a class="btn btn-success mx-2" href="/Item/Details?id=' + data[element].id + '">Details</a>' +
                    '</div>' +
                    '</td>';

                table += '<tr>' + center + '</tr>';
            }
            table += '</tbody></table>';
            $('#content').html(table);
            console.log('items loaded');
        },
    });
}
function loaditems() {
    console.log('loading items...');
    $.ajax({
        url: "../api/items",
        type: "get",
        success: function (data) {
            items = data;
            let table = '<table class="table table-border table-striped" style="width:100%">';
            table += '<thead><tr><th>Image</th><th>Item Name</th><th>Model</th><th>Make</th><th>Price</th><th>City</th><th>Date created</th><th></th></tr ></thead ><tbody class="border">';
            for (element in data) {
                var mainImage;
                for (image in data[element].images) {
                    if (data[element].images[image].isMainImage)
                        mainImage = data[element].images[image];
                }
                let center = ' <td width="10%"><img src=' + mainImage.url + ' class="w-100" /></td>';
                center += '<td width="15%">' + data[element].title + '</td>';
                center += '<td width="20%">' + data[element].model + '</td>';
                center += '<td width="10%">' + data[element].make + '</td>';
                center += '<td width="10%">' + data[element].price + '.00 RSD</td>'
                center += '<td width="10%">' + data[element].city + '</td>'
                center += '<td width="10%">' + new Date(data[element].created).toISOString().split('T')[0] + '</td>'
                center += '<td>' +
                    '<div class="w-75 btn-group" role="group">' +
                    '<a class="btn btn-success mx-2" href="/Item/Details?id=' + data[element].id + '">Details</a>' +
                    '</div>' +
                    '</td>';

                table += '<tr>' + center + '</tr>';
            }
            table += '</tbody></table>';
            $('#content').html(table);
            console.log('items loaded');
        },
    });
}

function sortByModel(order) {
    if (order == 1)
        items.sort((a, b) => (a.Model > b.Model) ? 1 : -1);
    else if (order == 0)
        items.sort((a, b) => (a.Model < b.Model) ? 1 : -1);
    tableReload();
}
function sortByMake(order) {
    if (order == 1)
        items.sort((a, b) => (a.Make > b.Make) ? 1 : -1);
    else if (order == 0)
        items.sort((a, b) => (a.Make < b.Make) ? 1 : -1);
    tableReload();
}

function sortByName(order) {
    if (order == 1)
        items.sort((a, b) => (a.title.toLowerCase() > b.title.toLowerCase()) ? 1 : -1);
    else if (order == 0)
        items.sort((a, b) => (a.title.toLowerCase() < b.title.toLowerCase()) ? 1 : -1);
    tableReload();
}

function sortByPrice(order) {
    if (order == 1)
        items.sort((a, b) => (a.price > b.price) ? 1 : -1);
    else if (order == 0)
        items.sort((a, b) => (a.title < b.price) ? 1 : -1);
    tableReload();
}
function tableReload() {
    data = items;
    let table = '<table class="table table-border table-striped" style="width:100%">';
    table += '<thead><tr><th>Image</th><th>Item Name</th><th>Model</th><th>Make</th><th>Price</th><th>City</th><th>Date created</th><th></th></tr ></thead ><tbody class="border">';
    for (element in data) {
        var mainImage;
        for (image in data[element].images) {
            if (data[element].images[image].isMainImage)
                mainImage = data[element].images[image];
        }
        let center = ' <td width="10%"><img src=' + mainImage.url + ' class="w-100" /></td>';
        center += '<td width="15%">' + data[element].title + '</td>';
        center += '<td width="20%">' + data[element].model + '</td>';
        center += '<td width="10%">' + data[element].make + '</td>';
        center += '<td width="10%">' + data[element].price + '.00 RSD</td>'
        center += '<td width="10%">' + data[element].city + '</td>'
        center += '<td width="10%">' + new Date(data[element].created).toISOString().split('T')[0] + '</td>'
        center += '<td>' +
            '<div class="w-75 btn-group" role="group">' +
            '<a class="btn btn-success mx-2" href="/Item/Details?id=' + data[element].id + '">Details</a>' +
            '</div>' +
            '</td>';

        table += '<tr>' + center + '</tr>';
    }
    table += '</tbody></table>';
    $('#content').html(table);
}

$(document).ready(function () {
    $('#order').change(function () {
        var orderBy = $(this).find("option:selected").attr("value");

        if (orderBy == 'orderByMakeA') {
            sortByMake(1);
        }
        else if (orderBy == 'orderByMakeD') {
            sortByMake(0);
        }
        else if (orderBy == 'orderByNameA') {
            sortByName(1);
        }
        else if (orderBy == 'orderByNameD') {
            sortByName(0);
        }
        else if (orderBy == 'orderByModelA') {
            sortByModel(1);
        }
        else if (orderBy == 'orderByModelD') {
            sortByModel(0);
        }
        else if (orderBy == 'orderByPriceA') {
            sortByPrice(1);
        }
        else if (orderBy == 'orderByPriceD') {
            sortByPrice(0);
        }
        else if (orderBy == 'no-order') {
            filterItems();
        }
    });
    loaditems();
});

$(document).on('click', '#search', function () {
    filterItems();
})

$(document).on('click', '#cancle', function () {
    $('#nameSearch').val(null);
    $('#modelSearch').val(null);
    $('#makeSearch').val(null);
    $('#order').prop('selectedIndex', 0);
    filterItems();
})