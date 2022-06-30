
const connectionUser = new signalR.HubConnectionBuilder().withUrl("/notificaciones")
    .configureLogging(signalR.LogLevel.Error)
    .build();

var btnNotificacionLimpiar = false;
var btnNotificacionLimpiarContador = false;

connectionUser.on("sendToUser", (titulo, mensaje, fecha) => {

    let objNotifcacion = new Object();
    objNotifcacion.creacionFecha = fecha;
    objNotifcacion.proceso ="";
    objNotifcacion.detalle = mensaje;
    objNotifcacion.titulo = titulo;



    agregarNotificaciones(objNotifcacion);

});

connectionUser.on("RecibirNotificacion", function (notificacion) {
    var mensajeInicial = notificacion.mensaje;

    let valNotificacion = $("#alertCantidad").html();
    let cantidadNotificacion =0;
    if (valNotificacion == "") {
        cantidadNotificacion = 1;
    } else {
        cantidadNotificacion = 1 + parseInt(valNotificacion);
    }
   
    agregarNotificaciones(notificacion);

});


connectionUser.start().then(function () {
    connectionUser.invoke('Matricular',strLayouCodigoUsuario)
        .catch(function (err) { return console.error(err.toString()) })
        .then(function (result) {

            $.each(result.notificaciones, function (index,value) {
                agregarNotificaciones(value);
            });

           
    })
});


$(document).ready(function ()
{
    $("#contentNotififacion").on("click", function (sender) {
        let valNotificacion = $("#alertCantidad").html();
        if (valNotificacion != "") {
            btnNotificacionLimpiarContador = true;
            LimpiarContadorNotificacion();
        }
    });

});

function LimpiarNotificacion() {

    if (btnNotificacionLimpiar) {
        connectionUser.invoke("LimpiarNotificaciones", strLayouCodigoUsuario)
            .catch(function (err) {
                console.log(err);
            });

        $('#alertCantidad').text("");
        
        btnNotificacionLimpiar = false;
    }
}

function LimpiarContadorNotificacion() {

    if (btnNotificacionLimpiarContador) {
        connectionUser.invoke("LimpiarContadorNotificaciones", strLayouCodigoUsuario)
            .catch(function (err) {
                console.log(err);
            });

        $('#alertCantidad').text("");
        btnNotificacionLimpiarContador = false;
    }
}

function agregarNotificaciones(notificacion) {


    let aAnchor = $("<a>");
    aAnchor.prop("class", "dropdown-item dropdown-notifications-item");
    aAnchor.prop("href", notificacion.link );

    let divNotif = $("<div>");
    divNotif.prop("class", "dropdown-notifications-item-icon bg-info");

    divNotif.html("<svg xmlns='<svg xmlns = 'http://www.w3.org/2000/svg' width ='24' height ='24' viewBox = '0 0 24 24' fill = 'none' stroke = 'currentColor' stroke - width='2' stroke - linecap='round' stroke - linejoin='round' class= 'feather feather-bar-chart' ><line x1='12' y1='20' x2='12' y2='10'></line><line x1='18' y1='20' x2='18' y2='4'></line><line x1='6' y1='20' x2='6' y2='16'></line></svg>");
    aAnchor.append(divNotif);

    let divContent = $("<div>");
    divContent.prop("class", "dropdown-notifications-item-content");

    let divDetalle = $("<div>");
    divDetalle.prop("class", "dropdown-notifications-item-content-details small");
    divDetalle.html(notificacion.fechaFormato);
    divContent.append(divDetalle);

    let divText = $("<div>");
    divText.prop("class", "dropdown-notifications-item-content-text");
    divText.html(notificacion.mensaje);

    divContent.append(divText);

    aAnchor.append(divContent);

    $("#divListAlertas").prepend(aAnchor);

    if (notificacion.contadorVisible ==false) {

        let valNotificacion = $("#alertCantidad").html();

        if (valNotificacion == "") {

            $("#alertCantidad").html(1);
        } else {
            $("#alertCantidad").html(1 + parseInt(valNotificacion));

        }
    }

}
