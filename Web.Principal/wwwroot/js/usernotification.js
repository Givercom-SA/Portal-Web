


const connectionUser = new signalR.HubConnectionBuilder().withUrl("/notificaciones")
    .configureLogging(signalR.LogLevel.Error)
    .build();


connectionUser.on("sendToUser", (titulo, mensaje, fecha) => {

    let strItem = "";
    strItem += "<a class='dropdown-item dropdown-notifications-item' href='javascript:void(0)'>";
    strItem += "<div class='dropdown-notifications-item-icon bg-info' >  <svg xmlns='http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24' fill='none' stroke='currentColor'";
    strItem += "stroke-width='2' stroke-linecap='round' stroke-linejoin='round' class='feather feather-bar-chart' >"
    strItem += "<line x1='12' y1='20' x2='12' y2='10'></line>"
    strItem += "<line x1='18' y1='20' x2='18' y2='4'></line><line x1='6' y1='20' x2='6' y2='16'></line></svg > </div >";
    strItem += "<div class='dropdown-notifications-item-content'>";
    strItem += "<div class='dropdown-notifications-item-content-details'> <label class='small'>" + fecha + " </label></div>";
    strItem += "<div class='dropdown-notifications-item-content-text'><label class='small'>" + titulo + "</label></div>";
    strItem += "<div class='dropdown-notifications-item-content-text'><label class='small'>" + mensaje + "</label></div>";

    strItem += "</div>";
    strItem += "</a>";

    $("#divListAlertas").prepend(strItem);
});

connectionUser.start().then(function () {
    //document.getElementById("user").innerHTML = "UserId: " + userId;
    connectionUser.invoke('Matricular',strLayouCodigoUsuario)
        .catch(function (err) { return console.error(err.toString()) })
        .then(function (result) {

            $.each(result, function (index,value) {

                agregarNotificacion(value);
            });

            //document.getElementById('signalRConnectionId').innerHTML = connectionId;

    })
});


function agregarNotificacion() {


}