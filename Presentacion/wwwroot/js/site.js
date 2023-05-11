
// Este script realiza el formato de la tabla
$(document).ready(function () {
    $('#DtUsuario').DataTable();
});

// Realiza la exportacion a Excel
var $btnDLtoExcel = $('#exportar');
$btnDLtoExcel.on('click', function () {
    $("#DtUsuario").excelexportjs({
        containerid: "DtUsuario"
        , datatype: 'table'
    });

});

//este fragmento se realiza toda la funcion de Adicionar, Modificar y Eliminar
$(function () {

    var placeHolder = $('#PlaceHolder');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        var decodeUrl = decodeURIComponent(url);
        $.get(decodeUrl).done(function (data) {
            placeHolder.html(data);
            placeHolder.find('.modal').modal('show');
        })
    });


    placeHolder.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var url = "/Usuario/" + actionUrl;
        var sendData = form.serialize();
        $.post(url, sendData).done(function (data) {
            placeHolder.find('.modal').modal('hide');
            window.location.href = "/Usuario/Index";
        });
    })
});
