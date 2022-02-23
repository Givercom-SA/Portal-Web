// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ShowAlertMensage(idcontainer, type, title, body, autoclose, callback) {
    var div = document.createElement('div');
    div.className = "alert";
    switch (type) {
        case 'danger': div.classList.add('alert-danger'); break;
        case 'warning': div.classList.add('alert-warning'); break;
        case 'success': div.classList.add('alert-success'); break;
    }
    if (title != "")
        div.innerHTML = "<strong>" + title + "</strong>";
    div.innerHTML += body;
    document.getElementById(idcontainer).innerHTML = "";
    document.getElementById(idcontainer).appendChild(div);
    if (autoclose > 0) {
        setTimeout(function () {
            document.getElementById(idcontainer).innerHTML = "";
            if (callback) callback();
        }, autoclose*1000);
    }
}



function CambiarPerfil(IdPerfil) {
    $("#IdPerfilSesion").val(IdPerfil);
    $("#frmCambiarPerfil").submit();
}

function CambiarEmpresa(CodigoEmpresa) {
    $("#CodigoEmpresa").val(CodigoEmpresa);
    $("#frmCambiarEmpresa").submit();
}

function SeleccionarEmpresa(CodigoEmpresa) {
    $("#CodigoEmpresa").val(CodigoEmpresa);
    $("#frmCambiarEmpresa").submit();
}
